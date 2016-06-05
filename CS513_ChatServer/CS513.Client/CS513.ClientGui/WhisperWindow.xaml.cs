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
using System.Windows.Shapes;
using CS513.ClientServices;
using CS513.Interfaces;
using CS513.Interfaces.Client;
using CS513.Interfaces.Shared;

namespace CS513.ClientGui
{
    /// <summary>
    /// Interaction logic for WhisperWindow.xaml
    /// </summary>
    public partial class WhisperWindow : Window
    {
        private string sender;
        private Services services;

        public WhisperWindow(string sender, string receiver, Services services)
        {
            InitializeComponent();
            this.sender = sender;
            this.receiver.Text = receiver;
            this.services = services;
        }

        private void OnSendMessagePress(object sender, RoutedEventArgs e)
        {
            string sendTo = this.receiver.Text;
            string message = this.chatBox.Text;
            string from = this.sender;

            Task.Run(() =>
            {
                try
                {
                    IMessageHandler messageHandler = this.services.MessageHandler;
                    IMessage request = messageHandler.GetMessage(from, sendTo, message, MessageCommand.WhisperMessage);

                    IClientHandler clientHandler = this.services.ClientHandler;
                    clientHandler.SendMessage(request);
                }
                catch (Exception exception)
                {
                    this.services.ChatLog.LogMessage(string.Format("Unable to send whisper : {0}", exception.Message));
                }
            });

            this.Close();
        }

        private void OnCancelPress(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
