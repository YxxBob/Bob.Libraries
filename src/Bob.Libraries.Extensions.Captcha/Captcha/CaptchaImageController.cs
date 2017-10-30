using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Bob.Libraries.Extensions.Captcha.Contracts;
using Bob.Libraries.Extensions.Captcha.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Bob.Libraries.Extensions.Captcha
{
    /// <summary>
    /// Captcha Image Controller
    /// </summary>
    [AllowAnonymous]
    public class CaptchaImageController : Controller
    {
        private readonly ITempDataProvider _tempDataProvider;
        private readonly ILogger<CaptchaImageController> _logger;
        private readonly ICaptchaCodeMain _captchaCodeMain;

        /// <summary>
        /// Captcha Image Controller
        /// </summary>
        public CaptchaImageController(
            ITempDataProvider tempDataProvider,
            ILogger<CaptchaImageController> logger,
            ICaptchaCodeMain captchaCodeMain)
        {

            _tempDataProvider = tempDataProvider;
            _logger = logger;
            _captchaCodeMain = captchaCodeMain;
        }

        /// <summary>
        /// The ViewContext Provider
        /// </summary>
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Refresh the captcha
        /// </summary>
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        public IActionResult Refresh(string rndDate, CaptchaTagHelperHtmlAttributes model)
        {
            if (!IsAjaxRequest())
            {
                return BadRequest();
            }

            if (IsImageHotlinking())
            {
                return BadRequest();
            }

            _captchaCodeMain.Remove(model.CaptchaToken);

            var tagHelper = HttpContext.RequestServices.GetRequiredService<CaptchaTagHelper>();
            tagHelper.Placeholder = model.Placeholder;
            tagHelper.TextBoxClass = model.TextBoxClass;
            tagHelper.ContainerTemplate = model.ContainerTemplate;
            tagHelper.ContainerClass = model.ContainerClass;
            tagHelper.ValidationErrorMessage = model.ValidationErrorMessage;
            tagHelper.ValidationMessageClass = model.ValidationMessageClass;
            tagHelper.RefreshButtonClass = model.RefreshButtonClass;
            tagHelper.CaptchaLength = model.CaptchaLength;
            tagHelper.ImageWidth = model.ImageWidth;
            tagHelper.ImageHeight = model.ImageHeight;
            tagHelper.FontSize = model.FontSize;
            tagHelper.HasNumber = model.HasNumber;
            tagHelper.HasLower = model.HasLower;
            tagHelper.HasUpper = model.HasUpper;

            var tagHelperContext = new TagHelperContext(
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: Guid.NewGuid().ToString("N"));

            var tagHelperOutput = new TagHelperOutput(
                tagName: "div",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent(string.Empty);
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
            tagHelper.ViewContext = ViewContext ?? new ViewContext(
                                        new ActionContext(this.HttpContext, HttpContext.GetRouteData(), ControllerContext.ActionDescriptor),
                                        new FakeView(),
                                        new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                                        {
                                            Model = null
                                        },
                                        new TempDataDictionary(this.HttpContext, _tempDataProvider),
                                        TextWriter.Null,
                                        new HtmlHelperOptions());

            tagHelper.Process(tagHelperContext, tagHelperOutput);

            var attrs = new StringBuilder();
            foreach (var attr in tagHelperOutput.Attributes)
            {
                attrs.Append($" {attr.Name}='{attr.Value}'");
            }

            var content = $"<div {attrs}>{tagHelperOutput.Content.GetContent()}</div>";
            return Content(content);
        }

        /// <summary>
        /// Creates the captcha image.
        /// </summary>
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        public IActionResult Show(string name, int captchaLength, bool hasNumber, bool haslower, bool hasUpper, int imageWidth, int imageHeight, float fontSize, string rndDate)
        {
            if (IsImageHotlinking())
            {
                return BadRequest();
            }
            byte[] image;
            try
            {
                image = _captchaCodeMain.GeneratorCaptcha(name, captchaLength, hasNumber, haslower, hasUpper, imageWidth, imageHeight, fontSize);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(1001, ex, "DrawCaptcha error.");
                return BadRequest(ex.Message);
            }
            return new FileContentResult(image, "image/png");
        }

        private bool IsAjaxRequest()
        {
            return Request?.Headers != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private bool IsImageHotlinking()
        {
            var applicationUrl = $"{Request.Scheme}://{Request.Host.Value}";
            var urlReferrer = (string)Request.Headers[HeaderNames.Referer];
            return string.IsNullOrEmpty(urlReferrer) ||
                   !urlReferrer.StartsWith(applicationUrl, StringComparison.OrdinalIgnoreCase);
        }
    }
}