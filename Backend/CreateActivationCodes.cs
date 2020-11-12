/*MIT License

Copyright (c) [2020] [JOSE ALEJANDRO BENITEZ ARAGON]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

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
                var lifetime = (int) data?.lifetime;              

                //Simple validation
                if( product_code=="" || product_name == "" || license_type == "" || quantity < 1 || lifetime < 0)
                    throw new Exception("Parameters are not valid for operation.");                

                for(int i = 0; i < quantity; i++)
                {
                    var activationCode = new ActivationCode();
                    activationCode.GuidCode = Guid.NewGuid().ToString();
                    activationCode.ProductCode = product_code;
                    activationCode.ProductName = product_name;
                    activationCode.LicenseType = license_type;
                    activationCode.LifeTime = lifetime;

                    await collection.InsertOneAsync(activationCode);
                }

                BasicResponse response = new BasicResponse();
                response.Status = "OK";
                response.Code = "Success";
                response.Message = "Activation codes created succefully.";

                return new OkObjectResult(JsonConvert.SerializeObject(response));

            }catch(Exception e)
            {
                return new OkObjectResult(e.Message);
            }

            
        }
    }
}
