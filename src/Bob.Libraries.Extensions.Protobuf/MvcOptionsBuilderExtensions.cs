using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Bob.Libraries.Extensions.Protobuf
{
    public static class MvcOptionsBuilderExtensions
    {
        /// <summary>
        /// 添加PROTOBUF格式序列化器
        /// </summary>
        /// <param name="options"></param>
        public static void AddProtobuf(this MvcOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            options.OutputFormatters.Add(new ProtobufOutputFormatter());
            options.InputFormatters.Add(new ProtobufInputFormatter());
            options.FormatterMappings.SetMediaTypeMappingForFormat("protobuf",
                MediaTypeHeaderValue.Parse(Consts.ApplicationProtobuf));
        }

    }
}