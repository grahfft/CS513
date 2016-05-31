using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS513.Interfaces.Client
{
    public interface ILog
    {
        void LogChat(string sender, string message);

        void LogWhisper(string sender, string message);

        void LogMessage(string message);
    }
}
