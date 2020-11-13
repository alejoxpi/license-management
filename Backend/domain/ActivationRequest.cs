using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace LM.Domain
{
    public class ActivationRequest    {
        
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("ActivationCode")]
        public string ActivationCode  {get;set;} 

        [BsonElement("ActivationSettings")] 
        public ActivationSettings ActivationSettings {get;set;}

        [BsonElement("Status")] 
        public string Status {get;set;}

        [BsonElement("ActivationResult")] 
        public string ActivationResult {get;set;}

        [BsonElement("RequestDate")] 
        public DateTime RequestDate { get; set; }

    }
}