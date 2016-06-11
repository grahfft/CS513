using System;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Server;
using CS513.Interfaces.Shared;

namespace CS513.ServerSocketManager
{
    /// <summary>
    /// Class that handles information coming in from a specific connection
    /// Named after the client on the other end
    /// </summary>
    public class ConnectionHandler : IConnectionHandler
    {
        private IConnection connection;

        private IMessageHandler messageHandler;

        private bool disposed = false;

        private bool restart = false;

        public ConnectionHandler(IConnection connection, IMessageHandler messageHandler, string name)
        {
            this.connection = connection;
            this.messageHandler = messageHandler;
            this.Name = name;
        }

        ~ConnectionHandler()
        {
            this.Dispose(false);
        }

        public event EventHandler<IRequest> MessageReceived;

        public event EventHandler Disposing;

        public string Name { get; set; }

        /// <summary>
        /// Sends a message to the underlying connection
        /// </summary>
        /// <param name="message">message to be sent</param>
        public void SendMessage(IMessage message)
        {
            this.connection.SendMessage(message.Serialize());
        }

        /// <summary>
        /// Begins the dedicated thread for monitoring incoming messages
        /// </summary>
        public void Connect()
        {
            try
            {
                this.restart = true;
                this.MonitorConnection();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        /// Monitors incoming messages
        /// </summary>
        private void MonitorConnection()
        {
            Task.Run(() =>
            {
                bool currentRestart = this.restart;
                try
                {
                    if (!this.connection.IsConnected && currentRestart)
                    {
                        //this.logger.LogInfo(string.Format("Client disconnected attempting to reconnect"));
                        this.MonitorConnection();
                        return;
                    }

                    byte[] data = this.connection.GetData();
                    IRequest message = this.messageHandler.GetRequest(data);

                    EventHandler<IRequest> handler = this.MessageReceived;
                    if (message != null && handler != null)
                    {
                        handler(this, message);
                    }
                }
                catch (Exception exception)
                {
                    
                }

                if (currentRestart)
                {
                    this.MonitorConnection();
                }
            });
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
                this.restart = false;

                EventHandler handler = this.Disposing;
                if (handler != null)
                {
                    handler.BeginInvoke(this, new EventArgs(), null, null);
                }

                this.connection.Dispose();
            }
        }
    }
}
