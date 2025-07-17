using Ambev.DeveloperEvaluation.ORM.MongoDb;
using Ambev.DeveloperEvaluation.ORM.MongoDb.Models;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Bogus;
using FluentAssertions;
using MongoDB.Driver;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.MongoDb
{
    public class MongoSaleAuditRepositoryTests
    {
        private readonly IMongoCollection<SaleAuditDocument> _collectionMock;
        private readonly MongoDbContext _contextMock;
        private readonly MongoSaleAuditRepository _repository;

        public MongoSaleAuditRepositoryTests()
        {
            _collectionMock = Substitute.For<IMongoCollection<SaleAuditDocument>>();
            var contextMock = Substitute.For<IMongoDbContext>();
            contextMock.SalesAudit.Returns(_collectionMock);

            _repository = new MongoSaleAuditRepository(contextMock);
        }

        [Fact(DisplayName = "Should log sale audit in Mongo when valid data is given")]
        public async Task Should_LogAudit_When_ValidSaleProvided()
        {
            // Arrange
            var faker = new Faker();
            var doc = new SaleAuditDocument
            {
                Id = Guid.NewGuid(),
                Customer = faker.Company.CompanyName(),
                Total = faker.Random.Decimal(100, 500),
                Items = new List<SaleItemDocument>
                {
                    new SaleItemDocument
                    {
                        Product = faker.Commerce.ProductName(),
                        Quantity = faker.Random.Int(1, 5),
                        UnitPrice = faker.Random.Decimal(10, 50)
                    }
                }
            };

            // Act
            Func<Task> act = async () =>
                await _repository.LogAsync(doc, CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();

            await _collectionMock
                .Received(1)
                .InsertOneAsync(doc, null, CancellationToken.None);
        }
    }
}
