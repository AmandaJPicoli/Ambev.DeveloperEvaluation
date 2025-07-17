namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    /// <summary>
    /// Request DTO for updating an existing sale.
    /// </summary>
    public class UpdateSaleRequest
    {
        /// <summary>
        /// Identifier of the sale to update.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// New branch for the sale.
        /// </summary>
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Updated collection of sale items.
        /// </summary>
        public List<UpdateSaleItemDto> Items { get; set; } = new();
    }

    /// <summary>
    /// DTO representing an item in the update sale request.
    /// </summary>
    public class UpdateSaleItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
