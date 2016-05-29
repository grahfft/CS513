using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Shared;

namespace CS513.MessageHandling
{
    public class MessageHeader : IMessageHeader
    {
        private const int senderSize = 50;

        private const int receiverSize = 50;

        public MessageHeader()
        {
            
        }

        public MessageHeader(string sender, string receiver, MessageCommand command)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.MessageCommand = command;
        }

        public string Sender { get; private set; }

        public string Receiver { get; private set; }

        public MessageCommand MessageCommand { get; private set; }

        public void Deserialize(byte[] data)
        {
            int dataIndex = 0;
            byte[] sender = new byte[senderSize];
            byte[] receiver = new byte[receiverSize];
            byte[] message;

            Array.Copy(data, dataIndex, sender, 0, senderSize);
            dataIndex = dataIndex + senderSize;

            Array.Copy(data, dataIndex, receiver, 0, receiverSize);
            dataIndex = dataIndex + receiverSize;

            this.MessageCommand = (MessageCommand)IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(data, dataIndex));
            dataIndex = dataIndex + sizeof (int);

            

            this.Sender = System.Text.Encoding.Default.GetString(sender);
            this.Receiver = System.Text.Encoding.Default.GetString(receiver);
        }

        public byte[] Serialize()
        {
            return new byte[0];
        }

        public int DataSize()
        {
            return senderSize + receiverSize + sizeof (Int32);
        }
    }
}
