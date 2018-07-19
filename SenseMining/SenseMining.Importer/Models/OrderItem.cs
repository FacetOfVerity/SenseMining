using MongoDB.Bson.Serialization.Attributes;

namespace SenseMining.Importer.Models
{
    [BsonIgnoreExtraElements]
    public class OrderItem
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("price")]
        public PaymentModel Price { get; set; }

        [BsonElement("sum")]
        public PaymentModel Payment { get; set; }
    }
}
