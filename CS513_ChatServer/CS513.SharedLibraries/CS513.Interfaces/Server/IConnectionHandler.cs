using System;
using CS513.Interfaces.Shared;

namespace CS513.Interfaces.Server
{
    public interface IConnectionHandler : IDisposable
    {
        event EventHandler<IRequest> MessageReceived;

        event EventHandler Disposing;

        string Name { get; set; }

        void SendMessage(IMessage message);

        void Connect();
    }
}
