using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    /// <summary>
    /// Provides test data for DeleteSaleHandler tests.
    /// </summary>
    public static class DeleteSaleHandlerTestData
    {
        /// <summary>
        /// Generates a valid DeleteSaleCommand with a new GUID.
        /// </summary>
        public static DeleteSaleCommand GenerateValidCommand() => new(Guid.NewGuid());

        /// <summary>
        /// Generates an invalid DeleteSaleCommand with an empty GUID.
        /// </summary>
        public static DeleteSaleCommand GenerateInvalidCommand() => new(Guid.Empty);
    }
}
