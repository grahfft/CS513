using System;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using CS513.Interfaces.Server;
using CS513.Interfaces.Shared;
using CS513.ServerSocketManager.Connections;

namespace CS513.ServerSocketManager
{
    /// <summary>
    /// Locates which service to use and will create all future services off this.
    /// </summary>
    public class ConnectionFactory : IConnectionFactory
    {
        private Type connectionType;

        private int connectionId = 1;

        private IMessageHandler messageHandler;

        public ConnectionFactory(IMessageHandler messageHandler)
        {
            this.messageHandler = messageHandler;
            this.LoadConfigs();
        }

        /// <summary>
        /// Creates a new connection handler
        /// </summary>
        /// <param name="socket">socket of connected client</param>
        /// <returns>connection handler for connected client</returns>
        public IConnectionHandler GetNewConnection(Socket socket)
        {
            IConnection connection = null;
            
            if (this.connectionType == null)
            {
                connection = new WrapperConnection(socket);
            }
            else
            {
                connection = (IConnection) Activator.CreateInstance(this.connectionType, socket);
            }

            return new ConnectionHandler(connection, this.messageHandler, this.connectionId++.ToString());
        }

        /// <summary>
        /// Use reflection to load the right type of connection to create
        /// </summary>
        private void LoadConfigs()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string type = config.AppSettings.Settings["ConnectionType"].Value;

            this.connectionType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => Attribute.IsDefined(t, typeof(ConnectionAttribute))
                            && !t.IsAbstract
                            && !t.IsInterface).ToList().FirstOrDefault(
                                t =>
                                    t.GetCustomAttributes(typeof(ConnectionAttribute), false)
                                        .Any(
                                            a =>
                                                ((ConnectionAttribute)a).Name.ToUpper() ==
                                                type.ToUpper()));

        }
    }
}
