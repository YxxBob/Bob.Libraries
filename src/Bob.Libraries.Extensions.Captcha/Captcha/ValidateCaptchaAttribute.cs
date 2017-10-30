using System;
using Bob.Libraries.Extensions.Captcha.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Bob.Libraries.Extensions.Captcha
{
    /// <summary>
    /// Represents a filter attribute enabling CAPTCHA validation
    /// </summary>
    public class ValidateCaptchaAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Create instance of the filter attribute 
        /// </summary>
        /// <param name="actionParameterName">The name of the action parameter to which the result will be passed</param>
        public ValidateCaptchaAttribute(string actionParameterName = "captchaValid") : base(
            typeof(ValidateCaptchaFilter))
        {
            this.Arguments = new object[] { actionParameterName };
        }
        #region Nested filter

        /// <summary>
        /// Represents a filter enabling CAPTCHA validation
        /// </summary>
        private class ValidateCaptchaFilter : IActionFilter
        {

            #region Fields

            private readonly string _actionParameterName;

            #endregion

            #region Ctor

            public ValidateCaptchaFilter(string actionParameterName)
            {
                this._actionParameterName = actionParameterName;
            }

            #endregion

            #region Utilities

            /// <summary>
            /// Validate CAPTCHA
            /// </summary>
            /// <param name="context">A context for action filters</param>
            /// <returns>True if CAPTCHA is valid; otherwise false</returns>
            protected bool ValidateCaptcha(ActionExecutingContext context)
            {
                var storage = context.HttpContext.RequestServices.GetService<ICaptchaCodeMain>();

                var form = context.HttpContext.Request.Form;
                var captchaName = (string)form[CaptchaTagHelper.CaptchaHiddenTokenName];
                var inputText = (string)form[CaptchaTagHelper.CaptchaInputName];

                return storage.VerifyCaptcha(captchaName, inputText);
            }

            #endregion

            #region Methods

            /// <summary>
            /// Called before the action executes, after model binding is complete
            /// </summary>
            /// <param name="context">A context for action filters</param>
            public void OnActionExecuting(ActionExecutingContext context)
            {
                if (context == null)
                    return;

                //whether CAPTCHA is enabled
                if (context.HttpContext?.Request != null && string.Equals("POST", context.HttpContext.Request.Method, StringComparison.OrdinalIgnoreCase))
                {
                    //push the validation result as an action parameter
                    context.ActionArguments[_actionParameterName] = ValidateCaptcha(context);
                }
                else
                    context.ActionArguments[_actionParameterName] = false;

            }

            /// <summary>
            /// Called after the action executes, before the action result
            /// </summary>
            /// <param name="context">A context for action filters</param>
            public void OnActionExecuted(ActionExecutedContext context)
            {
                //do nothing
            }

            #endregion
        }

        #endregion
    }
}
