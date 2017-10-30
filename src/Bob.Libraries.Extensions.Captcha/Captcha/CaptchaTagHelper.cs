using System;
using System.Linq;
using System.Threading.Tasks;
using Bob.Libraries.Extensions.Captcha.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Bob.Libraries.Extensions.Captcha
{
    /// <summary>
    /// Captcha TagHelper
    /// </summary>
    [HtmlTargetElement("captcha")]
    public class CaptchaTagHelper : CaptchaTagHelperHtmlAttributes, ITagHelper
    {
        /// <summary>
        /// The default hidden input name of the captcha's cookie token.
        /// </summary>
        public const string CaptchaHiddenTokenName = "CaptchaToken";

        /// <summary>
        /// The default input name of the captcha.
        /// </summary>
        public const string CaptchaInputName = "CaptchaInputText";

        /// <summary>
        /// Default order is <c>0</c>.
        /// </summary>
        public int Order { get; } = 0;

        /// <summary>
        /// The current ViewContext.
        /// </summary>
        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        /// <inheritdoc />
        public void Init(TagHelperContext context)
        {
        }

        /// <summary>
        /// Process the taghelper and generate the output.
        /// </summary>
        public void Process(TagHelperContext context, TagHelperOutput output)
        {

            output.TagName = "div";
            output.Attributes.Add("class", ContainerClass);
            var captchaDivId = $"Captcha{context.UniqueId}{Name}";
            output.Attributes.Add("id", captchaDivId);
            output.TagMode = TagMode.StartTagAndEndTag;

            var captchaImage = GetCaptchaImageTagBuilder();
            //output.Content.AppendHtml(captchaImage);

            var refreshButton = GetRefreshButtonTagBuilder();
            //output.Content.AppendHtml(refreshButton);

            var textInput = GetTextInputTagBuilder();
            //output.Content.AppendHtml($"{string.Format(TextBoxTemplate, textInput.GetString())}");

            output.Content.AppendHtml(ContainerTemplate.Replace("{Textbox}", textInput.GetString()).Replace("{Image}", captchaImage.GetString()).Replace("{RefreshBtn}", refreshButton.GetString()));

            var validationMessage = GetValidationMessageTagBuilder();
            output.Content.AppendHtml(validationMessage);

            var hiddenInputToken = GetHiddenInputTokenTagBuilder(Name);
            output.Content.AppendHtml(hiddenInputToken);
        }

        /// <summary>
        /// Asynchronously executes the <see cref="TagHelper"/> with the given <paramref name="context"/> and
        /// <paramref name="output"/>.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        /// <returns>A <see cref="Task"/> that on completion updates the <paramref name="output"/>.</returns>
        /// <remarks>By default this calls into <see cref="Process"/>.</remarks>.
        public Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Process(context, output);
            return Task.CompletedTask;
        }

        private static TagBuilder GetHiddenInputTokenTagBuilder(string token)
        {
            var hiddenInput = new TagBuilder("input");
            hiddenInput.Attributes.Add("id", CaptchaHiddenTokenName);
            hiddenInput.Attributes.Add("name", CaptchaHiddenTokenName);
            hiddenInput.Attributes.Add("type", "hidden");
            hiddenInput.Attributes.Add("value", token);
            return hiddenInput;
        }

        private TagBuilder GetCaptchaImageTagBuilder()
        {

            IUrlHelper urlHelper = new UrlHelper(ViewContext);
            var actionUrl = urlHelper.Action(action: nameof(CaptchaImageController.Show),
                controller: nameof(CaptchaImageController).Replace("Controller", string.Empty),
                values:
                new
                {
                    name = Name,
                    this.ImageWidth,
                    this.ImageHeight,
                    this.CaptchaLength,
                    this.HasNumber,
                    HasLower,
                    HasUpper,
                    fontSize = FontSize,
                    rndDate = DateTime.Now.Ticks,
                    area = ""
                });

            var captchaImage = new TagBuilder("img");
            var dntCaptchaImg = "CaptchaImg";
            captchaImage.Attributes.Add("id", dntCaptchaImg);
            captchaImage.Attributes.Add("name", dntCaptchaImg);
            captchaImage.Attributes.Add("alt", "captcha");
            captchaImage.Attributes.Add("src", actionUrl);
            captchaImage.Attributes.Add("onclick", $"this.src='{actionUrl}?' + Math.random()");
            return captchaImage;
        }

        private TagBuilder GetRefreshButtonTagBuilder()
        {
            IUrlHelper urlHelper = new UrlHelper(ViewContext);
            var actionUrl = urlHelper.Action(action: nameof(CaptchaImageController.Show),
                controller: nameof(CaptchaImageController).Replace("Controller", string.Empty),
                values:
                new
                {
                    name = Name,
                    this.ImageWidth,
                    this.ImageHeight,
                    this.CaptchaLength,
                    this.HasNumber,
                    HasLower,
                    HasUpper,
                    fontSize = FontSize,
                    rndDate = DateTime.Now.Ticks,
                    area = ""
                });

            var refreshButton = new TagBuilder("a");
            var dntCaptchaRefreshButton = "CaptchaRefreshButton";
            refreshButton.Attributes.Add("id", dntCaptchaRefreshButton);
            refreshButton.Attributes.Add("name", dntCaptchaRefreshButton);
            refreshButton.Attributes.Add("href", "#refresh");
            refreshButton.Attributes.Add("onclick", $"document.getElementById('CaptchaImg').src='{actionUrl}?' + Math.random()");
            refreshButton.Attributes.Add("class", RefreshButtonClass);
            return refreshButton;
        }

        private TagBuilder GetTextInputTagBuilder()
        {
            var textInput = new TagBuilder("input");
            textInput.Attributes.Add("id", CaptchaInputName);
            textInput.Attributes.Add("name", CaptchaInputName);
            textInput.Attributes.Add("autocomplete", "off");
            textInput.Attributes.Add("class", TextBoxClass);
            textInput.Attributes.Add("data-val", "true");
            textInput.Attributes.Add("data-val-required", ValidationErrorMessage);
            textInput.Attributes.Add("placeholder", Placeholder);
            textInput.Attributes.Add("dir", "ltr");
            textInput.Attributes.Add("type", HasNumber && !HasLower && !HasUpper ? "number" : "text");
            textInput.Attributes.Add("value", "");
            return textInput;
        }

        private TagBuilder GetValidationMessageTagBuilder()
        {
            var validationMessage = new TagBuilder("span");
            validationMessage.Attributes.Add("class", ValidationMessageClass);
            validationMessage.Attributes.Add("data-valmsg-for", CaptchaInputName);
            validationMessage.Attributes.Add("data-valmsg-replace", "true");

            if (!ViewContext.ModelState.IsValid)
            {
                ModelStateEntry captchaInputNameValidationState;
                if (ViewContext.ModelState.TryGetValue(CaptchaInputName, out captchaInputNameValidationState))
                {
                    if (captchaInputNameValidationState.ValidationState == ModelValidationState.Invalid)
                    {
                        var error = captchaInputNameValidationState.Errors.FirstOrDefault();
                        if (error != null)
                        {
                            var errorSpan = new TagBuilder("span");
                            errorSpan.InnerHtml.AppendHtml(error.ErrorMessage);
                            validationMessage.InnerHtml.AppendHtml(errorSpan);
                        }
                    }
                }
            }

            return validationMessage;
        }
    }
}