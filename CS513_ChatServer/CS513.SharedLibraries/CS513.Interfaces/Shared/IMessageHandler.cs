using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS513.Interfaces.Shared
{
    public interface IMessageHandler
    {
        IMessage GetMessage(byte[] data);

        IMessage GetMessage(IMessageHeader header, string message);
    }
}
