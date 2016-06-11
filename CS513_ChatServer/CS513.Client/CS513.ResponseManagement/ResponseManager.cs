using System;
using CS513.Interfaces.Client;
using CS513.Interfaces.Shared;

namespace CS513.ResponseManagement
{
    /// <summary>
    /// Class set up to provide necessary information to process responses
    /// </summary>
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

        /// <summary>
        /// Starts the processing of messages and conencts the client to the server
        /// </summary>
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

        /// <summary>
        /// Stops listening for events underneath
        /// </summary>
        public void Stop()
        {
            this.clientHandler.MessageReceived -= this.HandleResponse;
        }

        /// <summary>
        /// Response handler
        /// </summary>
        /// <param name="sender">ClientHandler</param>
        /// <param name="response">Resposne from server</param>
        private void HandleResponse(object sender, IResponse response)
        {
            response.ProcessMessage(this.userManager, this.log);
        }
    }
}
