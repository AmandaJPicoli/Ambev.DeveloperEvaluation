using Ambev.DeveloperEvaluation.ORM.MongoDb;
using Ambev.DeveloperEvaluation.ORM.MongoDb.Models;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public interface ISaleAuditRepository
    {
        Task LogAsync(SaleAuditDocument sale, CancellationToken cancellationToken);
    }

    public class MongoSaleAuditRepository : ISaleAuditRepository
    {
        private readonly IMongoDbContext _context;

        public MongoSaleAuditRepository(IMongoDbContext context)
        {
            _context = context;
        }

        public Task LogAsync(SaleAuditDocument sale, CancellationToken cancellationToken)
        {
            return _context.SalesAudit.InsertOneAsync(sale, cancellationToken: cancellationToken);
        }
    }
}
