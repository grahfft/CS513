using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Server;

namespace CS513.SocketListener.Listeners
{
    /// <summary>
    /// This class wraps around a TcpListener
    /// </summary>
    [Listener(Name)]
    public class WrapperListener : IListener
    {
        public const string Name = "Tcp";

        private TcpListener listener;

        public WrapperListener(int port)
        {
            this.listener = new TcpListener(IPAddress.Any, port);
        }

        public event EventHandler<Socket> NewConnectionReceived;

        public void Start()
        {
            this.listener.Start();
            this.listener.BeginAcceptSocket(this.SocketAccepted, null);
        }

        public void Stop()
        {
            this.listener.Stop();
        }

        private void SocketAccepted(IAsyncResult ar)
        {
            Socket newSocket = this.listener.EndAcceptSocket(ar);

            EventHandler<Socket> handler = this.NewConnectionReceived;
            if (handler != null)
            {
                handler(this, newSocket);
            }

            this.listener.BeginAcceptSocket(this.SocketAccepted, null);
        }
    }
}
