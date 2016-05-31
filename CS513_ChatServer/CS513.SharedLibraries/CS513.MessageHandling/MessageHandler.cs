using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using CS513.Interfaces;
using CS513.Interfaces.Shared;

namespace CS513.MessageHandling
{
    /// <summary>
    /// Message factory; Creates messages from different data
    /// </summary>
    public class MessageHandler : IMessageHandler
    {
        private Dictionary<MessageCommand, Type> messages;
 
        public MessageHandler()
        {
            this.messages = new Dictionary<MessageCommand, Type>();
            this.LoadMessages();
        }

        public IMessage GetMessage(byte[] data)
        {
            IMessage message = null;
            if (data.Length > sizeof (int))
            {
                MessageCommand command =
                    (MessageCommand) IPAddress.NetworkToHostOrder((int) BitConverter.ToUInt32(data, 0));

                Type messageType;


                if (this.messages.TryGetValue(command, out messageType))
                {
                    message = (IMessage) Activator.CreateInstance(messageType);
                    message.Deserialize(data);
                }
            }
            return message;
        }

        public IMessage GetMessage(string sender, string receiver, string contents, MessageCommand command)
        {
            Type messageType;

            IMessage message = null;
            if (this.messages.TryGetValue(command, out messageType))
            {
                message = (IMessage) Activator.CreateInstance(messageType, sender, receiver, contents);
            }

            return message;
        }

        public IRequest GetRequest(byte[] data)
        {
            IMessage message = this.GetMessage(data);
            return (IRequest) message;
        }

        public IRequest GetRequest(string sender, string receiver, string contents, MessageCommand command)
        {
            IMessage message = this.GetMessage(sender, receiver, contents, command);
            return (IRequest) message;
        }

        public IResponse GetResponse(byte[] data)
        {
            IMessage message = this.GetMessage(data);
            return (IResponse)message;
        }

        public IResponse GetResponse(string sender, string receiver, string contents, MessageCommand command)
        {
            IMessage message = this.GetMessage(sender, receiver, contents, command);
            return (IResponse)message;
        }

        private void LoadMessages()
        {
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => Attribute.IsDefined(t, typeof (MessageAttribute))
                            && !t.IsAbstract
                            && !t.IsInterface).ToList().ForEach((type) =>
                            {
                                MessageAttribute attribute = (MessageAttribute)type.GetCustomAttribute(typeof(MessageAttribute));
                                this.messages.Add(attribute.Command, type);
                            });
        }
    }
}
