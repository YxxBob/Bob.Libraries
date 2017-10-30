using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Bob.Libraries.Extensions.WeChat.Runtime
{
    public static class WeChatRequestExtensions
    {
        /// <summary>
        /// 是否是微信
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsWeChatBrowser(this HttpRequest request)
        {
            var userAgent = request.Headers[HeaderNames.UserAgent].ToString();
            return userAgent != null && userAgent.Contains("MicroMessenger");
        }
        /// <summary>
        /// 获取微信版本号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static decimal GetWeChatVersion(this HttpRequest request)
        {
            if (IsWeChatBrowser(request))
            {
                var userAgent = request.Headers[HeaderNames.UserAgent].ToString();
                int idx = userAgent.IndexOf("MicroMessenger", StringComparison.Ordinal);
                string vs = userAgent.Substring(idx + "MicroMessenger".Length + 1, 3);
                var version = Convert.ToDecimal(vs);
                return version;
            }
            else
            {
                return 0;
            }
        }
    }
}