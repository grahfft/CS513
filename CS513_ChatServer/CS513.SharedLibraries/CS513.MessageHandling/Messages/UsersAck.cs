using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Shared;

namespace CS513.MessageHandling.Messages
{
    [Message(MessageCommand.UsersAck)]
    public class UsersAck : Message, IMessage
    {
        public UsersAck()
        {
            this.Command = MessageCommand.UsersAck;
        }

        public UsersAck(string sender, string receiver, string contents)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Contents = contents;
            this.Command = MessageCommand.UsersAck;
        }
    }
}
