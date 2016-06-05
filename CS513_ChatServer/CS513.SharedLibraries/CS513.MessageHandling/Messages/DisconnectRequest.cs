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
    [Message(MessageCommand.DisconnectRequest)]
    public class DisconnectRequest : Message, IRequest
    {
        public DisconnectRequest()
        {
            this.Command = MessageCommand.DisconnectRequest;
        }

        public DisconnectRequest(string sender, string receiver, string contents)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Contents = contents;
            this.Command = MessageCommand.DisconnectRequest;
        }

        public void ProcessMessage(IConnectionHandler connection, ConcurrentDictionary<string, IConnectionHandler> connectionHandlers, IMessageHandler handler)
        {
            IConnectionHandler placeHolder;
            connectionHandlers.TryRemove(this.Sender, out placeHolder);
            connection.Dispose();

            IResponse response = handler.GetResponse(this.Sender, "all", this.Contents,
                MessageCommand.DisconnectResponse);

            foreach (IConnectionHandler connectionHandler in connectionHandlers.Values)
            {
                connectionHandler.SendMessage(response);
            }
        }
    }
}
