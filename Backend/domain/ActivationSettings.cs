using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using LM.Utils;


namespace LM.Domain
{
    public class ActivationSettings    {               

        [BsonElement("HardwareId")]
        public string HardwareId { get; set; } 

        [BsonElement("Email")]
        public string Email { get; set; } 

        [BsonElement("Company")]
        public string Company { get; set; } 

        [BsonElement("Location")]
        public string Location { get; set; } 

        [BsonElement("CustomerCode")]
        public string CustomerCode { get; set; } 

        [BsonElement("HashSettings")]
        public string HashSettings { 
            
            get
            {                
                return Crypto.sha256_hash(string.Concat(this.HardwareId,this.Email,this.Company,this.Company,this.Location,this.CustomerCode));
            }
            
        }
       
    }
}
