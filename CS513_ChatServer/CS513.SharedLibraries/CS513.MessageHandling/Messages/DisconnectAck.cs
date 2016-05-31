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
    [Message(MessageCommand.DisconnectResponse)]
    public class DisconnectAck : Message, IResponse
    {
        public DisconnectAck()
        {
            this.Command = MessageCommand.DisconnectResponse;
        }

        public DisconnectAck(string sender, string receiver, string contents)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Contents = contents;
            this.Command = MessageCommand.DisconnectResponse;
        }

        public void ProcessMessage(IUserManager userManager, ILog log)
        {
            log.LogMessage(string.Format("{0} has left", this.Sender));
            userManager.RemoveUser(this.Sender);
        }
    }
}
