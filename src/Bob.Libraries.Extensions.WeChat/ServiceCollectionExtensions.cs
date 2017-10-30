using Bob.Libraries.Extensions.WeChat.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bob.Libraries.Extensions.WeChat
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加微信配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddWeChatConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<WeChatSetting>(configuration.GetSection("WeChatSetting"));
        }
    }
}
