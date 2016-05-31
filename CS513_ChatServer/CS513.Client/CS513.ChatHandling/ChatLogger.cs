using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using CS513.Interfaces.Client;

namespace CS513.ChatHandling
{
    public class ChatLogger : ILog
    {
        private TextBox logger;

        private Dispatcher thread;

        public ChatLogger(TextBox logger, Dispatcher thread)
        {
            this.logger = logger;
            this.thread = thread;
        }

        public void LogChat(string sender, string message)
        {
            string output = string.Format("{0} says: {1}\n", sender, message);
            this.thread.BeginInvoke(new Action(() =>
            {
                this.PrintMsg(output);
            }));
        }

        public void LogWhisper(string sender, string message)
        {
            string output = string.Format("{0} whispers: {1}\n", sender, message);
            this.thread.BeginInvoke(new Action(() =>
            {
                this.PrintMsg(output);
            }));
        }

        public void LogMessage(string message)
        {
            string output = string.Format("{0}\n", message);
            this.thread.BeginInvoke(new Action(() =>
            {
                this.PrintMsg(output);
            }));
        }

        private void PrintMsg(string message)
        {
            this.logger.AppendText(message);
            this.logger.ScrollToEnd();
            this.logger.InvalidateVisual();
        }
    }
}
