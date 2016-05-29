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
    [Message(MessageCommand.ChatMessage)]
    public class ChatRequest : Message, IRequest
    {
        public ChatRequest()
        {
            this.Command = MessageCommand.ChatMessage;
        }

        public ChatRequest(string sender, string receiver, string contents)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Contents = contents;
            this.Command = MessageCommand.ChatMessage;
        }

        public void ProcessMessage(IConnectionHandler connection, ConcurrentDictionary<string, IConnectionHandler> connectionHandlers, IMessageHandler handler)
        {
            IMessage response = handler.GetMessage(this.Sender, this.Receiver, this.Contents, MessageCommand.ChatAck);
            Task.Run(() =>
            {
                foreach (IConnectionHandler connectionHandler in connectionHandlers.Values)
                {
                    connectionHandler.SendMessage(response);
                }
            });
        }
    }
}
