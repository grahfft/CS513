using System;
using System.Net;
using System.Net.Sockets;
using CS513.Interfaces.Server;

namespace CS513.SocketListener.Listeners
{
    /// <summary>
    /// This class wraps around a TcpListener
    /// Not unit testable will need to manually run test cases
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
            try
            {
                Socket newSocket = this.listener.EndAcceptSocket(ar);

                EventHandler<Socket> handler = this.NewConnectionReceived;
                if (handler != null && newSocket != null)
                {
                    handler(this, newSocket);
                }               

                this.listener.BeginAcceptSocket(this.SocketAccepted, this.listener);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
