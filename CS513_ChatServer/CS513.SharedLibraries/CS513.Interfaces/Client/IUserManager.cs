using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS513.Interfaces.Client
{
    public interface IUserManager
    {
        void AddUser(string userName);

        void RemoveUser(string userName);

        void UpdateUserName(string oldUserName, string newUserName);
    }
}
