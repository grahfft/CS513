﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS513.Interfaces.Server
{
    public interface IConnectionManager : IDisposable
    {
        void Configure();
    }
}