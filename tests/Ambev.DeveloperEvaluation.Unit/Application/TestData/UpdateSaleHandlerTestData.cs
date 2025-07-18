using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    /// <summary>
    /// Provides test data for UpdateSaleHandler tests.
    /// </summary>
    public static class UpdateSaleHandlerTestData
    {
        /// <summary>
        /// Generates a valid UpdateSaleCommand and corresponding existing Sale with matching ID.
        /// </summary>
        public static (UpdateSaleCommand Command, Sale ExistingSale) GenerateValidCommandWithExistingSale()
        {
            var saleId = Guid.NewGuid();

            var command = new Faker<UpdateSaleCommand>()
                .RuleFor(cmd => cmd.Id, _ => saleId) // Usar o mesmo ID
                .RuleFor(cmd => cmd.Branch, f => f.Company.CompanyName())
                .RuleFor(cmd => cmd.Items, f =>
                    Enumerable.Range(0, f.Random.Int(1, 3))
                              .Select(_ => new UpdateSaleItemDto
                              {
                                  ProductId = f.Random.Guid(),
                                  Quantity = f.Random.Int(1, 20),
                                  UnitPrice = f.Finance.Amount(1, 100)
                              }).ToList())
                .Generate();

            var existingSale = new Faker<Sale>()
                .CustomInstantiator(f => new Sale(
                    $"VN-{f.Random.Number(1000, 9999)}",
                    f.Date.Recent(30),
                    f.Random.Guid(),
                    f.Company.CompanyName(),
                    new[]
                    {
                new SaleItem(f.Random.Guid(), f.Random.Int(1, 10), f.Finance.Amount(1, 50))
                    }))
                .Generate();

            // Definir o mesmo ID para a sale existente
            SetSaleId(existingSale, saleId);

            return (command, existingSale);
        }

        private static void SetSaleId(Sale sale, Guid id)
        {
            // Use reflection para definir o ID se não houver setter público
            var idProperty = typeof(Sale).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
            if (idProperty?.CanWrite == true)
            {
                idProperty.SetValue(sale, id);
            }
            else
            {
                // Se for campo privado, use reflection para acessá-lo
                var idField = typeof(Sale).GetField("_id", BindingFlags.NonPublic | BindingFlags.Instance);
                idField?.SetValue(sale, id);
            }
        }

        private static readonly Faker<UpdateSaleCommand> CommandFaker = new Faker<UpdateSaleCommand>()
            .RuleFor(cmd => cmd.Id, f => f.Random.Guid())
            .RuleFor(cmd => cmd.Branch, f => f.Company.CompanyName())
            .RuleFor(cmd => cmd.Items, f =>
                Enumerable.Range(0, f.Random.Int(1, 3))
                          .Select(_ => new UpdateSaleItemDto
                          {
                              ProductId = f.Random.Guid(),
                              Quantity = f.Random.Int(1, 20),
                              UnitPrice = f.Finance.Amount(1, 100)
                          }).ToList());

        private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
            .CustomInstantiator(f => new Sale(
                $"VN-{f.Random.Number(1000, 9999)}",
                f.Date.Recent(30),
                f.Random.Guid(),
                f.Company.CompanyName(),
                new[]
                {
                    new SaleItem(f.Random.Guid(), f.Random.Int(1, 10), f.Finance.Amount(1, 50))
                }));

        /// <summary>
        /// Generates a valid UpdateSaleCommand.
        /// </summary>
        public static UpdateSaleCommand GenerateValidCommand() => CommandFaker.Generate();

        /// <summary>
        /// Generates an existing Sale for update tests.
        /// </summary>
        public static Sale GenerateExistingSale() => SaleFaker.Generate();

        /// <summary>
        /// Generates an invalid UpdateSaleCommand.
        /// </summary>
        public static UpdateSaleCommand GenerateInvalidCommand() => new UpdateSaleCommand
        {
            Id = Guid.Empty,
            Branch = "",
            Items = new List<UpdateSaleItemDto>()
        };
    }
}