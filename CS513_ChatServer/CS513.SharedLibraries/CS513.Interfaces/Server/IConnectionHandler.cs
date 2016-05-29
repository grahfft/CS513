using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS513.Interfaces.Server
{
    public interface IConnectionHandler : IDisposable
    {
        event EventHandler<IMessage> MessageReceived;

        event EventHandler Disposing;

        string Name { get; set; }
    }
}
