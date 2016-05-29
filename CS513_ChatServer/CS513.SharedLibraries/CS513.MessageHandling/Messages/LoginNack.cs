using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;

namespace CS513.MessageHandling.Messages
{
    [Message(MessageCommand.LoginNack)]
    public class LoginNack : Message, IMessage
    {
        public LoginNack()
        {
            this.Command = MessageCommand.LoginNack;
        }

        public LoginNack(string sender, string receiver, string contents)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Contents = contents;
            this.Command = MessageCommand.LoginNack;
        }
    }
}
