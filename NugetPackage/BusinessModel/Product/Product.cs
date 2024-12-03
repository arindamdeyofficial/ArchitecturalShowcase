using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BusinessModel.Product
{
    public class ProductBo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("Text")]
        public string Text { get; set; }
        [BsonElement("Version")]
        public string Version { get; set; }
        [BsonElement("SystemId")]
        public string SystemId { get; set; }
        [BsonElement("SystemType")]
        public string SystemType { get; set; }
    }
}
