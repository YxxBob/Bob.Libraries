using System.IO;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;

namespace Bob.Libraries.Extensions.WeChat.Handlers
{
    public class CustomMessageHandler : MessageHandler<CustomeMessageContext>
    {
        public CustomMessageHandler(Stream inputStream, PostModel postModel) : 
            base(inputStream, postModel)
        {
        }
        

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这条消息来自DefaultResponseMessage。";
            return responseMessage;
        }

        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            return base.OnTextRequest(requestMessage);
        }

        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var response = base.CreateResponseMessage<ResponseMessageText>();
            response.Content = "谢谢关注。";
            return response;
        }
        
    }

    public class CustomeMessageContext : MessageContext<IRequestMessageBase, IResponseMessageBase>
    {
        
    }
}
