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
    /// this class wraps around a tcp client
    /// </summary>
    public class WrapperConnection : IConnection
    {
        private Socket socket;

        private TcpClient client;

        public WrapperConnection(Socket socket)
        {
            this.socket = socket;
            this.client = new TcpClient();
            this.client.Client = this.socket;
        }

        public void SendMessage(byte[] data)
        {
            this.socket.Send(data);
        }


    }
}
