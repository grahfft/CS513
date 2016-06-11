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
    /// <summary>
    /// Holds all information for processing requests
    /// Maintains the list of Connections and Listens to each
    /// </summary>
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

        /// <summary>
        /// Sets up and starts the listener
        /// </summary>
        public void Configure()
        {
            this.listener.NewConnectionReceived += this.CreateNewConnection;
            this.listener.Start();
        }

        /// <summary>
        /// Creates new connection handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="socket"></param>
        private void CreateNewConnection(object sender, Socket socket)
        {
            IConnectionHandler connectionHandler = this.factory.GetNewConnection(socket);
            connectionHandler.MessageReceived += this.HandleIncomingMessage;
            connectionHandler.Disposing += this.HandleConnectionDispose;
            connectionHandler.Connect();

            this.connectionHandlers.TryAdd(connectionHandler.Name, connectionHandler);
        }

        /// <summary>
        /// Handles connection clean up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Handles incoming messages from connections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void HandleIncomingMessage(object sender, IRequest message)
        {
            IConnectionHandler handler = sender as IConnectionHandler;
            Task.Run(() => message.ProcessMessage(handler, this.connectionHandlers, this.messageHandler));
        }

        /// <summary>
        /// Dispose pattern for clean up
        /// </summary>
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
