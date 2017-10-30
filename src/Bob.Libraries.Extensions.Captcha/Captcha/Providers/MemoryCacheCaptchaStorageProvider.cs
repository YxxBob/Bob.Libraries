using Bob.Libraries.Extensions.Captcha.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Bob.Libraries.Extensions.Captcha.Providers
{
    class MemoryCacheCaptchaStorageProvider : ICaptchaStorageProvider
    {
        public void Add(HttpContext context, string token, string value)
        {
            var memoryCache = context.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;
            memoryCache.Set(token, value);
        }

        public bool Contains(HttpContext context, string token)
        {
            var memoryCache = context.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;
            return memoryCache.TryGetValue(token, out object value);
        }

        public string GetValue(HttpContext context, string token)
        {
            var memoryCache = context.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;
            return memoryCache.Get<string>(token);
        }

        public void Remove(HttpContext context, string token)
        {
            var memoryCache = context.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;
            memoryCache.Remove(token);
        }
    }
}
