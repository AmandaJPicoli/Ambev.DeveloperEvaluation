using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    /// <summary>
    /// Provides test data for CreateSaleHandler tests using Bogus.
    /// </summary>
    public static class CreateSaleHandlerTestData
    {
        private static readonly Faker<CreateSaleCommand> CommandFaker = new Faker<CreateSaleCommand>()
            .RuleFor(cmd => cmd.SaleNumber, f => $"VN-{f.Random.Number(1000, 9999)}")
            .RuleFor(cmd => cmd.Date, f => f.Date.Recent(10))
            .RuleFor(cmd => cmd.CustomerId, f => f.Random.Guid())
            .RuleFor(cmd => cmd.Branch, f => f.Company.CompanyName())
            .RuleFor(cmd => cmd.Items, f =>
                Enumerable.Range(0, f.Random.Int(1, 5))
                          .Select(_ => new CreateSaleItemDto
                          {
                              ProductId = f.Random.Guid(),
                              Quantity = f.Random.Int(1, 20),
                              UnitPrice = f.Finance.Amount(1, 100)
                          }).ToList());

        /// <summary>
        /// Generates a valid CreateSaleCommand with randomized data.
        /// </summary>
        /// <returns>A valid CreateSaleCommand</returns>
        public static CreateSaleCommand GenerateValidCommand() => CommandFaker.Generate();

        /// <summary>
        /// Generates a CreateSaleCommand with a specific sale number.
        /// </summary>
        /// <param name="saleNumber">The sale number to use</param>
        /// <returns>A CreateSaleCommand with specified sale number</returns>
        public static CreateSaleCommand GenerateCommandWithNumber(string saleNumber)
        {
            var command = CommandFaker.Generate();
            command.SaleNumber = saleNumber;
            return command;
        }

        /// <summary>
        /// Generates a CreateSaleCommand with duplicate sale number for testing uniqueness.
        /// </summary>
        /// <param name="existingNumber">Existing sale number</param>
        /// <returns>A CreateSaleCommand with duplicate number</returns>
        public static CreateSaleCommand GenerateDuplicateNumberCommand(string existingNumber)
        {
            var command = CommandFaker.Generate();
            command.SaleNumber = existingNumber;
            return command;
        }

        /// <summary>
        /// Generates an invalid CreateSaleCommand with empty values.
        /// </summary>
        /// <returns>An invalid CreateSaleCommand</returns>
        public static CreateSaleCommand GenerateInvalidCommand() => new CreateSaleCommand
        {
            SaleNumber = string.Empty,
            Date = DateTime.UtcNow.AddDays(1), // Future date
            CustomerId = Guid.Empty,
            Branch = string.Empty,
            Items = new List<CreateSaleItemDto>()
        };

        /// <summary>
        /// Generates a CreateSaleCommand with invalid item quantities.
        /// </summary>
        /// <returns>A CreateSaleCommand with invalid quantities</returns>
        public static CreateSaleCommand GenerateCommandWithInvalidQuantities()
        {
            var command = CommandFaker.Generate();
            command.Items = new List<CreateSaleItemDto>
            {
                new CreateSaleItemDto
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 25, // Invalid: > 20
                    UnitPrice = 10.00m
                }
            };
            return command;
        }

        /// <summary>
        /// Generates a CreateSaleCommand with future date.
        /// </summary>
        /// <returns>A CreateSaleCommand with future date</returns>
        public static CreateSaleCommand GenerateCommandWithFutureDate()
        {
            var command = CommandFaker.Generate();
            command.Date = DateTime.UtcNow.AddDays(1);
            return command;
        }

        /// <summary>
        /// Generates a CreateSaleCommand with no items.
        /// </summary>
        /// <returns>A CreateSaleCommand with no items</returns>
        public static CreateSaleCommand GenerateCommandWithNoItems()
        {
            var command = CommandFaker.Generate();
            command.Items = new List<CreateSaleItemDto>();
            return command;
        }

        /// <summary>
        /// Generates CreateSaleItemDto collection for testing.
        /// </summary>
        /// <param name="count">Number of items to generate</param>
        /// <returns>Collection of CreateSaleItemDto</returns>
        public static List<CreateSaleItemDto> GenerateValidItems(int count)
        {
            var faker = new Faker();
            return Enumerable.Range(0, count)
                .Select(_ => new CreateSaleItemDto
                {
                    ProductId = faker.Random.Guid(),
                    Quantity = faker.Random.Int(1, 20),
                    UnitPrice = faker.Finance.Amount(1, 100)
                }).ToList();
        }

        /// <summary>
        /// Generates CreateSaleItemDto with specific quantities for discount testing.
        /// </summary>
        /// <param name="quantities">Array of quantities to use</param>
        /// <returns>Collection of CreateSaleItemDto with specified quantities</returns>
        public static List<CreateSaleItemDto> GenerateItemsWithQuantities(params int[] quantities)
        {
            var faker = new Faker();
            return quantities.Select(qty => new CreateSaleItemDto
            {
                ProductId = faker.Random.Guid(),
                Quantity = qty,
                UnitPrice = faker.Finance.Amount(10, 50)
            }).ToList();
        }

        /// <summary>
        /// Generates a CreateSaleCommand for business rule testing.
        /// </summary>
        /// <returns>A CreateSaleCommand with items for discount testing</returns>
        public static CreateSaleCommand GenerateCommandForBusinessRuleTesting()
        {
            var faker = new Faker();
            return new CreateSaleCommand
            {
                SaleNumber = $"VN-{faker.Random.Number(1000, 9999)}",
                Date = faker.Date.Recent(10),
                CustomerId = faker.Random.Guid(),
                Branch = faker.Company.CompanyName(),
                Items = new List<CreateSaleItemDto>
                {
                    new CreateSaleItemDto { ProductId = faker.Random.Guid(), Quantity = 3, UnitPrice = 10.00m },   // No discount
                    new CreateSaleItemDto { ProductId = faker.Random.Guid(), Quantity = 5, UnitPrice = 20.00m },   // 10% discount
                    new CreateSaleItemDto { ProductId = faker.Random.Guid(), Quantity = 15, UnitPrice = 30.00m },  // 20% discount
                }
            };
        }
    }
}