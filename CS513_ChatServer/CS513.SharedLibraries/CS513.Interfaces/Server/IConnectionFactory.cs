using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CS513.Interfaces.Server
{
    public interface IConnectionFactory
    {
        IConnectionHandler GetNewConnection(Socket socket);
    }
}
