using CS513.Interfaces.Server;

namespace CS513.ServerRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.Server server = new Server.Server();
            IConnectionManager manager = server.ConnectionManager;

            manager.Configure();
        }
    }
}
