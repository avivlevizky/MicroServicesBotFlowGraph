using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BotFlowGraph.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAuthController : ControllerBase
    {
       
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var client = new HttpClient();

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = @"http://botflowgraph-identity/connect/token",
                ClientId = "client",
                ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A",
                Scope = "BotWebAPI"
            });

            var gatewayClient = new HttpClient();

            gatewayClient.SetBearerToken(tokenResponse.AccessToken);
            var response = await gatewayClient.GetAsync("http://apigw-base/api/values");
            return new JsonResult(response);
        }



    }
}