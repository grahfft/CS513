using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Server;

namespace CS513.Interfaces
{
    /// <summary>
    /// Interface for message sent over the wire
    /// </summary>
    public interface IMessage
    {
        void ProcessMessage(IConnectionHandler connection, ConcurrentDictionary<string, IConnectionHandler> connectionHandlers);

        byte[] Serialize();
    }
}
