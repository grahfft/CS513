using System;
using System.Net;
using System.Net.Sockets;
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

        private bool restart = false;

        private Socket listenerSocket;

        private int port;

        private string address;

        public CustomListener(int port)
        {
            this.port = port;
        }

        public event EventHandler<Socket> NewConnectionReceived;

        /// <summary>
        /// Begin Listening for connections coming in
        /// </summary>
        public void Start()
        {
            try
            {
                System.Net.IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, this.port);

                this.listenerSocket = new Socket(serverEndPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.IP);

                this.listenerSocket.Bind(serverEndPoint);
                this.listenerSocket.Listen(1028); // TODO don't hardcode this
                this.listenerSocket.BeginAccept(this.HandleAccept, this.listenerSocket);
            }
            catch (Exception exception)
            {
                
            }
        }

        /// <summary>
        /// Stop listening for connections
        /// </summary>
        public void Stop()
        {
            this.listenerSocket.Dispose();
        }

        /// <summary>
        /// Handle an accept coming in
        /// Notify a new socket connection
        /// Begin listening again
        /// </summary>
        /// <param name="ar">async result of the accept call</param>
        private void HandleAccept(IAsyncResult ar)
        {
            try
            {               
                Socket stateSocket = (Socket) ar.AsyncState;
                Socket acceptedSocket = stateSocket.EndAccept(ar);

                EventHandler<Socket> handler = this.NewConnectionReceived;
                if (handler != null)
                {
                    handler(this, acceptedSocket);
                }

                this.listenerSocket.BeginAccept(this.HandleAccept, this.listenerSocket);
            }
            catch (Exception exception)
            {
                
            }
        }
    }
}
