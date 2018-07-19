using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace SenseMining.Importer.Models
{
    [BsonIgnoreExtraElements]
    public class DocumentModel
    {
        [BsonElement("totalSum")]
        public PaymentModel Payment { get; set; }

        [BsonElement("items")]
        public List<OrderItem> Order { get; set; }
    }
}
