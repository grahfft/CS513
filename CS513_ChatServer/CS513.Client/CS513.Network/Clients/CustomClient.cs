using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Client;

namespace CS513.Network.Clients
{
    [Client(Name)]
    public class CustomClient : IClient
    {
        private const string Name = "Custom";

        private Socket socket;

        private string address;

        private int port;
        private bool disposed = false;

        /// <summary>
        /// Constructor for the CustomClient
        /// Creates a stream based socket with an IP protocol
        /// </summary>
        /// <param name="address">address of the server</param>
        /// <param name="port">port of the address</param>
        public CustomClient(string address, int port)
        {
            this.address = address;
            this.port = port;
            this.socket = new Socket(SocketType.Stream, ProtocolType.IP);
        }

        ~CustomClient()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Checks if the socket is connected
        /// </summary>
        public bool IsConnected
        {
            get { return this.socket.Connected; }
        }

        /// <summary>
        /// Send message byte data to the server
        /// </summary>
        /// <param name="data">data to be sent</param>
        public void SendMessage(byte[] data)
        {
            this.socket.Send(data);
        }

        /// <summary>
        /// Checks available data of the socket.
        /// Gets all available data and returns it
        /// </summary>
        /// <returns>All data available from the socket</returns>
        public byte[] GetData()
        {
            int dataAvailable = this.socket.Available;
            byte[] bytes = new byte[dataAvailable];

            if (dataAvailable != 0)
            {
                this.socket.Receive(bytes, dataAvailable, SocketFlags.None);
            }
            
            return bytes;
        }

        /// <summary>
        /// Connects to the server
        /// </summary>
        public void Connect()
        {
            try
            {
                this.socket.Connect(this.address, this.port);
            }
            catch (Exception exception)
            {
                
            }
        }

        /// <summary>
        /// Dispose pattern to clean up connection
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing && !this.disposed)
            {
                this.disposed = true;

                this.socket.Dispose();
            }
        }
    }
}
