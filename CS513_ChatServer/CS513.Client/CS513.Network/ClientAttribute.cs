using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS513.Network
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ClientAttribute : Attribute
    {
        public ClientAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
