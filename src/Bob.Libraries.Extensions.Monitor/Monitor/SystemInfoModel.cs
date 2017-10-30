using System;

namespace Bob.Libraries.Extensions.Monitor
{
    /// <summary>
    /// 系统信息
    /// </summary>
    public class SystemInfoModel
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 服务器名称
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// ASPNET信息
        /// </summary>
        public string AspNetInfo { get; set; }

        public string IsFullTrust { get; set; }

        /// <summary>
        /// App版本
        /// </summary>
        public string AppVersion { get; set; }

        /// <summary>
        /// 程序名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string OperatingSystem { get; set; }

        /// <summary>
        /// 服务器当前时间
        /// </summary>
        public DateTime ServerLocalTime { get; set; }

        /// <summary>
        /// 服务器时区
        /// </summary>
        public string ServerTimeZone { get; set; }

        /// <summary>
        /// Host
        /// </summary>
        public string HttpHost { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime ReleaseDate { get; set; }
    }
}