using System.Collections.Concurrent;
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
        string Sender { get; }

        string Receiver { get; }

        string Contents { get; }

        MessageCommand Command { get; }

        byte[] Serialize();

        void Deserialize(byte[] data);
    }
}
