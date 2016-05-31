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

        public MainWindow()
        {
            InitializeComponent();
            ILog log = new ChatLogger(this.chatBox, this.Dispatcher);
            this.services = new Services(log);
        }

        private void OnSendMessagePress(object sender, RoutedEventArgs e)
        {

        }

        private void OnLoginPress(object sender, RoutedEventArgs e)
        {
            string loginName = this.localName.Text;
            Task.Run(() =>
            {
                IResponseManager responseManager = this.services.ResponseManager;
                responseManager.Start();

                IMessageHandler messageHandler = this.services.MessageHandler;
                IMessage request = messageHandler.GetMessage(loginName, "server", loginName, MessageCommand.LoginRequest);

                IClientHandler clientHandler = this.services.ClientHandler;
                clientHandler.SendMessage(request);
            });
        }

        private void OnUpdateNamePress(object sender, RoutedEventArgs e)
        {

        }
    }
}
