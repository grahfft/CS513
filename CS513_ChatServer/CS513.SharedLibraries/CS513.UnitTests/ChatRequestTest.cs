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
    public class ChatRequestTest
    {
        [TestMethod]
        public void GenerateChatAckTest()
        {
            string sender = "Tommy Two Tone";
            string receiver = "Server";
            string contents = "8675309";

            IMessageHandler handler = new MessageHandler();
            IMessage response = null;

            Mock<IConnectionHandler> connectionHandlerMock = new Mock<IConnectionHandler>();
            connectionHandlerMock.Setup(connectionHandler => connectionHandler.Name).Returns(sender);
            connectionHandlerMock.Setup(connectionHandler => connectionHandler.SendMessage(It.IsAny<IMessage>()))
                .Callback((IMessage message) =>
                {
                    response = message;
                });

            ConcurrentDictionary<string, IConnectionHandler> connectionHandlers = new ConcurrentDictionary<string, IConnectionHandler>();
            connectionHandlers.TryAdd(connectionHandlerMock.Object.Name, connectionHandlerMock.Object);
            
            IRequest chatRequest = new ChatRequest(sender, receiver, contents);
            chatRequest.ProcessMessage(connectionHandlerMock.Object, connectionHandlers, handler);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Command, MessageCommand.ChatAck);
        }
    }
}
