using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Client;

namespace CS513.Network.Clients
{
    /// <summary>
    /// This class wraps around TcpClient
    /// </summary>
    [Client(Name)]
    public class WrapperClient : IClient
    {
        private const string Name = "Tcp";

        private TcpClient client;

        private string address;

        private int port;
        private bool disposed = false;

        /// <summary>
        /// Constructed using addess and port of server
        /// </summary>
        /// <param name="address">address of server</param>
        /// <param name="port">port of server</param>
        public WrapperClient(string address, int port)
        {
            this.address = address;
            this.port = port;
            this.client = new TcpClient();
        }

        /// <summary>
        /// Check if client is still connected
        /// </summary>
        public bool IsConnected
        {
            get { return this.client.Connected; }
        }

        /// <summary>
        /// Send converted message to the server
        /// </summary>
        /// <param name="messageData">byte array of a message</param>
        public void SendMessage(byte[] messageData)
        {
            this.client.Client.Send(messageData);
        }

        /// <summary>
        /// Gets the data from the underlying stream.
        /// </summary>
        /// <returns>data found in the socket buffer</returns>
        public byte[] GetData()
        {
            byte[] bytes = new byte[this.client.Client.Available];//TODO figure out a better way this is nasty; currently set to 1 MB of data

            if (this.client == null || !this.client.Connected)
            {
                return bytes;
            }

            NetworkStream stream = this.client.GetStream();

            if (stream.CanRead)
            {
                int readSoFar = 0;
                while (stream.DataAvailable && readSoFar <= bytes.Length)
                {
                    int read = bytes.Length - readSoFar > 1028 ? 1028 : bytes.Length - readSoFar;
                    int interval = stream.Read(bytes, readSoFar, read);
                    readSoFar = readSoFar + interval;
                }
            }

            return bytes;
        }

        /// <summary>
        /// Establishes the connection from client to server
        /// </summary>
        public void Connect()
        {
            try
            {
                this.client.Connect(address, port);
            }
            catch (Exception exception)
            {
                
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                this.disposed = true;

                this.client = null;
            }
        }
    }
}
