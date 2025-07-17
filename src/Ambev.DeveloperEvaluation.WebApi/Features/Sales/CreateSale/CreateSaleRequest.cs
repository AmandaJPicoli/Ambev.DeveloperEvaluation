using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    /// <summary>
    /// Request DTO for creating a new sale via API.
    /// </summary>
    public class CreateSaleRequest
    {
        /// <summary>
        /// Unique sale number.
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>
        /// Date and time of the sale.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Identifier of the customer.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Branch where the sale took place.
        /// </summary>
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Collection of items in the sale.
        /// </summary>
        public List<CreateSaleItemDto> Items { get; set; } = new();
    }

    /// <summary>
    /// Item DTO nested in CreateSaleRequest.
    /// </summary>
    public class CreateSaleItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
