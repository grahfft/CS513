using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Server;

namespace CS513.Interfaces.Shared
{
    /// <summary>
    /// Messages from Client to Server
    /// </summary>
    public interface IRequest : IMessage
    {
        void ProcessMessage(IConnectionHandler connection, ConcurrentDictionary<string, IConnectionHandler> connectionHandlers, IMessageHandler handler);
    }
}
