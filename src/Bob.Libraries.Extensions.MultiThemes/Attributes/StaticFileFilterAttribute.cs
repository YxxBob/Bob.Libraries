using System;
using System.IO;
using System.Text;
using Bob.Libraries.Extensions.MultiThemes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bob.Libraries.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public class StaticPageFilterAttribute : ActionFilterAttribute
    {
        public string Key
        {
            get; set;
        }
        public string[] Keys
        {
            get; set;
        }

        private string GetDirectoryPath(string basePath, string deviceType,string theme)
        {

            var dir = Path.Combine(basePath, "wwwroot", "staticPages");
            if (!string.IsNullOrEmpty(deviceType))
            {
                dir = Path.Combine(dir, deviceType);
            }
            if (!string.IsNullOrEmpty(theme))
            {
                dir = Path.Combine(dir, theme);
            }
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var deviceType = context.HttpContext.Request.GetDeviceType();

            string controllerName = context.RouteData.Values["controller"].ToString().ToLower();
            string actionName = context.RouteData.Values["action"].ToString().ToLower();

            string id = "";

            if (Key != null)
            {
                id = context.RouteData.Values.ContainsKey(Key) ? context.RouteData.Values[Key].ToString() : "";
                if (string.IsNullOrEmpty(id) && context.HttpContext.Request.Query.ContainsKey(Key))
                {
                    id = context.HttpContext.Request.Query[Key];
                }
            }
            var env = context.HttpContext.RequestServices.GetService<IHostingEnvironment>();
            var themeProvider = context.HttpContext.RequestServices.GetService<IThemeProvider>();
            string filePath = Path.Combine(GetDirectoryPath(env.ContentRootPath, deviceType, 
                themeProvider.GetWorkingTheme(context.HttpContext.Request.IsMobileDevice(),context.HttpContext.Request.Host.Host)?.ThemeName), controllerName + "-" + actionName + (string.IsNullOrEmpty(id) ? "" : ("-" + id)) + ".html");
            if (File.Exists(filePath))
            {
                using (var fs = File.Open(filePath, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs, Encoding.UTF8))
                    {
                        var contentresult = new ContentResult
                        {
                            Content = sr.ReadToEnd(),
                            ContentType = "text/html",
                            StatusCode = 200
                        };
                        context.Result = contentresult;
                    }
                }
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            IActionResult actionResult = context.Result;
            if (actionResult is ViewResult)
            {
                ViewResult viewResult = actionResult as ViewResult;
                var services = context.HttpContext.RequestServices;
                var executor = services.GetRequiredService<ViewResultExecutor>();
                var option = services.GetRequiredService<IOptions<MvcViewOptions>>();
                var result = executor.FindView(context, viewResult);
                result.EnsureSuccessful(null);
                var view = result.View;
                StringBuilder builder = new StringBuilder();

                using (var writer = new StringWriter(builder))
                {
                    var viewContext = new ViewContext(
                        context,
                        view,
                        viewResult.ViewData,
                        viewResult.TempData,
                        writer,
                        option.Value.HtmlHelperOptions);

                    view.RenderAsync(viewContext).GetAwaiter().GetResult();
                    writer.Flush();
                }
                string controllerName = context.RouteData.Values["controller"].ToString().ToLower();
                string actionName = context.RouteData.Values["action"].ToString().ToLower();

                string id = "";

                if (Key != null)
                {
                    id = context.RouteData.Values.ContainsKey(Key) ? context.RouteData.Values[Key].ToString() : "";
                    if (string.IsNullOrEmpty(id) && context.HttpContext.Request.Query.ContainsKey(Key))
                    {
                        id = context.HttpContext.Request.Query[Key];
                    }
                }
                var deviceType = context.HttpContext.Request.GetDeviceType();
                var env = context.HttpContext.RequestServices.GetService<IHostingEnvironment>();
                var themeProvider = context.HttpContext.RequestServices.GetService<IThemeProvider>();
                string devicedir = GetDirectoryPath(env.ContentRootPath, deviceType, 
                    themeProvider.GetWorkingTheme(context.HttpContext.Request.IsMobileDevice(), context.HttpContext.Request.Host.Host)?.ThemeName);

                string filePath = Path.Combine(devicedir, controllerName + "-" + actionName + (string.IsNullOrEmpty(id) ? "" : ("-" + id)) + ".html");
                string html = builder.ToString();
                using (FileStream fs = File.Open(filePath, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.Write(html);
                    }
                }
                ContentResult contentresult = new ContentResult
                {
                    Content = html,
                    ContentType = "text/html"
                };
                context.Result = contentresult;
            }
        }
    }
}
