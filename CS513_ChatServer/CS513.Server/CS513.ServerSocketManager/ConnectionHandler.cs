using System;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Server;
using CS513.Interfaces.Shared;

namespace CS513.ServerSocketManager
{
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

        public event EventHandler<IMessage> MessageReceived;

        public event EventHandler Disposing;

        public string Name { get; set; }

        public void SendMessage(IMessage message)
        {
            this.connection.SendMessage(message.Serialize());
        }

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

        private void MonitorConnection()
        {
            Task.Run(() =>
            {
                bool currentRestart = this.restart;
                if (!this.connection.IsConnected && currentRestart)
                {
                    //this.logger.LogInfo(string.Format("Client disconnected attempting to reconnect"));
                    this.Connect();
                    return;
                }

                byte[] data = this.connection.GetData();
                IMessage message = this.messageHandler.GetMessage(data);

                EventHandler<IMessage> handler = this.MessageReceived;
                if (message != null && handler != null)
                {
                    Task.Run(() => handler(this, message));
                }

                if (currentRestart)
                {
                    this.MonitorConnection();
                }
            });
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
