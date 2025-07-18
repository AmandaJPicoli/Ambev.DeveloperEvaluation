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
        /// <returns>A valid Sale entity</returns>
        public static Sale CreateValidSale() => SaleFaker.Generate();

        /// <summary>
        /// Creates a valid Sale instance with specific number of items.
        /// </summary>
        /// <param name="itemCount">Number of items to include in the sale</param>
        /// <returns>A valid Sale entity with specified item count</returns>
        public static Sale CreateValidSaleWithItems(int itemCount)
        {
            var faker = new Faker();
            return new Sale(
                saleNumber: $"VN-{faker.Random.Number(1000, 9999)}",
                date: faker.Date.Recent(30),
                customerId: faker.Random.Guid(),
                branch: faker.Company.CompanyName(),
                items: SaleItemFaker.Generate(itemCount));
        }

        /// <summary>
        /// Creates a valid Sale instance with specific sale number.
        /// </summary>
        /// <param name="saleNumber">The sale number to use</param>
        /// <returns>A valid Sale entity with specified sale number</returns>
        public static Sale CreateValidSaleWithNumber(string saleNumber)
        {
            var faker = new Faker();
            return new Sale(
                saleNumber: saleNumber,
                date: faker.Date.Recent(30),
                customerId: faker.Random.Guid(),
                branch: faker.Company.CompanyName(),
                items: SaleItemFaker.Generate(faker.Random.Int(1, 3)));
        }

        /// <summary>
        /// Creates a valid Sale instance with specific customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID to use</param>
        /// <returns>A valid Sale entity with specified customer ID</returns>
        public static Sale CreateValidSaleForCustomer(Guid customerId)
        {
            var faker = new Faker();
            return new Sale(
                saleNumber: $"VN-{faker.Random.Number(1000, 9999)}",
                date: faker.Date.Recent(30),
                customerId: customerId,
                branch: faker.Company.CompanyName(),
                items: SaleItemFaker.Generate(faker.Random.Int(1, 3)));
        }

        /// <summary>
        /// Creates a valid Sale instance with specific branch.
        /// </summary>
        /// <param name="branch">The branch name to use</param>
        /// <returns>A valid Sale entity with specified branch</returns>
        public static Sale CreateValidSaleForBranch(string branch)
        {
            var faker = new Faker();
            return new Sale(
                saleNumber: $"VN-{faker.Random.Number(1000, 9999)}",
                date: faker.Date.Recent(30),
                customerId: faker.Random.Guid(),
                branch: branch,
                items: SaleItemFaker.Generate(faker.Random.Int(1, 3)));
        }

        /// <summary>
        /// Creates a valid Sale instance with items having specific quantities for discount testing.
        /// </summary>
        /// <param name="quantities">Array of quantities for each item</param>
        /// <returns>A valid Sale entity with specified item quantities</returns>
        public static Sale CreateSaleWithSpecificQuantities(params int[] quantities)
        {
            var faker = new Faker();
            var items = quantities.Select(qty => new SaleItem(
                faker.Random.Guid(),
                qty,
                faker.Finance.Amount(10, 50)
            )).ToList();

            return new Sale(
                saleNumber: $"VN-{faker.Random.Number(1000, 9999)}",
                date: faker.Date.Recent(30),
                customerId: faker.Random.Guid(),
                branch: faker.Company.CompanyName(),
                items: items);
        }

        /// <summary>
        /// Creates a cancelled sale for testing.
        /// </summary>
        /// <returns>A cancelled Sale entity</returns>
        public static Sale CreateCancelledSale()
        {
            var sale = CreateValidSale();
            sale.CancelSale();
            return sale;
        }

        /// <summary>
        /// Creates a valid SaleItem with specific parameters.
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="unitPrice">Unit price</param>
        /// <returns>A valid SaleItem entity</returns>
        public static SaleItem CreateValidSaleItem(Guid? productId = null, int? quantity = null, decimal? unitPrice = null)
        {
            var faker = new Faker();
            return new SaleItem(
                productId ?? faker.Random.Guid(),
                quantity ?? faker.Random.Int(1, 20),
                unitPrice ?? faker.Finance.Amount(1, 100)
            );
        }

        /// <summary>
        /// Creates multiple valid SaleItems.
        /// </summary>
        /// <param name="count">Number of items to create</param>
        /// <returns>Collection of valid SaleItem entities</returns>
        public static List<SaleItem> CreateValidSaleItems(int count)
        {
            return SaleItemFaker.Generate(count);
        }

        /// <summary>
        /// Creates SaleItems with different quantities for discount testing.
        /// </summary>
        /// <returns>Collection of SaleItems with various quantities</returns>
        public static List<SaleItem> CreateSaleItemsForDiscountTesting()
        {
            var faker = new Faker();
            return new List<SaleItem>
            {
                new SaleItem(faker.Random.Guid(), 2, 10.00m),   // No discount
                new SaleItem(faker.Random.Guid(), 5, 20.00m),   // 10% discount
                new SaleItem(faker.Random.Guid(), 15, 30.00m),  // 20% discount
            };
        }

        /// <summary>
        /// Action to create a sale with invalid quantity (>20) to test business rule.
        /// </summary>
        public static Action CreateInvalidQuantitySale => () => new Sale(
            saleNumber: $"VN-{Guid.NewGuid():N}",
            date: DateTime.UtcNow,
            customerId: Guid.NewGuid(),
            branch: "Test Branch",
            items: new[]
            {
                new SaleItem(Guid.NewGuid(), 21, 10m) // Invalid quantity > 20
            });

        /// <summary>
        /// Action to create a sale with empty sale number to test validation.
        /// </summary>
        public static Action CreateSaleWithEmptyNumber => () => new Sale(
            saleNumber: string.Empty,
            date: DateTime.UtcNow,
            customerId: Guid.NewGuid(),
            branch: "Test Branch",
            items: new[]
            {
                new SaleItem(Guid.NewGuid(), 5, 10m)
            });

        /// <summary>
        /// Action to create a sale with empty customer ID to test validation.
        /// </summary>
        public static Action CreateSaleWithEmptyCustomerId => () => new Sale(
            saleNumber: "VN-001",
            date: DateTime.UtcNow,
            customerId: Guid.Empty,
            branch: "Test Branch",
            items: new[]
            {
                new SaleItem(Guid.NewGuid(), 5, 10m)
            });

        /// <summary>
        /// Action to create a sale with empty branch to test validation.
        /// </summary>
        public static Action CreateSaleWithEmptyBranch => () => new Sale(
            saleNumber: "VN-001",
            date: DateTime.UtcNow,
            customerId: Guid.NewGuid(),
            branch: string.Empty,
            items: new[]
            {
                new SaleItem(Guid.NewGuid(), 5, 10m)
            });

        /// <summary>
        /// Action to create a sale with no items to test validation.
        /// </summary>
        public static Action CreateSaleWithNoItems => () => new Sale(
            saleNumber: "VN-001",
            date: DateTime.UtcNow,
            customerId: Guid.NewGuid(),
            branch: "Test Branch",
            items: new SaleItem[0]);

        /// <summary>
        /// Action to create a sale item with empty product ID to test validation.
        /// </summary>
        public static Action CreateSaleItemWithEmptyProductId => () => new SaleItem(
            Guid.Empty,
            5,
            10m);

        /// <summary>
        /// Action to create a sale item with zero quantity to test validation.
        /// </summary>
        public static Action CreateSaleItemWithZeroQuantity => () => new SaleItem(
            Guid.NewGuid(),
            0,
            10m);

        /// <summary>
        /// Action to create a sale item with negative quantity to test validation.
        /// </summary>
        public static Action CreateSaleItemWithNegativeQuantity => () => new SaleItem(
            Guid.NewGuid(),
            -1,
            10m);

        /// <summary>
        /// Action to create a sale item with zero unit price to test validation.
        /// </summary>
        public static Action CreateSaleItemWithZeroPrice => () => new SaleItem(
            Guid.NewGuid(),
            5,
            0m);

        /// <summary>
        /// Action to create a sale item with negative unit price to test validation.
        /// </summary>
        public static Action CreateSaleItemWithNegativePrice => () => new SaleItem(
            Guid.NewGuid(),
            5,
            -10m);

        /// <summary>
        /// Creates a sale with specific date for testing.
        /// </summary>
        /// <param name="date">The date to use for the sale</param>
        /// <returns>A valid Sale entity with specified date</returns>
        public static Sale CreateSaleWithDate(DateTime date)
        {
            var faker = new Faker();
            return new Sale(
                saleNumber: $"VN-{faker.Random.Number(1000, 9999)}",
                date: date,
                customerId: faker.Random.Guid(),
                branch: faker.Company.CompanyName(),
                items: SaleItemFaker.Generate(1));
        }

        /// <summary>
        /// Creates a sale with specific total amount for testing.
        /// </summary>
        /// <param name="targetAmount">Target total amount (approximate)</param>
        /// <returns>A valid Sale entity with total close to target amount</returns>
        public static Sale CreateSaleWithApproximateTotal(decimal targetAmount)
        {
            var faker = new Faker();
            var unitPrice = targetAmount / 5; // Create 5 items
            var items = Enumerable.Range(1, 5)
                .Select(_ => new SaleItem(faker.Random.Guid(), 1, unitPrice))
                .ToList();

            return new Sale(
                saleNumber: $"VN-{faker.Random.Number(1000, 9999)}",
                date: faker.Date.Recent(30),
                customerId: faker.Random.Guid(),
                branch: faker.Company.CompanyName(),
                items: items);
        }
    }
}