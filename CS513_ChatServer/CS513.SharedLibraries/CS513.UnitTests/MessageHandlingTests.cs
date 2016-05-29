using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Shared;
using CS513.MessageHandling;
using CS513.MessageHandling.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CS513.UnitTests
{
    [TestClass]
    public class MessageHandlingTests
    {
        [TestMethod]
        public void GetMessageFromData()
        {
            IMessageHandler messageHandler = new MessageHandler();

            string sender = "Tommy TwoTone";
            string receiver = "general public";
            string contents = "8675309";

            IMessage login = new LoginRequest(sender, receiver, contents);

            IMessage message = messageHandler.GetMessage(login.Serialize());
            Assert.IsNotNull(message);
            Assert.AreEqual(sender, message.Sender);
            Assert.AreEqual(receiver, message.Receiver);
            Assert.AreEqual(contents, message.Contents);
        }

        [TestMethod]
        public void GetMessage()
        {
            IMessageHandler messageHandler = new MessageHandler();

            IMessage message = messageHandler.GetMessage("Test", "Test", "test", MessageCommand.LoginRequest);
            Assert.IsNotNull(message);
        }

        [TestMethod]
        public void GetRequestFromData()
        {
            IMessageHandler messageHandler = new MessageHandler();

            string sender = "Tommy TwoTone";
            string receiver = "general public";
            string contents = "8675309";

            IMessage login = new LoginRequest(sender, receiver, contents);

            IRequest message = messageHandler.GetRequest(login.Serialize());
            Assert.IsNotNull(message);
            Assert.AreEqual(sender, message.Sender);
            Assert.AreEqual(receiver, message.Receiver);
            Assert.AreEqual(contents, message.Contents);
        }

        [TestMethod]
        public void GetRequest()
        {
            IMessageHandler messageHandler = new MessageHandler();

            IRequest message = messageHandler.GetRequest("Test", "Test", "test", MessageCommand.LoginRequest);
            Assert.IsNotNull(message);
        }
    }
}
