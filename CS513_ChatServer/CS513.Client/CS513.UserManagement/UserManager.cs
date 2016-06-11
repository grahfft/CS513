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
    /// <summary>
    /// Class dedicated for updating its internal concurrent dictionary
    /// Holds on current online users
    /// </summary>
    public class UserManager : IUserManager
    {
        private ConcurrentDictionary<string, IUser> users; 

        public UserManager()
        {
            this.users = new ConcurrentDictionary<string, IUser>();
        }

        /// <summary>
        /// Collection of Users for the UI to bind to
        /// </summary>
        public IReadOnlyCollection<IUser> Users
        {
            get
            {
                return new ReadOnlyCollection<IUser>(this.users.Values.ToList());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Adds a User to the internal dictionary
        /// </summary>
        /// <param name="userName">user name of user to be added</param>
        public void AddUser(string userName)
        {
            if (users.ContainsKey(userName)) return;
            IUser user = new User(userName);
            this.users.TryAdd(userName, user);
            this.OnPropertyChanged("Users");
        }

        /// <summary>
        /// Removes a User from the current active user list
        /// </summary>
        /// <param name="userName">user name of user to be removed</param>
        public void RemoveUser(string userName)
        {
            if (!this.users.ContainsKey(userName)) return;
            IUser user;
            this.users.TryRemove(userName, out user);
            this.OnPropertyChanged("Users");
        }

        /// <summary>
        /// Updates the user name of a user
        /// </summary>
        /// <param name="oldUserName">old user name of the user</param>
        /// <param name="newUserName">new user name of the user</param>
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

        /// <summary>
        /// Notifies the UI to update its User list
        /// </summary>
        /// <param name="propertyName"></param>
        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
