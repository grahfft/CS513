using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Server;

namespace CS513.ServerSocketManager
{
    public class ConnectionManager
    {
        private IListener listener;

        private ConnectionFactory factory;

        private List<IConnection> connections; 

        public ConnectionManager(IListener listener)
        {
            this.listener = listener;
            this.factory = new ConnectionFactory();
            this.connections = new List<IConnection>();
        }

        public void Configure()
        {
            this.listener.NewConnectionReceived += this.CreateNewConnection;
        }

        private void CreateNewConnection(object sender, Socket socket)
        {
            IConnection connection = this.factory.GetNewConnection(socket);
        }
    }
}
