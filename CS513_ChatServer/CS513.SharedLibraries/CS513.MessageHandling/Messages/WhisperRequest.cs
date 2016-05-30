using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Server;
using CS513.Interfaces.Shared;

namespace CS513.MessageHandling.Messages
{
    [Message(MessageCommand.WhisperMessage)]
    public class WhisperRequest : Message, IRequest
    {
        public WhisperRequest()
        {
            this.Command = MessageCommand.WhisperMessage;
        }

        public WhisperRequest(string sender, string receiver, string contents)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Contents = contents;
            this.Command = MessageCommand.WhisperMessage;
        }

        public void ProcessMessage(IConnectionHandler connection, ConcurrentDictionary<string, IConnectionHandler> connectionHandlers, IMessageHandler handler)
        {
            IConnectionHandler receiverHandler;
            if (connectionHandlers.TryGetValue(this.Receiver, out receiverHandler))
            {
                IMessage response = handler.GetMessage(this.Sender, this.Receiver, this.Contents,
                    MessageCommand.WhisperAck);

                Task.Run(() =>
                {
                    connection.SendMessage(response);
                    receiverHandler.SendMessage(response);
                });
            }
            else
            {
                IMessage response = handler.GetMessage(this.Sender, this.Receiver, string.Format("Could not find user {0}\n", this.Receiver),
                    MessageCommand.WhisperNack);

                Task.Run(() =>
                {
                    connection.SendMessage(response);
                });
            }
        }
    }
}
