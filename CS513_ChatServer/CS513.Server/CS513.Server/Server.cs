using CS513.Interfaces.Server;
using CS513.Interfaces.Shared;
using CS513.MessageHandling;
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

        private IConnectionFactory connectionFactory;

        public Server()
        {
            this.listenerFactory = new ListenerFactory();
            this.Listener = this.listenerFactory.CreataListener();
            this.messageHandler = new MessageHandler();
            this.connectionFactory = new ConnectionFactory(this.messageHandler);
            this.ConnectionManager = new ConnectionManager(this.Listener, this.connectionFactory, this.messageHandler);
        }

        public IListener Listener { get; private set; }

        public IConnectionManager ConnectionManager { get; private set; }
    }
}
