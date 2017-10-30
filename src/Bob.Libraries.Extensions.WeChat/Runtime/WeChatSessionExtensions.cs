using System;
using Bob.Libraries.Extensions.WeChat.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Bob.Libraries.Extensions.WeChat.Runtime
{
    public static class WeChatSessionExtensions
    {
        /// <summary>
        /// 设置当前Session的OAuthAccessToken
        /// </summary>
        /// <param name="session"></param>
        /// <param name="data"></param>
        public static void SetOAuthAccessToken(this ISession session, OAuthAccessToken data)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }
            if (data == null)
            {
                session.Remove(nameof(OAuthAccessToken));
            }
            else
            {
                session.SetString(nameof(OAuthAccessToken), JsonConvert.SerializeObject(data));
            }
        }
        /// <summary>
        /// 获取当前Session的OAuthAccessToken
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static OAuthAccessToken GetOAuthAccessToken(this ISession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }
            var str = session.GetString(nameof(OAuthAccessToken));
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<OAuthAccessToken>(str);
        }
    }
}
