using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;

namespace Bob.Libraries.Extensions.Monitor
{
    public class WebSiteController : Controller
    {
        public SystemInfoModel SystemInfo()
        {
            var app = PlatformServices.Default.Application;
            var model = new SystemInfoModel();
            model.AppVersion = app.ApplicationVersion;
            try
            {
                model.MachineName = Environment.MachineName;
                model.OperatingSystem = RuntimeInformation.OSDescription;
            }
            catch (Exception) { }
            try
            {
                model.AspNetInfo = app.RuntimeFramework.FullName;// RuntimeInformation.FrameworkDescription;
            }
            catch (Exception) { }
            try
            {
                model.IsFullTrust = AppDomain.CurrentDomain.IsFullyTrusted.ToString();
            }
            catch (Exception) { }
            model.AppName = app.ApplicationName;
            model.ServerTimeZone = TimeZoneInfo.Local.StandardName;
            model.ServerLocalTime = DateTime.Now;
            model.HttpHost = HttpContext.Request.Host.Host;
            model.IPAddress = GetServerIP();
            DirectoryInfo info = new DirectoryInfo(app.ApplicationBasePath);
            var files = info.GetFiles("*.dll", SearchOption.TopDirectoryOnly);
            model.ReleaseDate = files.Max(f => f.LastWriteTime);

            return model;
        }

        public IActionResult KeepAlive()
        {
            return Content("别担心,我还活着!");
        }

        private static string ServerIP = null;
        private string GetServerIP()
        {
            if (string.IsNullOrEmpty(ServerIP))
            {
                //var url =
                //    "https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=0&rsv_idx=1&tn=baidu&wd=IP&rsv_pq=db4eb7d40002dd86&rsv_t=14d7uOUvNnTdrhnrUx0zdEVTPEN8XDq4aH7KkoHAEpTIXkRQkUD00KJ2p94&rqlang=cn&rsv_enter=1&rsv_sug3=2&rsv_sug1=2&rsv_sug7=100&rsv_sug2=0&inputT=875&rsv_sug4=875";

                //var htmlContent = Senparc.Weixin.HttpUtility.RequestUtility.HttpGet(url, cookieContainer: null);
                //var result = Regex.Match(htmlContent, @"(?<=本机IP:[^\d+]*)(\d+\.\d+\.\d+\.\d+)(?=</span>)");
                var url = "http://2017.ip138.com/ic.asp";
                var task = new HttpClient().GetStringAsync(url);
                task.Wait();
                var htmlContent = task.Result;
                var result = Regex.Match(htmlContent, @"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}");
                if (result.Success)
                {
                    ServerIP = result.Value;
                }
            }
            return ServerIP;
        }

        public const string SystemInfoApiUrl = "/WebSite/SystemInfo";

        public const string KeepAliveApiUrl = "/WebSite/KeepAlive";
    }
}