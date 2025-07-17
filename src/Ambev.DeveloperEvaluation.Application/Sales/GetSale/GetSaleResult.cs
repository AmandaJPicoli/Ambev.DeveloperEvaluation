namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// Result DTO returned when querying a sale by its identifier.
    /// Contains all sale information including items and totals.
    /// </summary>
    public class GetSaleResult
    {
        /// <summary>
        /// Identifier of the sale.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Unique sale number.
        /// </summary>
        public string SaleNumber { get; init; } = default!;

        /// <summary>
        /// Date of the sale.
        /// </summary>
        public DateTime Date { get; init; }

        /// <summary>
        /// Customer identifier for this sale.
        /// </summary>
        public Guid CustomerId { get; init; }

        /// <summary>
        /// Branch where the sale was made.
        /// </summary>
        public string Branch { get; init; } = default!;

        /// <summary>
        /// Indicates whether the sale has been cancelled.
        /// </summary>
        public bool Cancelled { get; init; }

        /// <summary>
        /// Total amount of the sale including discounts.
        /// </summary>
        public decimal TotalAmount { get; init; }

        /// <summary>
        /// Collection of items included in the sale.
        /// </summary>
        public List<GetSaleItemResult> Items { get; init; } = new();
    }

    /// <summary>
    /// DTO representing an individual item within the sale result.
    /// </summary>
    public class GetSaleItemResult
    {
        /// <summary>
        /// Identifier of the product.
        /// </summary>
        public Guid ProductId { get; init; }

        /// <summary>
        /// Quantity sold.
        /// </summary>
        public int Quantity { get; init; }

        /// <summary>
        /// Unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; init; }

        /// <summary>
        /// Discount applied to this item.
        /// </summary>
        public decimal Discount { get; init; }

        /// <summary>
        /// Total price for this item after discount.
        /// </summary>
        public decimal Total { get; init; }
    }
}
