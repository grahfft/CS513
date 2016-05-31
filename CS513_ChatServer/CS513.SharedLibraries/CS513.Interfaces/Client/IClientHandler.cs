using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Shared;

namespace CS513.Interfaces.Client
{
    public interface IClientHandler : IDisposable
    {
        event EventHandler<IResponse> MessageReceived;

        string Name { get; set; }

        void SendMessage(IMessage message);

        void Connect();
    }
}
