using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;

namespace CS513.MessageHandling.Messages
{
    [Message(MessageCommand.WhisperAck)]
    public class WhisperAck : Message, IMessage
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
    }
}
