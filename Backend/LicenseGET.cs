using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Bson;
using LM.Domain;


namespace LM.Functions
{
    public static class LicenseGET
    {
        [FunctionName("LicenseGET")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/license/{id}")] HttpRequest req, string id,
            ILogger log)
        {
            try
            {
                var conn = System.Environment.GetEnvironmentVariable("MongoDBConnectionString");
                var client = new MongoClient(conn);
                var dbName = System.Environment.GetEnvironmentVariable("license_management_db");
                var database = client.GetDatabase(dbName);
                var licenses_col = database.GetCollection<License>(
                    System.Environment.GetEnvironmentVariable("licenses_col")
                    );

                
                //var filter = Builders<License>.Filter.Eq("_id", new ObjectId(id));
                var filter = Builders<License>.Filter.Eq("LicenseHash", id);                
                var _element = licenses_col.Find(filter).FirstOrDefault();

                return new OkObjectResult(_element);
                
            }
            catch (System.Exception)
            {
                
                throw;
            }           

        }
    }
}
