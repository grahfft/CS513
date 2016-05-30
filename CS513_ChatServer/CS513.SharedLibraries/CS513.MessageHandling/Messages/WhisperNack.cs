using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;

namespace CS513.MessageHandling.Messages
{
    [Message(MessageCommand.WhisperNack)]
    public class WhisperNack : Message, IMessage
    {
        public WhisperNack()
        {
            this.Command = MessageCommand.WhisperNack;
        }

        public WhisperNack(string sender, string receiver, string contents)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Contents = contents;
            this.Command = MessageCommand.WhisperNack;
        }
    }
}
