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
    [Message(MessageCommand.UpdateAck)]
    public class UpdateNameAck : Message, IResponse
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

        public void ProcessMessage(IUserManager userManager, ILog log)
        {
            log.LogMessage(string.Format("{0} has updated his name to {1}", this.Sender, this.Contents));
            userManager.UpdateUserName(this.Sender, this.Contents);
        }
    }
}
