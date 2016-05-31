using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Client;

namespace CS513.UserManagement
{
    public class UserManager : IUserManager
    {
        private ConcurrentDictionary<string, IUser> users; 

        public UserManager()
        {
            this.users = new ConcurrentDictionary<string, IUser>();
        }

        public IReadOnlyCollection<IUser> Users
        {
            get
            {
                return new ReadOnlyCollection<IUser>(this.users.Values.ToList());
            }
        }

        public void AddUser(string userName)
        {
            IUser user = new User(userName);
            this.users.TryAdd(userName, user);
        }

        public void RemoveUser(string userName)
        {
            IUser user;
            this.users.TryRemove(userName, out user);
        }

        public void UpdateUserName(string oldUserName, string newUserName)
        {
            IUser user;
            if (this.users.ContainsKey(oldUserName))
            {
                this.users.TryRemove(oldUserName, out user);
            }
            else
            {
                user = new User(newUserName);
            }

            this.users.TryAdd(newUserName, user);
            user.Name = newUserName;
        }
    }
}
