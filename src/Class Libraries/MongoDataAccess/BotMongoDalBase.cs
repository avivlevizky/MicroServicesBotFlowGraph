using Contracts.Enums;
using Contracts.Model;
using Contracts.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace MongoDataAccess
{
    public abstract class BotMongoDalBase
    {
        private IMongoDbConnectionConfig _mongoDbConfig;
        protected readonly ILogger<BotMongoDalBase> logger;


        public BotMongoDalBase(ILogger<BotMongoDalBase> logger, IConfiguration Configuration)
        {       
            _mongoDbConfig = new MongoDbConnectionConfig
            {
                ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value,
                Database = Configuration.GetSection("MongoConnection:Database").Value
            };
        }


        private IMongoDatabase GetDatabase()
        {
            return new MongoClient(_mongoDbConfig.GetConnectionString()).GetDatabase(_mongoDbConfig.GetDatabase());
        }

        protected IMongoCollection<T> GetCollection<T>(CollectionNames collection)
        {
            return GetDatabase().GetCollection<T>(collection.ToString());
        }


    }
}
