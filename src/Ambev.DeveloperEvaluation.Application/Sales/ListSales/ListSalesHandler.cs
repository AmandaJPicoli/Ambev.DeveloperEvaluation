using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales
{
    /// <summary>
    /// Handler for processing <see cref="ListSalesQuery"/> requests.
    /// </summary>
    public class ListSalesHandler : IRequestHandler<ListSalesQuery, ListSalesResult>
    {
        private readonly ISaleRepository _repository;

        /// <summary>
        /// Initializes a new instance of <see cref="ListSalesHandler"/>.
        /// </summary>
        /// <param name="repository">The repository for sales.</param>
        public ListSalesHandler(ISaleRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Handles the <see cref="ListSalesQuery"/> to return a paged list of sales.
        /// </summary>
        /// <param name="request">The query containing paging, sorting, and filtering options.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="ListSalesResult"/> containing sales summaries.</returns>
        public async Task<ListSalesResult> Handle(ListSalesQuery request, CancellationToken cancellationToken)
        {
            var queryable = (await _repository.GetAllAsync(cancellationToken)).AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(request.Branch))
                queryable = queryable.Where(s => s.Branch == request.Branch);
            if (request.MinTotal.HasValue)
                queryable = queryable.Where(s => s.TotalAmount >= request.MinTotal.Value);
            if (request.MaxTotal.HasValue)
                queryable = queryable.Where(s => s.TotalAmount <= request.MaxTotal.Value);

            // Sorting
            if (!string.IsNullOrWhiteSpace(request.Order))
            {
                foreach (var part in request.Order.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    var segments = part.Trim().Split(' ');
                    var field = segments[0];
                    var desc = segments.Length > 1 && segments[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

                    if (field.Equals("totalamount", StringComparison.OrdinalIgnoreCase))
                        queryable = desc
                            ? queryable.OrderByDescending(s => s.TotalAmount)
                            : queryable.OrderBy(s => s.TotalAmount);
                    else if (field.Equals("date", StringComparison.OrdinalIgnoreCase))
                        queryable = desc
                            ? queryable.OrderByDescending(s => s.Date)
                            : queryable.OrderBy(s => s.Date);
                }
            }

            // Total count before paging
            var total = queryable.Count();

            // Paging
            var skip = (request.Page - 1) * request.Size;
            var items = queryable
                .Skip(skip)
                .Take(request.Size)
                .Select(s => new SaleSummaryDto
                {
                    Id = s.Id,
                    SaleNumber = s.SaleNumber,
                    Date = s.Date,
                    Branch = s.Branch,
                    TotalAmount = s.TotalAmount
                })
                .ToList();

            return new ListSalesResult
            {
                Page = request.Page,
                Size = request.Size,
                TotalCount = total,
                Sales = items
            };
        }
    }
}
