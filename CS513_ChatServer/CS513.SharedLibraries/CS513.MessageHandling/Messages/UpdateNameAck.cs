using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;

namespace CS513.MessageHandling.Messages
{
    [Message(MessageCommand.UpdateAck)]
    public class UpdateNameAck : Message, IMessage
    {
        public UpdateNameAck()
        {
            this.Command = MessageCommand.UpdateAck;
        }

        public UpdateNameAck(string sender, string receiver, string contents)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Contents = contents;
            this.Command = MessageCommand.UpdateAck;
        }
    }
}
