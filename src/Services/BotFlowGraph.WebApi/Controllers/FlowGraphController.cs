using Contracts.Model.MongoDbModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotFlowGraph.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowGraphController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly BotMongoDal _botMongoDal;

        public FlowGraphController(ILogger<FlowGraphController> logger, BotMongoDal botMongoDal)
        {
            _logger = logger;
            _botMongoDal = botMongoDal;
        }

        [HttpGet]
        // GET: api/FlowGraph
        public async Task<IEnumerable<FlowGraphItem>> Get()
        {
            return await _botMongoDal.GetAllFlowGraphItemAsync();
        }




        // POST: api/FlowGraph
        // For create operation
        [HttpPost("{id}")]
        public async Task<bool> Post([FromBody]FlowGraphItem value)
        {
            try
            {
                return await _botMongoDal.InsertFlowGraphItemAsync(value).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return false;
            }


        }


        // PUT: api/FlowGraph/5
        // For update operation
        [HttpPut("{id}")]
        public async Task<bool> Put(string id, [FromBody]FlowGraphItem value)
        {
            try
            {
                return await _botMongoDal.UpdateFlowGraphItemAsync(value._id, value).ConfigureAwait(false); ;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
