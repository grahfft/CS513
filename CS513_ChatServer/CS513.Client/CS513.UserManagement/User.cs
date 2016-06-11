using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Client;

namespace CS513.UserManagement
{
    /// <summary>
    /// Class designed using MVVM pattern
    /// Meant to display names of active users
    /// </summary>
    public class User : IUser
    {
        private string currentName;

        public User(string name)
        {
            this.Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// UserName
        /// </summary>
        public string Name
        {
            get
            {
                return this.currentName;
            }
            set
            {
                this.currentName = value;
                this.OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Notify UI a property has changed
        /// </summary>
        /// <param name="propertyName">property to update</param>
        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
