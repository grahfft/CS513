using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Server;
using CS513.Interfaces.Shared;

namespace CS513.ServerSocketManager
{
    public class ConnectionManager : IConnectionManager
    {
        private IListener listener;

        private ConnectionFactory factory;

        private ConcurrentDictionary<string, IConnectionHandler> connectionHandlers; 

        private IMessageHandler messageHandler;

        private int connectionId = 1;

        private bool disposed = false;

        public ConnectionManager(IListener listener, IMessageHandler messageHandler)
        {
            this.listener = listener;
            this.factory = new ConnectionFactory();
            this.messageHandler = messageHandler;
            this.connectionHandlers = new ConcurrentDictionary<string, IConnectionHandler>();
        }

        ~ConnectionManager()
        {
            this.Dispose(false);
        }

        public void Configure()
        {
            this.listener.NewConnectionReceived += this.CreateNewConnection;
        }

        private void CreateNewConnection(object sender, Socket socket)
        {
            IConnection connection = this.factory.GetNewConnection(socket);
            IConnectionHandler connectionHandler = new ConnectionHandler(connection, this.messageHandler, this.connectionId++.ToString());
            connectionHandler.MessageReceived += this.HandleIncomingMessage;
            connectionHandler.Disposing += this.HandleConnectionDispose;

            this.connectionHandlers.TryAdd(connectionHandler.Name, connectionHandler);
        }

        private void HandleConnectionDispose(object sender, EventArgs e)
        {
            IConnectionHandler connectionHandler = sender as IConnectionHandler;
            if (connectionHandler != null)
            {
                connectionHandler.MessageReceived -= this.HandleIncomingMessage;
                connectionHandler.Disposing -= this.HandleConnectionDispose;
            }
        }

        private void HandleIncomingMessage(object sender, IMessage message)
        {
            IConnectionHandler handler = sender as IConnectionHandler;
            Task.Run(() => message.ProcessMessage(handler, this.connectionHandlers, this.messageHandler));
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

                foreach (IConnectionHandler connectionHandler in connectionHandlers.Values)
                {
                    connectionHandler.Dispose();
                }

                this.connectionHandlers = null;
            }
        }
    }
}
