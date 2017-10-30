using Bob.Libraries.Extensions.Captcha;
using Bob.Libraries.Extensions.Captcha.Contracts;
using Bob.Libraries.Extensions.Captcha.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bob.Libraries.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services required for captcha support
        /// </summary>
        public static void AddCaptcha(this IServiceCollection services, StorageMode storageMode = StorageMode.Session)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            switch (storageMode)
            {
                case StorageMode.Session:
                    services.AddTransient<ICaptchaStorageProvider, SessionCaptchaStorageProvider>();
                    break;
                case StorageMode.Cookie:
                    services.AddTransient<ICaptchaStorageProvider, CookieCaptchaStorageProvider>();
                    break;
                case StorageMode.MemoryCache:
                    services.AddTransient<ICaptchaStorageProvider, MemoryCacheCaptchaStorageProvider>();
                    break;
            }

            services.AddTransient<ICaptchaImageProvider, CaptchaImageProvider>();
            services.AddTransient<ICaptchaProtectionProvider, CaptchaProtectionProvider>();
            services.AddTransient<ICaptchaCodeGenerator, CaptchaCodeGenerator>();
            services.AddTransient<ICaptchaCodeMain, CaptchaCodeMain>();
            services.AddTransient<CaptchaTagHelper>();
        }
    }
    public enum StorageMode
    {
        Cookie = 1,
        Session = 2,
        MemoryCache = 3
    }
}
