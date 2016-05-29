using CS513.Interfaces;
using CS513.Interfaces.Shared;

namespace CS513.MessageHandling
{
    public class MessageHandler : IMessageHandler
    {


        public MessageHandler()
        {
            
        }

        public IMessage GetMessage(byte[] data)
        {
            return null;
        }

        public IMessage GetMessage(IMessageHeader header, string message)
        {
            return null;
        }
    }
}
