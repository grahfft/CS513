using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS513.Interfaces.Client
{
    public interface IClient : IDisposable
    {
        bool IsConnected { get; }

        void SendMessage(byte[] data);

        byte[] GetData();

        void Connect();
    }
}
