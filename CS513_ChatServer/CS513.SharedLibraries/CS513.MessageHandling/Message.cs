using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Server;
using CS513.Interfaces.Shared;

namespace CS513.MessageHandling
{
    public abstract class Message
    {
        public string Sender { get; protected set; }

        public string Receiver { get; protected set; }

        public string Contents { get; protected set; }

        public MessageCommand Command { get; protected set; }

        public byte[] Serialize()
        {
            List<byte> byteInfo = new List<byte>();

            int converted = IPAddress.NetworkToHostOrder((int)this.Command);
            byteInfo.AddRange(BitConverter.GetBytes(converted));

            byteInfo.AddRange(BitConverter.GetBytes(IPAddress.NetworkToHostOrder(this.Sender.ToCharArray().Length)));
            byteInfo.AddRange(System.Text.Encoding.ASCII.GetBytes(this.Sender.ToCharArray()));

            byteInfo.AddRange(BitConverter.GetBytes(IPAddress.NetworkToHostOrder(this.Receiver.ToCharArray().Length)));
            byteInfo.AddRange(System.Text.Encoding.ASCII.GetBytes(this.Receiver.ToCharArray()));

            byteInfo.AddRange(Encoding.ASCII.GetBytes(this.Contents.ToCharArray()));

            return byteInfo.ToArray();
        }

        public void Deserialize(byte[] data)
        {
            int dataIndex = sizeof(int);

            int sendSize = IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(data, dataIndex));
            dataIndex = dataIndex + sizeof (int);

            byte[] sender = new byte[sendSize];

            Array.Copy(data, dataIndex, sender, 0, sendSize);
            dataIndex = dataIndex + sendSize;

            int recSize = IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(data, dataIndex));
            dataIndex = dataIndex + sizeof(int);

            byte[] receiver = new byte[recSize];

            Array.Copy(data, dataIndex, receiver, 0, recSize);
            dataIndex = dataIndex + recSize;

            int messageLength = data.Length - dataIndex;
            byte[] message = new byte[messageLength];

            Array.Copy(data, dataIndex, message, 0, messageLength);

            this.Sender = Encoding.Default.GetString(sender);
            this.Receiver = Encoding.Default.GetString(receiver);
            this.Contents = Encoding.Default.GetString(message);
        }
    }
}
