using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Server;
using CS513.Interfaces.Shared;
using CS513.MessageHandling;
using CS513.MessageHandling.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CS513.UnitTests
{
    [TestClass]
    public class WhisperRequestTests
    {
        [TestMethod]
        public void GenerateWhisperAckTest()
        {
            string sender = "Tommy Two Tone";
            string receiver = "Umbracyl";
            string contents = "8675309";

            IMessageHandler handler = new MessageHandler();
            IMessage response = null;
            IMessage whisper = null;

            Mock<IConnectionHandler> connectionHandlerMock = new Mock<IConnectionHandler>();
            connectionHandlerMock.Setup(connectionHandler => connectionHandler.Name).Returns(sender);
            connectionHandlerMock.Setup(connectionHandler => connectionHandler.SendMessage(It.IsAny<IMessage>()))
                .Callback((IMessage message) =>
                {
                    response = message;
                });

            Mock<IConnectionHandler> receiverMock = new Mock<IConnectionHandler>();
            receiverMock.Setup(rec => rec.Name).Returns(receiver);
            receiverMock.Setup(connectionHandler => connectionHandler.SendMessage(It.IsAny<IMessage>()))
                .Callback((IMessage message) =>
                {
                    whisper = message;
                });

            ConcurrentDictionary<string, IConnectionHandler> connectionHandlers = new ConcurrentDictionary<string, IConnectionHandler>();
            connectionHandlers.TryAdd(connectionHandlerMock.Object.Name, connectionHandlerMock.Object);
            connectionHandlers.TryAdd(receiverMock.Object.Name, receiverMock.Object);

            IRequest whisperRequest = new WhisperRequest(sender, receiver, contents);
            whisperRequest.ProcessMessage(connectionHandlerMock.Object, connectionHandlers, handler);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Command, MessageCommand.WhisperAck);
            Assert.IsNotNull(whisper);
            Assert.AreEqual(whisper.Command, MessageCommand.WhisperAck);
        }

        [TestMethod]
        public void GenerateWhisperNackTest()
        {
            string sender = "Tommy Two Tone";
            string receiver = "Umbracyl";
            string contents = "8675309";

            IMessageHandler handler = new MessageHandler();
            IMessage response = null;
            IMessage whisper = null;

            Mock<IConnectionHandler> connectionHandlerMock = new Mock<IConnectionHandler>();
            connectionHandlerMock.Setup(connectionHandler => connectionHandler.Name).Returns(sender);
            connectionHandlerMock.Setup(connectionHandler => connectionHandler.SendMessage(It.IsAny<IMessage>()))
                .Callback((IMessage message) =>
                {
                    response = message;
                });

            Mock<IConnectionHandler> receiverMock = new Mock<IConnectionHandler>();
            receiverMock.Setup(rec => rec.Name).Returns(receiver);
            receiverMock.Setup(connectionHandler => connectionHandler.SendMessage(It.IsAny<IMessage>()))
                .Callback((IMessage message) =>
                {
                    whisper = message;
                });

            ConcurrentDictionary<string, IConnectionHandler> connectionHandlers = new ConcurrentDictionary<string, IConnectionHandler>();
            connectionHandlers.TryAdd(connectionHandlerMock.Object.Name, connectionHandlerMock.Object);

            IRequest whisperRequest = new WhisperRequest(sender, receiver, contents);
            whisperRequest.ProcessMessage(connectionHandlerMock.Object, connectionHandlers, handler);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Command, MessageCommand.WhisperNack);
            Assert.IsNull(whisper);
        }
    }
}
