using System;
using System.Net.Sockets;
using CS513.Interfaces.Server;

namespace CS513.ServerSocketManager.Connections
{
    /// <summary>
    /// this class acts like TcpClient
    /// </summary>
    [Connection(Name)]
    public class CustomConnection : IConnection
    {
        private const string Name = "Custom";

        private Socket socket;

        private bool disposed = false;

        public CustomConnection(Socket socket)
        {
            this.socket = socket;
        }

        ~CustomConnection()
        {
            this.Dispose(false);
        }

        public bool IsConnected
        {
            get { return this.socket.Connected; }
        }

        public void SendMessage(byte[] data)
        {
            this.socket.Send(data);
        }

        public byte[] GetData()
        {
            byte[] bytes = new byte[this.socket.Available + 1];

            if (!this.socket.Connected)
            {
                return bytes;
            }

            if (this.socket.Available != 0)
            {
                this.socket.Receive(bytes);
            }

            return bytes;
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
