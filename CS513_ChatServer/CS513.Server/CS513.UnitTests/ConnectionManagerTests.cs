using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Server;
using CS513.Interfaces.Shared;
using CS513.ServerSocketManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CS513.UnitTests
{
    [TestClass]
    public class ConnectionManagerTests
    {
        [TestMethod]
        public void StartListeningTest()
        {
            Mock<IListener> listenerMock = new Mock<IListener>();
            Mock<IConnectionFactory> factoryMock = new Mock<IConnectionFactory>();
            Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();

            IConnectionManager connectionManager = new ConnectionManager(listenerMock.Object, factoryMock.Object, messageHandlerMock.Object);

            connectionManager.Configure();

            listenerMock.Verify(listener => listener.Start(), Times.Once);
        }

        [TestMethod]
        public void ReceiveNewConnectionHandler()
        {
            TaskCompletionSource<bool> connectionReceived = new TaskCompletionSource<bool>();

            Mock<IListener> listenerMock = new Mock<IListener>();
            Mock<IConnectionHandler> connectionHandlerMock = new Mock<IConnectionHandler>();
            connectionHandlerMock.Setup(connectionHandler => connectionHandler.Name).Returns("Test");

            Mock<IConnectionFactory> factoryMock = new Mock<IConnectionFactory>();
            factoryMock.Setup(factory => factory.GetNewConnection(It.IsAny<Socket>()))
                .Returns(connectionHandlerMock.Object);

            Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();
            Socket socket = new Socket(SocketType.Stream, ProtocolType.IP);

            IConnectionManager connectionManager = new ConnectionManager(listenerMock.Object, factoryMock.Object, messageHandlerMock.Object);
            int currentCount = connectionManager.ConnectionHandlers.Count;

            connectionManager.Configure();

            listenerMock.Object.NewConnectionReceived += (sender, socket1) =>
            {
                connectionReceived.TrySetResult(true);
            };

            listenerMock.Raise(listener => listener.NewConnectionReceived += null, listenerMock.Object, socket);

            bool success = connectionReceived.Task.Wait(1000);
            Assert.IsTrue(success);
            Assert.AreEqual(connectionManager.ConnectionHandlers.Count, currentCount + 1);
        }

        [TestMethod]
        public void RemoveConnectionHandler()
        {
            TaskCompletionSource<bool> connectionReceived = new TaskCompletionSource<bool>();
            TaskCompletionSource<bool> disposeReceived = new TaskCompletionSource<bool>();

            Mock<IListener> listenerMock = new Mock<IListener>();
            Mock<IConnectionHandler> connectionHandlerMock = new Mock<IConnectionHandler>();
            connectionHandlerMock.Setup(connectionHandler => connectionHandler.Name).Returns("Test");

            Mock<IConnectionFactory> factoryMock = new Mock<IConnectionFactory>();
            factoryMock.Setup(factory => factory.GetNewConnection(It.IsAny<Socket>()))
                .Returns(connectionHandlerMock.Object);

            Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();
            Socket socket = new Socket(SocketType.Stream, ProtocolType.IP);

            IConnectionManager connectionManager = new ConnectionManager(listenerMock.Object, factoryMock.Object, messageHandlerMock.Object);
            int currentCount = connectionManager.ConnectionHandlers.Count;

            connectionManager.Configure();

            listenerMock.Object.NewConnectionReceived += (sender, socket1) =>
            {
                connectionReceived.TrySetResult(true);
            };

            listenerMock.Raise(listener => listener.NewConnectionReceived += null, listenerMock.Object, socket);

            bool success = connectionReceived.Task.Wait(1000);
            Assert.IsTrue(success);

            connectionHandlerMock.Object.Disposing += (sender, args) =>
            {
                disposeReceived.TrySetResult(true);
            };

            connectionHandlerMock.Raise(connectionHandler => connectionHandler.Disposing += null, connectionHandlerMock.Object, new EventArgs());

            bool disposeSuccess = disposeReceived.Task.Wait(1000);
            Assert.IsTrue(disposeSuccess);
            Assert.AreEqual(connectionManager.ConnectionHandlers.Count, currentCount);
        }

        [TestMethod]
        public void ProcessMessage()
        {
            TaskCompletionSource<bool> connectionReceived = new TaskCompletionSource<bool>();
            TaskCompletionSource<bool> msgProcessed = new TaskCompletionSource<bool>();

            Mock<IListener> listenerMock = new Mock<IListener>();
            Mock<IConnectionHandler> connectionHandlerMock = new Mock<IConnectionHandler>();
            connectionHandlerMock.Setup(connectionHandler => connectionHandler.Name).Returns("Test");

            Mock<IConnectionFactory> factoryMock = new Mock<IConnectionFactory>();
            factoryMock.Setup(factory => factory.GetNewConnection(It.IsAny<Socket>()))
                .Returns(connectionHandlerMock.Object);

            Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();
            Socket socket = new Socket(SocketType.Stream, ProtocolType.IP);

            IConnectionManager connectionManager = new ConnectionManager(listenerMock.Object, factoryMock.Object, messageHandlerMock.Object);

            connectionManager.Configure();

            listenerMock.Object.NewConnectionReceived += (sender, socket1) =>
            {
                connectionReceived.TrySetResult(true);
            };

            listenerMock.Raise(listener => listener.NewConnectionReceived += null, listenerMock.Object, socket);

            bool success = connectionReceived.Task.Wait(1000);
            Assert.IsTrue(success);

            Mock<IMessage> messageMock = new Mock<IMessage>();
            connectionHandlerMock.Object.MessageReceived += (sender, message) =>
            {
                msgProcessed.TrySetResult(true);
            };
            connectionHandlerMock.Raise(connectionHandler => connectionHandler.MessageReceived += null, connectionHandlerMock.Object, messageMock.Object);

            bool msgProcess = msgProcessed.Task.Wait(1000);
            Assert.IsTrue(msgProcess);
            messageMock.Verify(message => message.ProcessMessage(It.IsAny<IConnectionHandler>(), It.IsAny<ConcurrentDictionary<string, IConnectionHandler>>(), It.IsAny<IMessageHandler>()), Times.Once);
        }

        [TestMethod]
        public void DisposeConnectionManagerTest()
        {
            TaskCompletionSource<bool> connectionReceived = new TaskCompletionSource<bool>();

            Mock<IListener> listenerMock = new Mock<IListener>();
            Mock<IConnectionHandler> connectionHandlerMock = new Mock<IConnectionHandler>();
            connectionHandlerMock.Setup(connectionHandler => connectionHandler.Name).Returns("Test");

            Mock<IConnectionFactory> factoryMock = new Mock<IConnectionFactory>();
            factoryMock.Setup(factory => factory.GetNewConnection(It.IsAny<Socket>()))
                .Returns(connectionHandlerMock.Object);

            Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();
            Socket socket = new Socket(SocketType.Stream, ProtocolType.IP);

            IConnectionManager connectionManager = new ConnectionManager(listenerMock.Object, factoryMock.Object, messageHandlerMock.Object);
            int currentCount = connectionManager.ConnectionHandlers.Count;

            connectionManager.Configure();

            listenerMock.Object.NewConnectionReceived += (sender, socket1) =>
            {
                connectionReceived.TrySetResult(true);
            };

            listenerMock.Raise(listener => listener.NewConnectionReceived += null, listenerMock.Object, socket);

            bool success = connectionReceived.Task.Wait(1000);
            Assert.IsTrue(success);

            connectionManager.Dispose();

            listenerMock.Verify(listener => listener.Stop(), Times.Once);
            connectionHandlerMock.Verify(connectionHandler => connectionHandler.Dispose(), Times.Once);
        }
    }
}
