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
    [Message(MessageCommand.UpdateName)]
    public class UpdateNameRequest : Message, IRequest
    {
        public UpdateNameRequest()
        {
            this.Command = MessageCommand.UpdateName;
        }

        public UpdateNameRequest(string sender, string receiver, string contents)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Contents = contents;
            this.Command = MessageCommand.UpdateName;
        }

        public void ProcessMessage(IConnectionHandler connection, ConcurrentDictionary<string, IConnectionHandler> connectionHandlers, IMessageHandler handler)
        {
            if (!connectionHandlers.ContainsKey(this.Contents))
            {
                IConnectionHandler connectionHandler;
                connectionHandlers.TryRemove(connection.Name, out connectionHandler);
                connectionHandlers.TryAdd(this.Contents, connectionHandler);

                connectionHandler.Name = this.Contents;

                IMessage response = handler.GetMessage(this.Sender, this.Receiver,
                    this.Contents, MessageCommand.UpdateAck);

                foreach (IConnectionHandler otherConnectionHandler in connectionHandlers.Values)
                {
                    otherConnectionHandler.SendMessage(response);
                }

            }
            else
            {
                IMessage response = handler.GetMessage(this.Sender, this.Receiver,
                    string.Format("Name {0} already in use", this.Contents), MessageCommand.UpdateNack);
                connection.SendMessage(response);
            }
        }
    }
}
