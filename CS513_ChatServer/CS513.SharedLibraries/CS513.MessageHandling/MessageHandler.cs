﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces;
using CS513.Interfaces.Shared;

namespace CS513.MessageHandling
{
    public class MessageHandler : IMessageHandler
    {


        public MessageHandler()
        {
            
        }

        public IMessage GetMessage(byte[] data)
        {
            return null;
        }
    }
}
