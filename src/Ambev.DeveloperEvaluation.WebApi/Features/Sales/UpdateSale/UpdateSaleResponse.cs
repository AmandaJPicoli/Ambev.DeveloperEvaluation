namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    /// <summary>
    /// Response DTO returned after updating a sale.
    /// </summary>
    public class UpdateSaleResponse
    {
        /// <summary>
        /// Identifier of the updated sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// New total amount of the sale.
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
