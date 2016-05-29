using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
