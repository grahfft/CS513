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

        public CustomConnection(Socket socket)
        {
            this.socket = socket;
        }

        public void SendMessage(byte[] data)
        {
            this.socket.Send(data);
        }

        public void Connect()
        {
            
        }
    }
}
