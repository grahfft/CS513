using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Client;
using CS513.Interfaces.Shared;

namespace CS513.ResponseManagement
{
    public class ResponseManager : IResponseManager
    {
        private IUserManager userManager;

        private IClientHandler clientHandler;

        private ILog log;

        public ResponseManager(IUserManager userManager, IClientHandler clientHandler, ILog log)
        {
            this.userManager = userManager;
            this.clientHandler = clientHandler;
            this.log = log;
        }

        public void Start()
        {
            try
            {
                this.clientHandler.MessageReceived += this.HandleResponse;
                this.clientHandler.Connect();
            }
            catch (Exception exception)
            {
                
            }
        }

        public void Stop()
        {
            this.clientHandler.MessageReceived -= this.HandleResponse;
        }

        private void HandleResponse(object sender, IResponse response)
        {
            Task.Run(() =>
            {
                response.ProcessMessage(this.userManager, this.log);
            });
        }
    }
}
