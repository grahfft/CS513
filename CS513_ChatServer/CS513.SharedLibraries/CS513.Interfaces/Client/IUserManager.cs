using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS513.Interfaces.Client
{
    public interface IUserManager : INotifyPropertyChanged
    {
        IReadOnlyCollection<IUser> Users { get; }

        void AddUser(string userName);

        void RemoveUser(string userName);

        void UpdateUserName(string oldUserName, string newUserName);
    }
}
