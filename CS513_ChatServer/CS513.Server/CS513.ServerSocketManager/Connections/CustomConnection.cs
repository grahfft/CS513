using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Server;

namespace CS513.ServerSocketManager.Connections
{
    /// <summary>
    /// this class acts like TcpClient
    /// </summary>
    public class CustomConnection : IConnection
    {
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

        public void SendMessage(byte[] data)
        {
            this.socket.Send(data);
        }

        public void Connect()
        {
            
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
