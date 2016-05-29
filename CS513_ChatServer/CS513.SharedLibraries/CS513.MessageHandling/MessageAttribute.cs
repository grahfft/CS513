using System;
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
