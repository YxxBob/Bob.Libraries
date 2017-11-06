using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bob.Libraries.Extensions.MonitorWeb.Models;
using Newtonsoft.Json;

namespace Bob.Libraries.Extensions.MonitorWeb.Controllers
{
    public class WebServerCore
    {
        HttpClient client = null;

        public HttpClient Client
        {
            get
            {
                if (client == null)
                {
                    client = new HttpClient
                    {
                        Timeout = TimeSpan.FromSeconds(1)
                    };
                }
                return client;
            }
        }

        private async Task<TResult> SendAsync<TResult>(string url) where TResult : class
        {

            try
            {
                string response = await Client.GetStringAsync(url);
                TResult result;
                if (typeof(TResult) == typeof(String))
                {
                    result = response as TResult;
                }
                else
                {
                    result = JsonConvert.DeserializeObject<TResult>(response);
                }
                return result;
            }
            catch (System.Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取系统信息
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public SystemInfoModel GetSystemInfo(HostModel host)
        {
            var infoTask = SendAsync<SystemInfoModel>(host.Host + "/WebSite/SystemInfo");
            infoTask.Wait();
            var model = infoTask.Result;
            if (model == null)
            {
                model = new SystemInfoModel();
            }
            var aliveTask = KeepAlive(host.Host);
            aliveTask.Wait();
            var aliveResult = aliveTask.Result;
            model.ETime = aliveResult.etime;
            model.IsAlive = aliveResult.isAlive;
            model.HttpHost = host.Host;
            model.HostName = host.Name;
            return model;
        }

        public async Task<(bool isAlive, int etime)> KeepAlive(string domain)
        {
            var start = DateTime.Now;
            var res = await SendAsync<string>(domain + "/WebSite/KeepAlive");
            var end = DateTime.Now;
            if (res == "别担心,我还活着!")
            {
                return (true, (end - start).Milliseconds);
            }
            return (false, 0);
        }

    }

}