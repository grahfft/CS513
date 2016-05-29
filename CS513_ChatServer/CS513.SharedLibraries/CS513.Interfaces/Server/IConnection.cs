using System;

namespace CS513.Interfaces.Server
{
    public interface IConnection : IDisposable
    {
        bool IsConnected { get; }

        void SendMessage(byte[] data);

        byte[] GetData();
    }
}
