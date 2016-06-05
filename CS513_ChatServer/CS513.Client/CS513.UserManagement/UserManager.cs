using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddUser(string userName)
        {
            if (users.ContainsKey(userName)) return;
            IUser user = new User(userName);
            this.users.TryAdd(userName, user);
            this.OnPropertyChanged("Users");
        }

        public void RemoveUser(string userName)
        {
            if (!this.users.ContainsKey(userName)) return;
            IUser user;
            this.users.TryRemove(userName, out user);
            this.OnPropertyChanged("Users");
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
            this.OnPropertyChanged("Users");
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
