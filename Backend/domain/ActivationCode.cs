using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace LM.Domain
{
public class ActivationCode{

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("GuidCode")]
        public string GuidCode { get; set; }

        [BsonElement("ProductCode")]
        public string ProductCode { get; set; }

        [BsonElement("ProductName")]
        public string ProductName { get; set; }

        [BsonElement("LicenseType")]
        public string LicenseType { get; set; }

    }
}