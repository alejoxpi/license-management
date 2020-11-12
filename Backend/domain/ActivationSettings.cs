using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;



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
        public string HashSettings { get; set;}

        public List<string> GetValueList()
        {
            List<string> _list = new List<string>();

            _list.Add(HardwareId);
            _list.Add(Email);
            _list.Add(Company);
            _list.Add(Location);
            _list.Add(CustomerCode);          

            return _list;
        }
    }
}