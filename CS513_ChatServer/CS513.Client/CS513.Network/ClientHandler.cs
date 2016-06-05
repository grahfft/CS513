using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Client;
using CS513.Interfaces.Server;
using CS513.Interfaces.Shared;

namespace CS513.Network
{
    public class ClientHandler : IClientHandler
    {
        private IClient client;

        private IMessageHandler messageHandler;

        private bool disposed = false;

        private bool restart = false;

        public ClientHandler(IClient client, IMessageHandler messageHandler)
        {
            this.client = client;
            this.messageHandler = messageHandler;
        }

        ~ClientHandler()
        {
            this.Dispose(false);
        }

        public event EventHandler<IResponse> MessageReceived;

        public string Name { get; set; }

        public void SendMessage(IMessage message)
        {
            try
            {
                this.client.SendMessage(message.Serialize());
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public void Connect()
        {
            try
            {
                this.restart = true;
                this.client.Connect();
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
                if (!this.client.IsConnected && currentRestart)
                {
                    //this.logger.LogInfo(string.Format("Client disconnected attempting to reconnect"));
                    this.Connect();
                    return;
                }

                byte[] data = this.client.GetData();
                IResponse message = this.messageHandler.GetResponse(data);

                EventHandler<IResponse> handler = this.MessageReceived;
                if (message != null && handler != null)
                {
                    handler(this, message);
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
              
                this.client.Dispose();
            }
        }
    }
}
