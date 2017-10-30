using Bob.Libraries.Extensions.WeChat.Runtime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bob.Libraries.Extensions.WeChat.Attributes
{
    public class WeChatAutoLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.IsWeChatBrowser())
            {
                if (context.HttpContext.Session.GetOAuthAccessToken() == null)
                {
                    context.Result = new RedirectToRouteResult("WeChatLogin",
                        new { returnUrl = context.HttpContext.Request.Path.ToString() });
                }
            }
            else
            {
                base.OnActionExecuting(context);
            }
        }

    }
}
