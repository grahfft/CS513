using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;

namespace CS513.MessageHandling.Messages
{
    [Message(MessageCommand.ChatAck)]
    public class ChatResponse : Message, IMessage
    {
        public ChatResponse()
        {
            this.Command = MessageCommand.ChatAck;
        }

        public ChatResponse(string sender, string receiver, string contents)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Contents = contents;
            this.Command = MessageCommand.ChatAck;
        }
    }
}
