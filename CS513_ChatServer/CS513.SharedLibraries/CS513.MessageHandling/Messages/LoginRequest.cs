using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Server;
using CS513.Interfaces.Shared;

namespace CS513.MessageHandling.Messages
{
    [Message(MessageCommand.LoginRequest)]
    public class LoginRequest : Message, IRequest
    {
        public LoginRequest()
        {
            this.Command = MessageCommand.LoginRequest;
        }

        public LoginRequest(string sender, string receiver, string contents)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Contents = contents;
            this.Command = MessageCommand.LoginRequest;
        }

        public void ProcessMessage(IConnectionHandler connection, ConcurrentDictionary<string, IConnectionHandler> connectionHandlers, IMessageHandler messageHandler)
        {
            IConnectionHandler handler;
            string responseMessage = string.Empty;
            
            if (!connectionHandlers.ContainsKey(this.Contents))
            {
                //Remove the place holder name with login name and broadcast login ack to everyone
                connectionHandlers.TryRemove(connection.Name, out handler);
                connectionHandlers.TryAdd(this.Contents, connection);
                connection.Name = this.Contents;

                responseMessage = connectionHandlers.Keys.Aggregate(string.Empty, (current, userName) => current + "|" + userName);
                
                IMessage response = messageHandler.GetMessage(this.Contents, "all", responseMessage, MessageCommand.LoginAck);

                foreach (IConnectionHandler connectionHandler in connectionHandlers.Values)
                {
                    connectionHandler.SendMessage(response);
                }
            }
            else
            {
                //Login in use don't; kill connection and nack the request to the otherside
                connectionHandlers.TryRemove(connection.Name, out handler);

                responseMessage = "Login already in use";
                IMessage response = messageHandler.GetMessage("server", connection.Name, responseMessage, MessageCommand.LoginNack);

                connection.SendMessage(response);
                connection.Dispose();
            }
        }
    }
}
