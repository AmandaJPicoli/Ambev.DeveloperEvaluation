using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    /// <summary>
    /// Provides methods for generating test data for CreateSaleHandler using Bogus.
    /// </summary>
    public static class CreateSaleHandlerTestData
    {
        private static readonly Faker<CreateSaleCommand> Faker = new Faker<CreateSaleCommand>()
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
        public static CreateSaleCommand GenerateValidCommand() => Faker.Generate();

        /// <summary>
        /// Generates a command with a duplicate SaleNumber to test uniqueness constraint.
        /// </summary>
        public static CreateSaleCommand GenerateDuplicateNumberCommand(string existingNumber)
        {
            var cmd = Faker.Generate();
            cmd.SaleNumber = existingNumber;
            return cmd;
        }
    }
}
