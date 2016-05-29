using System;
using System.Net.Sockets;
using CS513.Interfaces.Shared;

namespace CS513.Interfaces.Server
{
    /// <summary>
    /// Interface for socket listeners
    /// </summary>
    public interface IListener
    {
        event EventHandler<Socket> NewConnectionReceived;

        void Start();

        void Stop();
    }
}
