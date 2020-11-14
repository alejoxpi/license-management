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
using LM.Utils;

namespace LM.Functions
{
    public class ValidateLicense
    {
        [FunctionName("ValidateLicense")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/ValidateLicense")] HttpRequest req,
            ILogger log)
        {
            BasicResponse response = new BasicResponse();

            try
            {
                var conn = System.Environment.GetEnvironmentVariable("MongoDBConnectionString");
                var client = new MongoClient(conn);
                var dbName = System.Environment.GetEnvironmentVariable("license_management_db");
                var database = client.GetDatabase(dbName);

                var validation_requests_col = database.GetCollection<ValidationRequest>(
                    System.Environment.GetEnvironmentVariable("validation-requests-col")
                    );

                 var licenses_col = database.GetCollection<License>(
                     System.Environment.GetEnvironmentVariable("licenses-col")
                ); 

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                ValidationRequest validationRequest = JsonConvert.DeserializeObject<ValidationRequest>(requestBody); 
                validationRequest.RequestDate = DateTime.UtcNow;

                var findLicensesResult = licenses_col.Find(q=>q.LicenseHash == validationRequest.LicenseCode).ToList(); 

                if(findLicensesResult.Count == 0)
                {
                    // 0 means there is not license
                    validationRequest.Status = "Failed";
                    validationRequest.ValidationResult = "License not found.";                     
                    await validation_requests_col.InsertOneAsync(validationRequest);

                    
                    response.Status = "OK";
                    response.Code = "Failed";
                    response.Message = "License not found.";

                    return new OkObjectResult(response);
                }else if(findLicensesResult.Count > 1){
                    throw new Exception("More than 1 license with the same license code.");
                }               

                License license = findLicensesResult[0];

                LicenseValidationResponse licenseValidationResponse = new LicenseValidationResponse(); 

                //Step 1: validate setting
                if(CompareHashSettingsLicenseAndRequest(license,validationRequest))
                {
                    //Step 2: Validate remaining time
                    DateTime _now = DateTime.UtcNow;
                    TimeSpan span = _now.Subtract ( license.ActivationDate );

                    if(license.Status == "Active")
                    {
                        if(license.LifeTime > 0){

                            if(span.Minutes < license.LifeTime){

                                licenseValidationResponse.RemainingTime = license.LifeTime - span.Minutes;
                                licenseValidationResponse.valid = true;
                                licenseValidationResponse.active = true;
                                licenseValidationResponse.LicenseStatus = "Active";
                            }else{

                                licenseValidationResponse.RemainingTime = -1;
                                licenseValidationResponse.valid = false;
                                licenseValidationResponse.active = false;
                                licenseValidationResponse.LicenseStatus = "Expired";
                            }
                        }else{
                            licenseValidationResponse.RemainingTime = -1;
                            licenseValidationResponse.valid = true;
                            licenseValidationResponse.active = true;
                            licenseValidationResponse.LicenseStatus = "Active";
                        }
                    }else{
                        licenseValidationResponse.RemainingTime = -1;
                        licenseValidationResponse.valid = true;
                        licenseValidationResponse.active = false;
                        licenseValidationResponse.LicenseStatus = "Inactive";
                    }
                }else{                    
                   licenseValidationResponse.RemainingTime = -1;
                   licenseValidationResponse.valid = false;
                   licenseValidationResponse.active = false;
                   licenseValidationResponse.LicenseStatus = "Invalid";
                }           

                licenseValidationResponse.Status = "OK";
                licenseValidationResponse.Code = "Success";
                licenseValidationResponse.Message = "License validated.";

                validationRequest.ValidationSettings.HashSettings 
                    = Crypto.concatenate_sha256_hash(validationRequest.ValidationSettings.GetValueList());
                    
                validationRequest.Status = "Success";
                validationRequest.ValidationResult = licenseValidationResponse.LicenseStatus;

                await validation_requests_col.InsertOneAsync(validationRequest);

                return new OkObjectResult(licenseValidationResponse);
            }
            catch (System.Exception e)
            {   
                 
                response.Status = "OK";
                response.Code = "Excepcion";
                response.Message = e.Message;
                
                 return new OkObjectResult(response);
            }
        }
        public bool CompareHashSettingsLicenseAndRequest(License license, ValidationRequest validationRequest)
        {             
             return string.Equals(
                 Crypto.concatenate_sha256_hash(license.LicenseSettings.GetValueList()),
                 Crypto.concatenate_sha256_hash(validationRequest.ValidationSettings.GetValueList())
             );             
        }
    }
}
