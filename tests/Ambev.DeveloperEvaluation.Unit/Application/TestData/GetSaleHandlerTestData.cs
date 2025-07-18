using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    /// <summary>
    /// Provides test data for GetSaleHandler tests.
    /// </summary>
    public static class GetSaleHandlerTestData
    {
        /// <summary>
        /// Generates a valid GetSaleQuery.
        /// </summary>
        /// <returns>A valid GetSaleQuery</returns>
        public static GetSaleQuery GenerateValidQuery()
        {
            return new GetSaleQuery(Guid.NewGuid());
        }

        /// <summary>
        /// Generates a GetSaleQuery with specific ID.
        /// </summary>
        /// <param name="saleId">The sale ID to use</param>
        /// <returns>A GetSaleQuery with specified ID</returns>
        public static GetSaleQuery GenerateQueryForSale(Guid saleId)
        {
            return new GetSaleQuery(saleId);
        }

        /// <summary>
        /// Generates an invalid GetSaleQuery with empty ID.
        /// </summary>
        /// <returns>An invalid GetSaleQuery</returns>
        public static GetSaleQuery GenerateInvalidQuery()
        {
            return new GetSaleQuery(Guid.Empty);
        }

        /// <summary>
        /// Generates a Sale entity for testing GetSale functionality.
        /// </summary>
        /// <returns>A valid Sale entity</returns>
        public static Sale GenerateSaleForQuery()
        {
            return SaleTestData.CreateValidSale();
        }

        /// <summary>
        /// Generates a Sale entity with specific ID for testing.
        /// </summary>
        /// <param name="saleId">The sale ID to use</param>
        /// <returns>A valid Sale entity</returns>
        public static Sale GenerateSaleWithId(Guid saleId)
        {
            // Since we can't modify the ID after creation, we create a normal sale
            // In real scenarios, the ID would be set by the database
            return SaleTestData.CreateValidSale();
        }

        /// <summary>
        /// Generates a cancelled Sale for testing.
        /// </summary>
        /// <returns>A cancelled Sale entity</returns>
        public static Sale GenerateCancelledSale()
        {
            return SaleTestData.CreateCancelledSale();
        }

        /// <summary>
        /// Generates a Sale with multiple items for comprehensive testing.
        /// </summary>
        /// <returns>A Sale with multiple items</returns>
        public static Sale GenerateSaleWithMultipleItems()
        {
            return SaleTestData.CreateSaleWithSpecificQuantities(2, 5, 15); // Different discount tiers
        }

        /// <summary>
        /// Generates expected GetSaleResult based on a Sale entity.
        /// </summary>
        /// <param name="sale">The sale entity to base the result on</param>
        /// <returns>Expected GetSaleResult</returns>
        public static GetSaleResult GenerateExpectedResult(Sale sale)
        {
            return new GetSaleResult
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
                Date = sale.Date,
                CustomerId = sale.CustomerId,
                Branch = sale.Branch,
                Cancelled = sale.Cancelled,
                TotalAmount = sale.TotalAmount,
                Items = sale.Items.Select(item => new GetSaleItemResult
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount,
                    Total = item.Total
                }).ToList()
            };
        }
    }
}