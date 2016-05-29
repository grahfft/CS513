using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Server;
using CS513.Interfaces.Shared;
using CS513.ServerSocketManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CS513.UnitTests
{
    [TestClass]
    public class ConnectionFactoryTests
    {
        [TestMethod]
        public void CreateConnectionHandler()
        {
            Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();

            Socket socket = new Socket(SocketType.Stream, ProtocolType.IP);

            IConnectionFactory factory = new ConnectionFactory(messageHandlerMock.Object);
            IConnectionHandler handler = factory.GetNewConnection(socket);
            Assert.IsNotNull(handler);
        }
    }
}
