using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LM.Domain
{
    public class License    {
        
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("LicenseHash")]
        public string LicenseHash { get; set; }

        [BsonElement("ActivationCode")]
        public string ActivationCode  {get;set;} 

        [BsonElement("LicenseSettings")] 
        public LicenseSettings LicenseSettings  {get;set;}
        
    }
}