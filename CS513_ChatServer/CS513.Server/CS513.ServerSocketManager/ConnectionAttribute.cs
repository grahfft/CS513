using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS513.ServerSocketManager
{
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
