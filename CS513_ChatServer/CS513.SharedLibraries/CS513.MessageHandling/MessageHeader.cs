using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;

namespace CS513.MessageHandling
{
    public class MessageHeader
    {
        public MessageHeader()
        {
            
        }

        public string Sender { get; private set; }

        public string Receiver { get; private set; }

        public MessageCommand MessageCommand { get; private set; }

        public string Message { get; private set; }

        public void Deserialize(byte[] data)
        {
            int dataIndex = 0;
            byte[] sender = new byte[50];
            byte[] receiver = new byte[50];
            byte[] message;

            Array.Copy(data, dataIndex, sender, 0, 30);
            dataIndex = dataIndex + 50;

            Array.Copy(data, dataIndex, receiver, 0, 50);
            dataIndex = dataIndex + 50;

            this.MessageCommand = (MessageCommand)IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(data, dataIndex));
            dataIndex = dataIndex + sizeof (int);

            int messageLength = data.Length - dataIndex;
            message = new byte[messageLength + 1];

            Array.Copy(data, dataIndex, message, 0, messageLength);

            this.Sender = System.Text.Encoding.Default.GetString(sender);
            this.Receiver = System.Text.Encoding.Default.GetString(receiver);
            this.Message = System.Text.Encoding.Default.GetString(message);
        }

        public byte[] Serialize()
        {
            return new byte[0];
        }
    }
}
