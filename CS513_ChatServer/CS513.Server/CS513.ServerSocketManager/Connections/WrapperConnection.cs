using System;
using System.Net.Sockets;
using CS513.Interfaces.Server;

namespace CS513.ServerSocketManager.Connections
{
    /// <summary>
    /// this class wraps around a tcp client
    /// </summary>
    [Connection(Name)]
    public class WrapperConnection : IConnection
    {
        private const string Name = "Tcp";

        private Socket socket;

        private TcpClient client;

        private bool disposed = false;

        public WrapperConnection(Socket socket)
        {
            this.socket = socket;
            this.client = new TcpClient();
            this.client.Client = this.socket;
        }

        ~WrapperConnection()
        {
            this.Dispose(false);
        }

        public bool IsConnected
        {
            get { return this.client.Connected; }
        }

        public void SendMessage(byte[] data)
        {
            this.socket.Send(data);
        }

        public byte[] GetData()
        {
            byte[] bytes = new byte[this.socket.Available];//TODO figure out a better way this is nasty; currently set to 1 MB of data

            if (this.client == null || !this.client.Connected)
            {
                return bytes;
            }

            NetworkStream stream = this.client.GetStream();

            if (stream.CanRead)
            {
                int readSoFar = 0;
                while (stream.DataAvailable)
                {
                    int read = bytes.Length - readSoFar > 1028 ? 1028 : bytes.Length - readSoFar;
                    int interval = stream.Read(bytes, readSoFar, read);
                    readSoFar = readSoFar + interval;
                }
            }

            return bytes;
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
                this.socket.Dispose();
            }
        }
    }
}
