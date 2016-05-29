using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ConnectionHandlerTests
    {
        [TestMethod]
        public void UpdateName()
        {
            string firstName = "test";
            string secondName = "Tommy Two Tone";

            IConnectionHandler connectionHandler = new ConnectionHandler(null, null, firstName);
            Assert.AreNotEqual(connectionHandler.Name, secondName);
            connectionHandler.Name = secondName;
            Assert.AreEqual(connectionHandler.Name, secondName);
        }

        [TestMethod]
        public void SendMessageTest()
        {
            Mock<IConnection> connectionMock = new Mock<IConnection>();
            Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();

            IConnectionHandler connectionHandler = new ConnectionHandler(connectionMock.Object, messageHandlerMock.Object, "Test");

            Mock<IMessage> messageMock = new Mock<IMessage>();
            messageMock.Setup(message => message.Serialize()).Returns(new byte[0]);

            connectionHandler.SendMessage(messageMock.Object);

            connectionMock.Verify(connection => connection.SendMessage(It.IsAny<byte[]>()), Times.Once);
        }

        [TestMethod]
        public void ReceiveMessageTest()
        {
            TaskCompletionSource<IMessage> msgReceived = new TaskCompletionSource<IMessage>();

            Mock<IMessage> messageMock = new Mock<IMessage>();
            Mock<IConnection> connectionMock = new Mock<IConnection>();

            connectionMock.Setup(connection => connection.GetData()).Returns(new byte[0]);
            connectionMock.Setup(connection => connection.IsConnected).Returns(true);

            Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();
            messageHandlerMock.Setup(messageHandler => messageHandler.GetMessage(It.IsAny<byte[]>()))
                .Returns(messageMock.Object);

            IConnectionHandler connectionHandler = new ConnectionHandler(connectionMock.Object, messageHandlerMock.Object, "Test");

            connectionHandler.MessageReceived += (sender, message) =>
            {
                msgReceived.TrySetResult(message);
            };

            connectionHandler.Connect();

            bool success = msgReceived.Task.Wait(100);
            Assert.IsTrue(success);
            Assert.IsNotNull(msgReceived.Task.Result);
        }

        [TestMethod]
        public void DontReceiveMessageTest()
        {
            TaskCompletionSource<IMessage> msgReceived = new TaskCompletionSource<IMessage>();

            IMessage emptyMessage = null;
            Mock<IConnection> connectionMock = new Mock<IConnection>();

            connectionMock.Setup(connection => connection.GetData()).Returns(new byte[0]);
            connectionMock.Setup(connection => connection.IsConnected).Returns(true);

            Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();
            messageHandlerMock.Setup(messageHandler => messageHandler.GetMessage(It.IsAny<byte[]>()))
                .Returns(emptyMessage);

            IConnectionHandler connectionHandler = new ConnectionHandler(connectionMock.Object, messageHandlerMock.Object, "Test");

            connectionHandler.MessageReceived += (sender, message) =>
            {
                msgReceived.TrySetResult(message);
            };

            connectionHandler.Connect();

            bool success = msgReceived.Task.Wait(100);
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void DisposeTest()
        {
            TaskCompletionSource<bool> disposeReceived = new TaskCompletionSource<bool>();

            Mock<IConnection> connectionMock = new Mock<IConnection>();
            Mock<IMessageHandler> messageHandlerMock = new Mock<IMessageHandler>();

            IConnectionHandler connectionHandler = new ConnectionHandler(connectionMock.Object, messageHandlerMock.Object, "Test");

            connectionHandler.Disposing += (sender, args) =>
            {
                disposeReceived.TrySetResult(true);
            };

            connectionHandler.Dispose();

            bool success = disposeReceived.Task.Wait(1000);
            Assert.IsTrue(success);
            connectionMock.Verify(connection => connection.Dispose(), Times.Once);
        }
    }
}
