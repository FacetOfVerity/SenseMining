using MongoDB.Bson.Serialization.Attributes;

namespace SenseMining.Importer.Models
{
    public class PaymentModel
    {
        [BsonElement("display")]
        public string Display { get; set; }

        [BsonElement("approx")]
        public decimal Value { get; set; }
    }
}
