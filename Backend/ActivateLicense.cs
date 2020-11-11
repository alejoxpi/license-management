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
    public static class ActivateLicense
    {
        [FunctionName("ActivateLicense")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/ActivateLicense")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var conn = System.Environment.GetEnvironmentVariable("MongoDBConnectionString");
                var client = new MongoClient(conn);
                var dbName = System.Environment.GetEnvironmentVariable("license_management_db");
                var database = client.GetDatabase(dbName);
                var collection = database.GetCollection<ActivationRequest>(
                    System.Environment.GetEnvironmentVariable("activation-requests-col")
                    );

                var licenses_col = database.GetCollection<License>(
                     System.Environment.GetEnvironmentVariable("licenses-col")
                );  

                var activationcodes_col = database.GetCollection<ActivationCode>(
                     System.Environment.GetEnvironmentVariable("activation-codes-col")
                );  



                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                ActivationRequest activationRequest = JsonConvert.DeserializeObject<ActivationRequest>(requestBody);

                var findActivationCodeResult = activationcodes_col.Find(q=>q.GuidCode == activationRequest.ActivationCode).ToList();

                //Activation Code not exists, then do nothing
                if(findActivationCodeResult.Count == 0)
                {
                    activationRequest.Status = "Failed";
                    activationRequest.ActivationResult = "Activation code not exists.";                     
                    await collection.InsertOneAsync(activationRequest);

                    BasicResponse response = new BasicResponse();
                    response.Status = "OK";
                    response.Code = "Failed";
                    response.Message = "Activation code could not be validated.";

                    return new OkObjectResult(JsonConvert.SerializeObject(response));
                }

                License license = new License();
                license.ActivationCode = activationRequest.ActivationCode;
                license.LicenseHash = activationRequest.ActivationSettings.HashSettings;

                var findLicensesResult = licenses_col.Find(q=>q.ActivationCode == license.ActivationCode).ToList();

                if(findLicensesResult.Count > 0){

                    // Greater than 0 means there is a license assined
                    activationRequest.Status = "Failed";
                    activationRequest.ActivationResult = "Activation code is already assigned";                     
                    await collection.InsertOneAsync(activationRequest);

                    BasicResponse response = new BasicResponse();
                    response.Status = "OK";
                    response.Code = "Failed";
                    response.Message = "License could not be activated.";

                    return new OkObjectResult(JsonConvert.SerializeObject(response));

                }else{
                    // 0 means there is not a license assined

                    license.LicenseSettings = new LicenseSettings();

                    license.LicenseSettings.Company = activationRequest.ActivationSettings.Company;
                    license.LicenseSettings.CustomerCode = activationRequest.ActivationSettings.CustomerCode;
                    license.LicenseSettings.Email = activationRequest.ActivationSettings.Email;
                    license.LicenseSettings.HardwareId = activationRequest.ActivationSettings.HardwareId;
                    license.LicenseSettings.Location = activationRequest.ActivationSettings.Location;                

                    await licenses_col.InsertOneAsync(license);

                    activationRequest.Status = "Success";
                    activationRequest.ActivationResult = "License has been assigned and activated.";                     
                    await collection.InsertOneAsync(activationRequest);

                    LicenseActivationSuccessResponse response = new LicenseActivationSuccessResponse();
                    response.Status = "OK";
                    response.Code = "Success";
                    response.Message = "License has been assigned and activated.";
                    response.LicenseCode = license.LicenseHash;

                    return new OkObjectResult(JsonConvert.SerializeObject(response));                    
                }
            }
            catch (System.Exception e)
            {                
                BasicResponse response = new BasicResponse();
                 
                response.Status = "OK";
                response.Code = "Excepcion";
                response.Message = e.Message;
                
                return new OkObjectResult(e.Message);  
            }           
           
        }
    }
}
