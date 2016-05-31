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
    [Message(MessageCommand.WhisperAck)]
    public class WhisperAck : Message, IResponse
    {
        public WhisperAck()
        {
            this.Command = MessageCommand.WhisperAck;
        }

        public WhisperAck(string sender, string receiver, string contents)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Contents = contents;
            this.Command = MessageCommand.WhisperAck;
        }

        public void ProcessMessage(IUserManager userManager, ILog log)
        {
            log.LogWhisper(this.Sender, this.Contents);
        }
    }
}
