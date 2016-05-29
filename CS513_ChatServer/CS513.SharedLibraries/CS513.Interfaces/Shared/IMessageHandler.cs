namespace CS513.Interfaces.Shared
{
    public interface IMessageHandler
    {
        IMessage GetMessage(byte[] data);

        IMessage GetMessage(IMessageHeader header, string message);
    }
}
