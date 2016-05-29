using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Server;
using CS513.SocketListener;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CS513.UnitTests
{
    [TestClass]
    public class ListenerFactoryTests
    {
        [TestMethod]
        public void GetListener()
        {
            ListenerFactory factory = new ListenerFactory();
            IListener listener = factory.CreataListener();

            Assert.IsNotNull(listener);
        }

        [TestMethod]
        public void GetSameListener()
        {
            ListenerFactory factory = new ListenerFactory();
            IListener listener = factory.CreataListener();

            IListener secondListener = factory.CreataListener();

            Assert.AreEqual(listener, secondListener);
        }

        [TestMethod]
        public void StartStopListener()
        {
            ListenerFactory factory = new ListenerFactory();
            IListener listener = factory.CreataListener();

            listener.Start();

            listener.Stop();
        }
    }
}
