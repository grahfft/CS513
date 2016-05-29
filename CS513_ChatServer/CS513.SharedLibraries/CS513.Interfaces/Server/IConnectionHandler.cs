using System;

namespace CS513.Interfaces.Server
{
    public interface IConnectionHandler : IDisposable
    {
        event EventHandler<IMessage> MessageReceived;

        event EventHandler Disposing;

        string Name { get; set; }

        void SendMessage(IMessage message);

        void Connect();
    }
}
