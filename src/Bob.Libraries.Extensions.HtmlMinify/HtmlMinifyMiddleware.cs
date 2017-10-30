using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Bob.Libraries.Extensions.HtmlMinify
{
    public class HtmlMinifyMiddleware
    {
        private RequestDelegate _next;
        private HtmlMinifyOptions _minificationOptions;
        public HtmlMinifyMiddleware(RequestDelegate next)
            : this(next, null)
        {
        }
        public HtmlMinifyMiddleware(RequestDelegate next, HtmlMinifyOptions minificationOptions)
        {
            _next = next;
            _minificationOptions = minificationOptions;
        }
        public async Task Invoke(HttpContext context)
        {
            var stream = context.Response.Body;
            if (_minificationOptions != null)
            {
                var filter = _minificationOptions.ExcludeFilter;
                if (Regex.IsMatch(context.Request.Path, filter))
                {
                    await _next(context);
                    return;
                }
            }
            var bufferStream = new MemoryStream();

            await _next(context);

            if (!context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.ContentType].ToString().Contains("text/html"))
            {
                return;
            }
            var reader = new StreamReader(bufferStream);
            string responseBody = await reader.ReadToEndAsync();
            responseBody = Regex.Replace(responseBody,
                @">\s+<", "><", RegexOptions.Compiled);
            responseBody = Regex.Replace(responseBody,
                @"<!--(?!\s*(?:\[if [^\]]+]|<!|>))(?:(?!-->)(.|\n))*-->", "", RegexOptions.Compiled);
            context.Response.Body = bufferStream;


        }
    }
}
