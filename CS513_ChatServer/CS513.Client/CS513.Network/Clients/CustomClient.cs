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

        public bool IsConnected
        {
            get { return this.socket.Connected; }
        }

        public void SendMessage(byte[] data)
        {
            if (this.socket.Connected)
            {
                this.socket.Send(data);
            }
        }

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
