using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using LM.Domain;
using System.Collections.Generic;

namespace LM.Functions
{
    public static class ActivationRequestsGET
    {
        [FunctionName("ActivationRequestsGET")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",  Route = "v1/ActivationRequests")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var conn = System.Environment.GetEnvironmentVariable("MongoDBConnectionString");
                var client = new MongoClient(conn);
                var dbName = System.Environment.GetEnvironmentVariable("license_management_db");
                var database = client.GetDatabase(dbName);
                var activationrequests_col = database.GetCollection<ActivationRequest>(
                    System.Environment.GetEnvironmentVariable("activation_requests_col")
                    );

                List<ActivationRequest> _list = activationrequests_col.Find( new BsonDocument() ).ToList();

                return new OkObjectResult(_list);
                
            }
            catch (System.Exception)
            {
                
                throw;
            }           
            
        }
    }
}
