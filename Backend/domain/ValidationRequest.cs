using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LM.Domain
{
    public class ValidationRequest    {
        
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("LicenseCode")]
        public string LicenseCode  {get;set;} 

        [BsonElement("ValidationSettings")] 
        public ValidationSettings ValidationSettings {get;set;}

        [BsonElement("Status")] 
        public string Status {get;set;}

        [BsonElement("ValidationResult")] 
        public string ValidationResult {get;set;}


    }
}