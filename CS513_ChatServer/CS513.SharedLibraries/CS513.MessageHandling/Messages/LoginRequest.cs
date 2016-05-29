using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Server;
using CS513.Interfaces.Shared;

namespace CS513.MessageHandling.Messages
{
    [Message(MessageCommand.LoginRequest)]
    public class LoginRequest : IMessage
    {
        private MessageHeader header;

        public LoginRequest(MessageHeader header, string message)
        {
            this.header = header;
        }

        public string Message { get; private set; }

        public void ProcessMessage(IConnectionHandler connection, ConcurrentDictionary<string, IConnectionHandler> connectionHandlers, IMessageHandler messageHandler)
        {
            IConnectionHandler handler;
            string responseMessage = string.Empty;
            MessageHeader responseHeader = null;

            if (!connectionHandlers.ContainsKey(this.Message))
            {
                connectionHandlers.TryAdd(Message, connection);
                connection.Name = Message;

                responseHeader = new MessageHeader("server", Message, MessageCommand.LoginAck);
                responseMessage = this.Message + "has logged in";
                IMessage response = messageHandler.GetMessage(responseHeader, responseMessage);

                Task.Run(() =>
                {
                    foreach (IConnectionHandler connectionHandler in connectionHandlers.Values)
                    {
                        connectionHandler.SendMessage(response);
                    }
                });
            }
            else
            {
                connectionHandlers.TryRemove(connection.Name, out handler);
                

                responseHeader = new MessageHeader("server", Message, MessageCommand.LoginNack);
                responseMessage = "Login already in use";
                IMessage response = messageHandler.GetMessage(responseHeader, responseMessage);

                Task.Run(() =>
                {
                    connection.SendMessage(response);
                    connection.Dispose();
                });
            }
        }

        public void Deserialize(byte[] data)
        {
            int dataIndex = this.header.DataSize();

            int messageLength = data.Length - dataIndex;
            byte[] message = new byte[messageLength + 1];

            Array.Copy(data, dataIndex, message, 0, messageLength);
        }

        public byte[] Serialize()
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(this.header.Serialize());



            return bytes.ToArray();
        }
    }
}
