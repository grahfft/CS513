using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Server;
using CS513.ServerSocketManager.Connections;

namespace CS513.ServerSocketManager
{
    /// <summary>
    /// Locates which service to use and will create all future services off this.
    /// </summary>
    public class ConnectionFactory
    {
        private Type connectionType;

        public ConnectionFactory()
        {
            this.LoadConfigs();
        }

        public IConnection GetNewConnection(Socket socket)
        {
            if (this.connectionType == null)
            {
                return new WrapperConnection(socket);
            }

            return (IConnection) Activator.CreateInstance(this.connectionType, socket);
        }

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
