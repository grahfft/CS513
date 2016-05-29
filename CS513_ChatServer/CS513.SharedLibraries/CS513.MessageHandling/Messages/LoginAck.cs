﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Server;
using CS513.Interfaces.Shared;

namespace CS513.MessageHandling.Messages
{
    /// <summary>
    /// Acknowledge response for a login
    /// Strongly defined in case an IResponse needs to be implemented
    /// </summary>
    [Message(MessageCommand.LoginAck)]
    public class LoginAck : Message, IMessage
    {
        public LoginAck()
        {
            this.Command = MessageCommand.LoginAck;
        }

        public LoginAck(string sender, string receiver, string contents)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Contents = contents;
            this.Command = MessageCommand.LoginAck;
        }
    }
}