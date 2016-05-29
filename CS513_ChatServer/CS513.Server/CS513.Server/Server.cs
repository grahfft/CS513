using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Server;
using CS513.Interfaces.Shared;
using CS513.ServerSocketManager;
using CS513.SocketListener;

namespace CS513.Server
{
    /// <summary>
    /// This class is responsible for creating all necessary classes for a server
    /// </summary>
    public class Server
    {
        private ListenerFactory listenerFactory;

        private IMessageHandler messageHandler;

        public Server()
        {
            this.listenerFactory = new ListenerFactory();
            this.Listener = this.listenerFactory.CreataListener();
            this.messageHandler = null;
            this.ConnectionManager = new ConnectionManager(this.Listener, this.messageHandler);
        }

        public IListener Listener { get; private set; }

        public IConnectionManager ConnectionManager { get; private set; }
    }
}
