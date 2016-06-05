using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CS513.ChatHandling;
using CS513.ClientServices;
using CS513.Interfaces;
using CS513.Interfaces.Client;
using CS513.Interfaces.Shared;

namespace CS513.ClientGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Services services;

        private string Name;

        public MainWindow()
        {
            InitializeComponent();
            ILog log = new ChatLogger(this.chatBox, this.Dispatcher);
            this.services = new Services(log);
            this.currentUsers.DataContext = this.services.UserManager;

            this.Closed += this.SendDisconnect;
        }

        private void SendDisconnect(object sender, EventArgs e)
        {
            try
            {
                IMessageHandler messageHandler = this.services.MessageHandler;
                IMessage request = messageHandler.GetMessage(this.Name, "server", this.Name,
                    MessageCommand.DisconnectRequest);

                IClientHandler clientHandler = this.services.ClientHandler;
                clientHandler.SendMessage(request);
            }
            catch (Exception exception)
            {
                this.services.ChatLog.LogMessage(string.Format("Unable to send disconnect : {0}", exception.Message));
            }
        }

        private void OnSendMessagePress(object sender, RoutedEventArgs e)
        {
            //Capture message text
            string message = this.messageText.Text;
            string messageSender = this.Name;

            //send message
            Task.Run(() =>
            {
                try
                {
                    IMessageHandler messageHandler = this.services.MessageHandler;
                    IMessage request = messageHandler.GetMessage(messageSender, "server", message,
                        MessageCommand.ChatMessage);

                    IClientHandler clientHandler = this.services.ClientHandler;
                    clientHandler.SendMessage(request);
                }
                catch (Exception exception)
                {
                    this.services.ChatLog.LogMessage(string.Format("Unable to send message : {0}", exception.Message));
                }
            });

            //Clear message text from box
            this.messageText.Clear();
        }

        private void OnLoginPress(object sender, RoutedEventArgs e)
        {
            //purposely using variable capturing here
            string loginName = this.localName.Text;
            this.Name = loginName;

            //get sending task off the GUI thread
            Task.Run(() =>
            {
                try
                {
                    IResponseManager responseManager = this.services.ResponseManager;
                    responseManager.Start();

                    IMessageHandler messageHandler = this.services.MessageHandler;
                    IMessage request = messageHandler.GetMessage(loginName, "server", loginName,
                        MessageCommand.LoginRequest);

                    IClientHandler clientHandler = this.services.ClientHandler;
                    clientHandler.SendMessage(request);
                }
                catch (Exception exception)
                {
                    this.services.ChatLog.LogMessage(string.Format("Unable to send log in : {0}", exception.Message));
                }
            });
        }

        private void OnUpdateNamePress(object sender, RoutedEventArgs e)
        {
            //purposely using variable capturing here
            string newName = this.localName.Text;
            string oldName = this.Name;
            this.Name = newName;
            
            //get sending task off the GUI thread
            Task.Run(() =>
            {
                try
                {
                    IMessageHandler messageHandler = this.services.MessageHandler;
                    IMessage request = messageHandler.GetMessage(oldName, "server", newName, MessageCommand.UpdateName);

                    IClientHandler clientHandler = this.services.ClientHandler;
                    clientHandler.SendMessage(request);
                }
                catch (Exception exception)
                {
                    this.services.ChatLog.LogMessage(string.Format("Unable to send name update : {0}", exception.Message));
                }
            });
        }

        /// <summary>
        /// Method for whispering a user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWhisperUser(object sender, RoutedEventArgs e)
        {
            IUser user = (IUser)this.currentUsers.SelectedItem;
            if (user != null)
            {
                WhisperWindow whisper = new WhisperWindow(this.Name, user.Name, this.services);

                whisper.ShowDialog();
            }
        }
    }
}
