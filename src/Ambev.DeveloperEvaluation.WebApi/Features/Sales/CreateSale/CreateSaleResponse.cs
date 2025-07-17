namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    /// <summary>
    /// Response DTO returned after creating a sale.
    /// </summary>
    public class CreateSaleResponse
    {
        /// <summary>
        /// Newly created sale identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Total amount of the sale.
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
