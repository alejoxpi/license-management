using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace LM.Domain
{
    public class License    {
        
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("LicenseHash")]
        public string LicenseHash { get; set; }

        [BsonElement("ActivationCode")]
        public string ActivationCode  {get;set;} 

        [BsonElement("ActivationDate")] 
        public DateTime ActivationDate { get; set; }

        [BsonElement("LifeTime")] 
        public int LifeTime { get; set; }

        [BsonElement("Type")]
        public string Type { get; set; }

        [BsonElement("LicenseSettings")] 
        public LicenseSettings LicenseSettings  {get;set;}
        
    }
}