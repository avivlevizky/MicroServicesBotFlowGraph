using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Model.MongoDbModel;

namespace Contracts.Interfaces
{
    public interface IBotMongoDal
    {
        Task<IEnumerable<FlowGraphItem>> GetAllFlowGraphItemAsync();
        Task<FlowGraphItem> GetFlowGraphItemByNameIDAsync(string nameID);
        Task<bool> InsertFlowGraphItemAsync(FlowGraphItem item);
        Task<bool> UpdateFlowGraphItemAsync(string id, FlowGraphItem item);
    }
}
