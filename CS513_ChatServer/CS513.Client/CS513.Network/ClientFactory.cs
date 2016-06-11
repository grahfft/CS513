using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Client;
using CS513.Interfaces.Shared;

namespace CS513.Network
{
    public class ClientFactory
    {
        private IClient currentClient;

        private IMessageHandler messageHandler;

        /// <summary>
        /// Construction dependent on the MessageHandler
        /// </summary>
        /// <param name="messageHandler">Single Message instance</param>
        public ClientFactory(IMessageHandler messageHandler)
        {
            this.messageHandler = messageHandler;
        }

        /// <summary>
        /// Checks to see if currentClient is already construected
        /// if not load configurations and create configured client
        /// </summary>
        /// <returns>Proxy handler for the configured client</returns>
        public IClientHandler GetClient()
        {
            if (this.currentClient == null)
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                string type = config.AppSettings.Settings["ClientType"].Value;
                string address = config.AppSettings.Settings["Address"].Value;
                int port = int.Parse(config.AppSettings.Settings["Port"].Value);

                Type clientType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => Attribute.IsDefined(t, typeof(ClientAttribute))
                                && !t.IsAbstract
                                && !t.IsInterface).ToList().FirstOrDefault(
                                    t =>
                                        t.GetCustomAttributes(typeof(ClientAttribute), false)
                                            .Any(
                                                a =>
                                                    ((ClientAttribute)a).Name.ToUpper() ==
                                                    type.ToUpper()));
                if (clientType != null)
                {
                    this.currentClient = (IClient) Activator.CreateInstance(clientType, address, port);
                }
            }

            return new ClientHandler(this.currentClient, this.messageHandler);
        }
    }
}
