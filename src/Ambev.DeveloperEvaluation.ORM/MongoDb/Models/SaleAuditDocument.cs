using MongoDB.Bson.Serialization.Attributes;

namespace Ambev.DeveloperEvaluation.ORM.MongoDb.Models
{
    public class SaleAuditDocument
    {
        [BsonId]
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Customer { get; set; }
        public decimal Total { get; set; }
        public List<SaleItemDocument> Items { get; set; }
    }

    public class SaleItemDocument
    {
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
