using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Interfaces
{
    public interface IMongoDbConnectionConfig
    {
        string GetConnectionString();
        string GetDatabase();
    }
}
