using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS513.Interfaces.Server;
using CS513.Interfaces.Shared;

namespace CS513.Interfaces
{
    public enum MessageCommand
    {
        ConnectionAck = 0,
        LoginRequest = 1,
        LoginAck = 2,
        LoginNack = 3,
        ChatMessage = 4,
        ChatAck = 5,
        UpdateName = 6,
        UpdateAck = 7,
        UpdateNack = 8,
        WhisperMessage = 9,
        WhisperAck = 10,
        WhisperNack = 11,
    }

    /// <summary>
    /// Interface for message sent over the wire
    /// </summary>
    public interface IMessage
    {
        void ProcessMessage(IConnectionHandler connection, ConcurrentDictionary<string, IConnectionHandler> connectionHandlers, IMessageHandler handler);

        byte[] Serialize();

        void Deserialize(byte[] data);
    }
}
