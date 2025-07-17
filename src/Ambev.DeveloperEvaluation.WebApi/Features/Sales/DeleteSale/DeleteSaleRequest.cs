namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale
{
    /// <summary>
    /// Request DTO for cancelling (deleting) a sale.
    /// </summary>
    public class DeleteSaleRequest
    {
        /// <summary>
        /// Identifier of the sale to cancel.
        /// </summary>
        public Guid Id { get; set; }
    }
}
