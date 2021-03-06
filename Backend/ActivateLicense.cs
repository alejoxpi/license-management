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
using System.Collections.Generic;
using LM.Utils;

namespace LM.Functions
{
    public class ActivateLicense
    {
        [FunctionName("ActivateLicense")]
        public async Task<IActionResult> Run(
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
                    System.Environment.GetEnvironmentVariable("activation_requests_col")
                    );

                var licenses_col = database.GetCollection<License>(
                     System.Environment.GetEnvironmentVariable("licenses_col")
                );  

                var activationcodes_col = database.GetCollection<ActivationCode>(
                     System.Environment.GetEnvironmentVariable("activation_codes_col")
                );  



                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                ActivationRequest activationRequest = JsonConvert.DeserializeObject<ActivationRequest>(requestBody);
                activationRequest.RequestDate = DateTime.UtcNow;

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

                    return new OkObjectResult(response);
                }                

                License license = new License();
                license.ActivationCode = activationRequest.ActivationCode;                

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

                    return new OkObjectResult(response);

                }else{
                    // 0 means there is not a license assined

                    ActivationCode activationCode = findActivationCodeResult[0];

                    List<string> activationSettings_ValueList = activationRequest.ActivationSettings.GetValueList();

                    // Generate hash only for settings
                    string license_hashsettings = Crypto.concatenate_sha256_hash(activationSettings_ValueList);

                    activationSettings_ValueList.Add(activationRequest.ActivationCode);

                    // Generate hash adding activation code for generate license code
                    activationRequest.ActivationSettings.HashSettings = Crypto.concatenate_sha256_hash(activationSettings_ValueList); 

                    license.LicenseHash = activationRequest.ActivationSettings.HashSettings;

                    license.LicenseSettings = new LicenseSettings();
                    license.LifeTime = activationCode.LifeTime;
                    license.Type = activationCode.LicenseType;
                    license.LicenseSettings.Company = activationRequest.ActivationSettings.Company;
                    license.LicenseSettings.CustomerCode = activationRequest.ActivationSettings.CustomerCode;
                    license.LicenseSettings.Email = activationRequest.ActivationSettings.Email;
                    license.LicenseSettings.HardwareId = activationRequest.ActivationSettings.HardwareId;
                    license.LicenseSettings.Location = activationRequest.ActivationSettings.Location; 
                    license.ActivationDate = DateTime.UtcNow;               
                    license.LicenseSettings.HashSettings = license_hashsettings;
                    license.Status = "Active";

                    

                    await licenses_col.InsertOneAsync(license);

                    activationRequest.Status = "Success";
                    activationRequest.ActivationResult = "License has been assigned and activated.";                     
                    await collection.InsertOneAsync(activationRequest);

                    LicenseActivationSuccessResponse response = new LicenseActivationSuccessResponse();
                    response.Status = "OK";
                    response.Code = "Success";
                    response.Message = "License has been assigned and activated.";
                    response.LicenseCode = license.LicenseHash;

                    return new OkObjectResult(response);                    
                }
            }
            catch (System.Exception e)
            {                
                BasicResponse response = new BasicResponse();
                 
                response.Status = "OK";
                response.Code = "Excepcion";
                response.Message = e.Message;
                
                return new OkObjectResult(response);  
            }           
           
        }
    }
}
