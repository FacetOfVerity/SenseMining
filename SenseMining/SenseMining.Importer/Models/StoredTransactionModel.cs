using MongoDB.Bson.Serialization.Attributes;

namespace SenseMining.Importer.Models
{
    [BsonIgnoreExtraElements]
    public class StoredTransactionModel
    {
        [BsonElement("regRetailAddress")]
        public string RetailAddress { get; set; }

        [BsonElement("doc")]
        public DocumentModel Document { get; set; }
    }
}
