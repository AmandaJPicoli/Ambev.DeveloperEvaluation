using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// Command for retrieving a sale by its ID.
    /// </summary>
    public class GetSaleQuery : IRequest<GetSaleResult>
    {
        /// <summary>
        /// The unique identifier of the sale to retrieve.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="GetSaleQuery"/>.
        /// </summary>
        /// <param name="id">The ID of the sale to retrieve.</param>
        public GetSaleQuery(Guid id)
        {
            Id = id;
        }
    }
}
