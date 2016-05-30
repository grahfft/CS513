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
    [Message(MessageCommand.UsersRequest)]
    public class UsersRequest : Message, IRequest
    {
        public UsersRequest()
        {
            this.Command = MessageCommand.UsersRequest;
        }

        public UsersRequest(string sender, string receiver, string contents)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Contents = contents;
            this.Command = MessageCommand.UsersRequest;
        }

        public void ProcessMessage(IConnectionHandler connection, ConcurrentDictionary<string, IConnectionHandler> connectionHandlers, IMessageHandler handler)
        {
            string allUsers = connectionHandlers.Keys.Aggregate("", (current, userName) => current + " " + userName);

            IMessage response = handler.GetMessage("Server", this.Sender, allUsers, MessageCommand.UsersAck);
            connection.SendMessage(response);
        }
    }
}
