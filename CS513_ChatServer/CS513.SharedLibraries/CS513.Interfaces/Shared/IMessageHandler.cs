namespace CS513.Interfaces.Shared
{
    public interface IMessageHandler
    {
        IMessage GetMessage(byte[] data);

        IMessage GetMessage(string sender, string receiver, string contents, MessageCommand command);

        IRequest GetRequest(byte[] data);

        IRequest GetRequest(string sender, string receiver, string contents, MessageCommand command);

        IResponse GetResponse(byte[] data);

        IResponse GetResponse(string sender, string receiver, string contents, MessageCommand command);
    }
}
