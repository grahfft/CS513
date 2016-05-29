using System;

namespace CS513.SocketListener
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ListenerAttribute : Attribute
    {
        public ListenerAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
