using System.Linq;
using Bob.Libraries.Extensions.Captcha.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Bob.Libraries.Extensions.Captcha.Providers
{
    public class SessionCaptchaStorageProvider : ICaptchaStorageProvider
    {
        private readonly ILogger<SessionCaptchaStorageProvider> _logger;
        public SessionCaptchaStorageProvider(ILogger<SessionCaptchaStorageProvider> logger)
        {
            _logger = logger;
        }

        public void Add(HttpContext context, string token, string value)
        {
            context.Session?.SetString(token,value);
        }

        public bool Contains(HttpContext context, string token)
        {
            return context.Session.Keys.Contains(token);
        }

        public string GetValue(HttpContext context, string token)
        {
            string cookieValue= context.Session?.GetString(token);
            if (string.IsNullOrEmpty(cookieValue))
            {
                _logger.LogWarning("Couldn't find the captcha value in the request.");
                return null;
            }

            Remove(context, token);

            return cookieValue;
        }

        public void Remove(HttpContext context, string token)
        {
            if (context.Session.Keys.Contains(token))
            {
                context.Session.Remove(token);
            }
        }
    }
}