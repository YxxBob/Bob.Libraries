using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Bob.Libraries.Extensions.Captcha
{
    /// <summary>
    /// Tag helper attributes
    /// </summary>
    public class CaptchaTagHelperHtmlAttributes
    {
      
        [HtmlAttributeName("asp-captcha-name")]
        public string Name { set; get; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 文本框Placeholder
        /// </summary>
        [HtmlAttributeName("asp-textbox-placeholder")]
        public string Placeholder { set; get; } = "请输入验证码";

        /// <summary>
        /// 验证码Div容器样式
        /// </summary>
        [HtmlAttributeName("asp-container-class")]
        public string ContainerClass { set; get; } = "";

        /// <summary>
        /// 文本框样式
        /// </summary>
        [HtmlAttributeName("asp-textbox-class")]
        public string TextBoxClass { set; get; } = "text-box single-line form-control col-md-4";

        /// <summary>
        /// 显示格式
        /// </summary>
        [HtmlAttributeName("asp-container-template")]
        public string ContainerTemplate { set; get; } = "{Textbox}{Image}{RefreshBtn}";

        /// <summary>
        /// 验证码图片宽度
        /// </summary>
        [HtmlAttributeName("asp-image-width")]
        public int ImageWidth { set; get; } = 60;

        /// <summary>
        /// 验证码图片高度
        /// </summary>
        [HtmlAttributeName("asp-image-height")]
        public int ImageHeight { set; get; } = 30;

        /// <summary>
        /// 验证码字体大小
        /// </summary>
        [HtmlAttributeName("asp-image-fontsize")]
        public int FontSize { set; get; } = 15;

        /// <summary>
        /// 验证码长度
        /// </summary>
        [HtmlAttributeName("asp-captcha-length")]
        public int CaptchaLength { set; get; } = 4;

        /// <summary>
        /// 验证码生成规则包含数字
        /// </summary>
        [HtmlAttributeName("asp-captcha-hasnumber")]
        public bool HasNumber { set; get; } = true;

        /// <summary>
        /// 验证码生成规则包含小写字母
        /// </summary>
        [HtmlAttributeName("asp-captcha-haslower")]
        public bool HasLower { set; get; } = true;

        /// <summary>
        /// 验证码生成规则包含大写字母
        /// </summary>
        [HtmlAttributeName("asp-captcha-hasupper")]
        public bool HasUpper { set; get; } = true;

        /// <summary>
        /// 错误提示
        /// </summary>
        [HtmlAttributeName("asp-validation-error-message")]
        public string ValidationErrorMessage { set; get; } = "请输入验证码";

        /// <summary>
        /// 错误提示样式
        /// </summary>
        [HtmlAttributeName("asp-validation-message-class")]
        public string ValidationMessageClass { set; get; } = "text-danger";

        /// <summary>
        /// 刷新按钮样式
        /// </summary>
        [HtmlAttributeName("asp-refresh-button-class")]
        public string RefreshButtonClass { set; get; } = "glyphicon glyphicon-refresh btn-sm";

        /// <summary>
        /// The Captcha Token
        /// </summary>
        [HtmlAttributeNotBound]
        public string CaptchaToken { set; get; }
    }
}