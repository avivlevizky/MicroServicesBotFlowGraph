using Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Model
{
    public class MongoDbConnectionConfig: IMongoDbConnectionConfig
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string GetConnectionString() => ConnectionString;
        public string GetDatabase() => Database;
       
    }
}
