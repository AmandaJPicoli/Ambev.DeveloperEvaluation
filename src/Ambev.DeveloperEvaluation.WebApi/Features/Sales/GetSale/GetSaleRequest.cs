namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// Request DTO for retrieving a sale by its ID via API.
    /// </summary>
    public class GetSaleRequest
    {
        /// <summary>
        /// The unique identifier of the sale to retrieve.
        /// </summary>
        public Guid Id { get; set; }
    }
}
