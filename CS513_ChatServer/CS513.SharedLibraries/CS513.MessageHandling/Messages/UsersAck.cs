using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Client;
using CS513.Interfaces.Shared;

namespace CS513.MessageHandling.Messages
{
    [Message(MessageCommand.UsersAck)]
    public class UsersAck : Message, IResponse
    {
        public UsersAck()
        {
            this.Command = MessageCommand.UsersAck;
        }

        public UsersAck(string sender, string receiver, string contents)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Contents = contents;
            this.Command = MessageCommand.UsersAck;
        }

        public void ProcessMessage(IUserManager userManager, ILog log)
        {
            string[] names = this.Contents.Split(' ');
            foreach (string name in names)
            {
                userManager.AddUser(name);
            }
        }
    }
}
