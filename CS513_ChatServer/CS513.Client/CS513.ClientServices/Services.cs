using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Client;
using CS513.Interfaces.Shared;
using CS513.MessageHandling;
using CS513.Network;
using CS513.ResponseManagement;
using CS513.UserManagement;

namespace CS513.ClientServices
{
    public class Services
    {
        private ClientFactory factory;

        public Services(ILog chatLog)
        {
            this.ChatLog = chatLog;
            this.MessageHandler = new MessageHandler();
            this.factory = new ClientFactory(this.MessageHandler);

            this.ClientHandler = factory.GetClient();

            this.UserManager = new UserManager();
            this.ResponseManager = new ResponseManager(this.UserManager, this.ClientHandler, this.ChatLog);
        }

        public ILog ChatLog { get; private set; }

        public IMessageHandler MessageHandler { get; private set; }

        public IClientHandler ClientHandler { get; private set; }

        public IUserManager UserManager { get; private set; }

        public IResponseManager ResponseManager { get; private set; }
    }
}
