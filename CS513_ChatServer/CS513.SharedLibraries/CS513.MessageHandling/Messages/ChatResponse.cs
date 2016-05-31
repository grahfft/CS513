using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Client;
using CS513.Interfaces.Shared;

namespace CS513.MessageHandling.Messages
{
    [Message(MessageCommand.ChatAck)]
    public class ChatResponse : Message, IResponse
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

        public void ProcessMessage(IUserManager userManager, ILog log)
        {
            log.LogChat(this.Sender, this.Contents);
        }
    }
}
