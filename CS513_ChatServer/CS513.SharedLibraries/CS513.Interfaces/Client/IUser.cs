using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS513.Interfaces.Client
{
    public interface IUser : INotifyPropertyChanged
    {
        string Name { get; set; }
    }
}
