using System;
using System.Configuration;
using System.Linq;
using CS513.Interfaces.Server;

namespace CS513.SocketListener
{
    /// <summary>
    /// Loads in run time configuration to determine what type of listener to create
    /// </summary>
    public class ListenerFactory
    {
        private IListener currentListener;

        public ListenerFactory()
        {
        }

        /// <summary>
        /// Create a listener class to accept connections
        /// </summary>
        /// <returns>IListener specified by configurations</returns>
        public IListener CreataListener()
        {
            if (this.currentListener == null)
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                string type = config.AppSettings.Settings["ListenerType"].Value;
                int port = int.Parse(config.AppSettings.Settings["Port"].Value);

                Type listenerType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => Attribute.IsDefined(t, typeof (ListenerAttribute))
                                && !t.IsAbstract
                                && !t.IsInterface).ToList().FirstOrDefault(
                                    t =>
                                        t.GetCustomAttributes(typeof (ListenerAttribute), false)
                                            .Any(
                                                a =>
                                                    ((ListenerAttribute) a).Name.ToUpper() ==
                                                    type.ToUpper()));

                if (listenerType != null)
                {
                    this.currentListener = (IListener) Activator.CreateInstance(listenerType, port);
                }
            }

            return this.currentListener;
        }
    }
}
