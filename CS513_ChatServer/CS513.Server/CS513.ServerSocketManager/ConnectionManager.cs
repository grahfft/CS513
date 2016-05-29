using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Server;
using CS513.Interfaces.Shared;

namespace CS513.ServerSocketManager
{
    public class ConnectionManager : IConnectionManager
    {
        private IListener listener;

        private IConnectionFactory factory;

        private ConcurrentDictionary<string, IConnectionHandler> connectionHandlers; 

        private IMessageHandler messageHandler;

        private bool disposed = false;

        public ConnectionManager(IListener listener, IConnectionFactory factory, IMessageHandler messageHandler)
        {
            this.listener = listener;
            this.messageHandler = messageHandler;
            this.factory = factory;
            this.connectionHandlers = new ConcurrentDictionary<string, IConnectionHandler>();
        }

        ~ConnectionManager()
        {
            this.Dispose(false);
        }

        public IReadOnlyDictionary<string, IConnectionHandler> ConnectionHandlers
        {
            get
            {
                return new ReadOnlyDictionary<string, IConnectionHandler>(this.connectionHandlers);
            }
        }

        public void Configure()
        {
            this.listener.NewConnectionReceived += this.CreateNewConnection;
            this.listener.Start();
        }

        private void CreateNewConnection(object sender, Socket socket)
        {
            IConnectionHandler connectionHandler = this.factory.GetNewConnection(socket);
            connectionHandler.MessageReceived += this.HandleIncomingMessage;
            connectionHandler.Disposing += this.HandleConnectionDispose;

            this.connectionHandlers.TryAdd(connectionHandler.Name, connectionHandler);
        }

        private void HandleConnectionDispose(object sender, EventArgs e)
        {
            IConnectionHandler connectionHandler = sender as IConnectionHandler;
            if (connectionHandler != null)
            {
                IConnectionHandler handler = null;
                this.connectionHandlers.TryRemove(connectionHandler.Name, out handler);
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
                this.listener.Stop();
                foreach (IConnectionHandler connectionHandler in connectionHandlers.Values)
                {
                    connectionHandler.Dispose();
                }

                this.connectionHandlers = null;
            }
        }
    }
}
