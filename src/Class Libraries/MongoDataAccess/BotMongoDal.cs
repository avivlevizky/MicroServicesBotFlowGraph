using Contracts.Enums;
using Contracts.Interfaces;
using Contracts.Model;
using Contracts.Model.MongoDbModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MongoDataAccess
{
    public class BotMongoDal : BotMongoDalBase, IBotMongoDal
    {

        static BotMongoDal()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(ChatFlowGraph)))
            {
                BsonClassMap.RegisterClassMap<ChatFlowGraph>();
            }
        }

        public BotMongoDal(ILogger<BotMongoDal> logger, IConfiguration Configuration) : base(logger, Configuration)
        {
        }


        public async Task<IEnumerable<FlowGraphItem>> GetAllFlowGraphItemAsync()
        {
            try
            {
                var coll = GetCollection<FlowGraphItem>(CollectionNames.DrawFlowGraphs);
                var flowGraphItems = await (await coll.FindAsync(_ => true).ConfigureAwait(false)).ToListAsync().ConfigureAwait(false);
                return flowGraphItems;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, string.Empty);
                return null;
            }
        }


        public async Task<FlowGraphItem> GetFlowGraphItemByNameIDAsync(string nameID)
        {
            try
            {
                var coll = GetCollection<FlowGraphItem>(CollectionNames.DrawFlowGraphs);
                var filter = Builders<FlowGraphItem>.Filter.Eq(p => p._t.ToLower(), nameID.ToLower());
                var res = await (await coll.FindAsync(filter)).FirstOrDefaultAsync<FlowGraphItem>().ConfigureAwait(false);
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, string.Empty);
                return null;
            }
        }



        public async Task<bool> InsertFlowGraphItemAsync(FlowGraphItem item)
        {
            try
            {
                item._id = item._id == null || item._id.Equals(ObjectId.Empty.ToString()) ?
                    ObjectId.GenerateNewId().ToString() : item._id;

                var filter = Builders<FlowGraphItem>.Filter.Where(x => x._t.Equals(item._t));
                var options = new FindOneAndReplaceOptions<FlowGraphItem, FlowGraphItem>
                {
                    IsUpsert = true,
                    ReturnDocument = ReturnDocument.After

                };
                var updatedEntity = await GetCollection<FlowGraphItem>(CollectionNames.DrawFlowGraphs).FindOneAndReplaceAsync(filter, item, options);
                return updatedEntity._id != ObjectId.Empty.ToString();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, string.Empty);
                return false;
            }


        }



        public async Task<bool> UpdateFlowGraphItemAsync(string id, FlowGraphItem item)
        {
            try
            {
                var all = await GetAllFlowGraphItemAsync();
                var filter = Builders<FlowGraphItem>.Filter.Where(x => x._id.Equals(id));

                if ((item.FlowGraphs == null) || (item.FlowGraphs.Count == 0))
                {
                    var result = await GetCollection<FlowGraphItem>(CollectionNames.DrawFlowGraphs).DeleteOneAsync(filter);
                    return result.IsAcknowledged;
                }


                var options = new FindOneAndReplaceOptions<FlowGraphItem, FlowGraphItem>
                {
                    IsUpsert = false,
                    ReturnDocument = ReturnDocument.After

                };

                var updatedEntity = await GetCollection<FlowGraphItem>(CollectionNames.DrawFlowGraphs).FindOneAndReplaceAsync(filter, item, options);

                return updatedEntity._id == id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, string.Empty);
                return false;
            }
        }
    }
}
