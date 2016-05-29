using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
