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
using MongoDB.Bson.Serialization.Attributes;
using LM.Domain;
using System.Collections.Generic;
using LM.Utils;


namespace LM.Functions
{
    public class ActivationCodesGET
    {
        [FunctionName("ActivationCodesGET")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/ActivationCodes")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var conn = System.Environment.GetEnvironmentVariable("MongoDBConnectionString");
                var client = new MongoClient(conn);
                var dbName = System.Environment.GetEnvironmentVariable("license_management_db");
                var database = client.GetDatabase(dbName);
                var activationcodes_col = database.GetCollection<ActivationCode>(
                    System.Environment.GetEnvironmentVariable("activation-codes-col")
                    );

                List<ActivationCode> _list = activationcodes_col.Find( new BsonDocument() ).ToList();

                return new OkObjectResult(_list);
                
            }catch(Exception)
            {
                throw;
            }
        }
    }
}
