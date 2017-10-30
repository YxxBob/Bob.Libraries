using Senparc.Weixin.Entities;

namespace Bob.Libraries.Extensions.WeChat.Models
{
    /// <summary>
    /// 微信配置
    /// </summary>
    public class WeChatSetting : SenparcWeixinSetting
    {
        /// <summary>
        /// 回调地址
        /// </summary>
        public string LoginCallbackUrl { get; set; }
    }
}