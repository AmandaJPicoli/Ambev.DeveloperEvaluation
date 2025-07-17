using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    /// <summary>
    /// Provides test data for Sale entity using Faker.
    /// </summary>
    public static class SaleTestData
    {
        private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
            .CustomInstantiator(f => new SaleItem(
                productId: f.Random.Guid(),
                quantity: f.Random.Int(1, 20),
                unitPrice: f.Finance.Amount(1, 100)));

        private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
            .CustomInstantiator(f => new Sale(
                saleNumber: $"VN-{f.Random.Number(1000, 9999)}",
                date: f.Date.Recent(30),
                customerId: f.Random.Guid(),
                branch: f.Company.CompanyName(),
                items: SaleItemFaker.Generate(f.Random.Int(1, 5))));

        /// <summary>
        /// Creates a valid Sale instance with randomized data.
        /// </summary>
        public static Sale CreateValidSale() => SaleFaker.Generate();

        /// <summary>
        /// Action to create a sale with invalid quantity (>20) to test exception.
        /// </summary>
        public static Action CreateInvalidQuantitySale => () => new Sale(
            saleNumber: $"VN-{Guid.NewGuid():N}",
            date: DateTime.UtcNow,
            customerId: Guid.NewGuid(),
            branch: "BranchB",
            items: new[]
            {
                new SaleItem(Guid.NewGuid(), 21, 10m)
            });
    }
}
