using System;

namespace CS513.ServerSocketManager
{
    /// <summary>
    /// Attribute for Connections
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ConnectionAttribute : Attribute
    {
        public ConnectionAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
