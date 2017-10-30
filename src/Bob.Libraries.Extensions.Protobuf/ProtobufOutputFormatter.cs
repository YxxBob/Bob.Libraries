using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Bob.Libraries.Extensions.Protobuf
{
    public class ProtobufOutputFormatter : OutputFormatter
    {
        private static MediaTypeHeaderValue protoMediaType = MediaTypeHeaderValue.Parse(Consts.ApplicationProtobuf);

        private static Lazy<RuntimeTypeModel> model = new Lazy<RuntimeTypeModel>(CreateTypeModel);

        public string ContentType { get; private set; }

        public static RuntimeTypeModel Model => model.Value;

        public ProtobufOutputFormatter()
        {
            ContentType = Consts.ApplicationProtobuf;
            SupportedMediaTypes.Add(protoMediaType);
        }

        protected override bool CanWriteType(Type type)
        {
            var isCan = type.GetCustomAttributes(typeof(ProtoContractAttribute), false) != null;
            if (!isCan && typeof(IEnumerable).IsAssignableFrom(type))
            {
                var temp = type.GetGenericArguments().FirstOrDefault();
                isCan = temp?.GetCustomAttributes(typeof(ProtoContractAttribute), false) != null;
            }
            return isCan;
        }

        public override bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            if (context.Object == null || !context.ContentType.HasValue)
            {
                return false;
            }
            MediaTypeHeaderValue requestContentType = null;

            MediaTypeHeaderValue.TryParse(context.ContentType.Value, out requestContentType);
            if (requestContentType == null || !requestContentType.IsSubsetOf(protoMediaType))
            {
                return false;
            }
            if (!CanWriteType(context.ObjectType))
            {
                return false;
            }
            return true;
        }

        private static RuntimeTypeModel CreateTypeModel()
        {
            var typeModel = TypeModel.Create();
            typeModel.UseImplicitZeroDefaults = false;
            return typeModel;
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = this.ContentType;

            Model.Serialize(response.Body, context.Object);
            return Task.FromResult(response);
        }
    }
}