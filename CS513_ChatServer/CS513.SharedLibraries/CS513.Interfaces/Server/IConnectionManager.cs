using System;
using System.Collections.Generic;

namespace CS513.Interfaces.Server
{
    public interface IConnectionManager : IDisposable
    {
        IReadOnlyDictionary<string, IConnectionHandler> ConnectionHandlers { get; }
 
        void Configure();
    }
}
