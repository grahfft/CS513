using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Client;

namespace CS513.Interfaces.Shared
{
    public interface IResponse : IMessage
    {
        void ProcessMessage(IUserManager userManager, ILog log);
    }
}
