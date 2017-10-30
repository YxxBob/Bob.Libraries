using Microsoft.AspNetCore.Builder;

namespace Bob.Libraries.Extensions.HtmlMinify
{
    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseHtmlMinify(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HtmlMinifyMiddleware>();
        }
        public static IApplicationBuilder UseHtmlMinify(this IApplicationBuilder app,
            string excludeFilter)
        {
            var options = new HtmlMinifyOptions() { ExcludeFilter = excludeFilter };
            return app.UseMiddleware<HtmlMinifyMiddleware>(options);
        }
        public static IApplicationBuilder UseHtmlMinify(this IApplicationBuilder app,
            HtmlMinifyOptions minificationOptions)
        {
            return app.UseMiddleware<HtmlMinifyMiddleware>(minificationOptions);
        }
    }
}