using Ambev.DeveloperEvaluation.ORM.MongoDb.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.ORM.MongoDb
{

    public interface IMongoDbContext
    {
        IMongoCollection<SaleAuditDocument> SalesAudit { get; }
    }


    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoSettings:ConnectionString"]);
            _database = client.GetDatabase(configuration["MongoSettings:Database"]);
        }

        public IMongoCollection<SaleAuditDocument> SalesAudit =>
            _database.GetCollection<SaleAuditDocument>("sales_audit");
    }
}
