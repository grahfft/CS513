using System;

namespace CS513.Interfaces.Server
{
    public interface IConnectionManager : IDisposable
    {
        void Configure();
    }
}
