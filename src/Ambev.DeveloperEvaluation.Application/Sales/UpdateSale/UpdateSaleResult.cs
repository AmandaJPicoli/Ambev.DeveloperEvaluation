namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Represents the result returned after successfully updating a sale.
    /// </summary>
    public class UpdateSaleResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the updated sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the recalculated total amount of the sale.
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}