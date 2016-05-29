using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;

namespace CS513.MessageHandling
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MessageAttribute : Attribute
    {
        public MessageAttribute(MessageCommand command)
        {
            this.Command = command;
        }

        public MessageCommand Command { get; private set; }
    }
}
