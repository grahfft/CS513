using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Server;

namespace CS513.SocketListener.Listeners
{
    /// <summary>
    /// Wraps around a socket and acts like TcpListener
    /// </summary>
    [Listener(Name)]
    public class CustomListener : IListener
    {
        public const string Name = "Custom";

        public CustomListener()
        {
            
        }

        public event EventHandler<Socket> NewConnectionReceived;

        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}
