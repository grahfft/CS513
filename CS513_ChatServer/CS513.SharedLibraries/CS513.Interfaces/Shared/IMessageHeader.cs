using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS513.Interfaces.Shared
{
    public interface IMessageHeader
    {
        string Sender { get; }

        string Receiver { get; }

        MessageCommand MessageCommand { get; }

        void Deserialize(byte[] data);

        byte[] Serialize();

        int DataSize();
    }
}
