using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using LM.Domain;


namespace LM.Functions
{
    public static class CreateActivationCodes
    {
        [FunctionName("CreateActivationCodes")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/CreateActivationCodes")] HttpRequest req,
            ILogger log)
        {

            try{
                var conn = System.Environment.GetEnvironmentVariable("MongoDBConnectionString");
                var client = new MongoClient(conn);
                var dbName = System.Environment.GetEnvironmentVariable("license_management_db");
                var database = client.GetDatabase(dbName);
                var collection = database.GetCollection<ActivationCode>(
                    System.Environment.GetEnvironmentVariable("activation-codes-col")
                    );

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                var product_code = ((string)  data?.productcode).Trim() ;
                var product_name = ((string) data?.productname).Trim();
                var license_type = ((string) data?.licensetype).Trim();                
                var quantity = (int) data?.quantity;                

                //Simple validation
                if( product_code=="" || product_name == "" || license_type == "" || quantity < 1 )
                    throw new Exception("Parameters no valid for operation.");                

                for(int i = 0; i < quantity; i++)
                {
                    var activationCode = new ActivationCode();
                    activationCode.GuidCode = Guid.NewGuid().ToString();
                    activationCode.ProductCode = product_code;
                    activationCode.ProductName = product_name;
                    activationCode.LicenseType = license_type;

                    await collection.InsertOneAsync(activationCode);
                }

                return new OkObjectResult("Success");

            }catch(Exception e)
            {
                return new OkObjectResult(e.Message);
            }

            
        }
    }
}
