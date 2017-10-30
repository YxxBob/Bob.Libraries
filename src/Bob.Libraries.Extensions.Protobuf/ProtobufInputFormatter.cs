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
    public class ProtobufInputFormatter : InputFormatter
    {
        public ProtobufInputFormatter()
        {
            SupportedMediaTypes.Add(protoMediaType);
        }
        private static Lazy<RuntimeTypeModel> model = new Lazy<RuntimeTypeModel>(CreateTypeModel);
        private static MediaTypeHeaderValue protoMediaType = MediaTypeHeaderValue.Parse(Consts.ApplicationProtobuf);

        public static RuntimeTypeModel Model => model.Value;

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var type = context.ModelType;
            object result = Model.Deserialize(context.HttpContext.Request.Body, null, type);
            return InputFormatterResult.SuccessAsync(result);
        }

        protected override bool CanReadType(Type type)
        {
            var isCan = type.GetCustomAttributes(typeof(ProtoContractAttribute),false) != null;
            if (!isCan && typeof(IEnumerable).IsAssignableFrom(type))
            {
                var temp = type.GetGenericArguments().FirstOrDefault();
                isCan = temp != null && temp.GetCustomAttributes(typeof(ProtoContractAttribute), false) != null;
            }
            return isCan;
        }

        public override bool CanRead(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            MediaTypeHeaderValue requestContentType = null;
            MediaTypeHeaderValue.TryParse(request.ContentType, out requestContentType);

            if (requestContentType == null)
            {
                return false;
            }

            return requestContentType.IsSubsetOf(protoMediaType);
        }

        private static RuntimeTypeModel CreateTypeModel()
        {
            var typeModel = TypeModel.Create();
            typeModel.UseImplicitZeroDefaults = false;
            return typeModel;
        }
    }
}
