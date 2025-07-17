namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    /// <summary>
    /// Request DTO for listing sales with pagination, sorting, and filtering.
    /// </summary>
    public class ListSalesRequest
    {
        /// <summary>Page number (1-based).</summary>
        public int Page { get; set; } = 1;

        /// <summary>Number of items per page.</summary>
        public int Size { get; set; } = 10;

        /// <summary>Sort expression, e.g. "TotalAmount desc, Date asc".</summary>
        public string? Order { get; set; }

        /// <summary>Filter by branch.</summary>
        public string? Branch { get; set; }

        /// <summary>Minimum total amount.</summary>
        public decimal? MinTotal { get; set; }

        /// <summary>Maximum total amount.</summary>
        public decimal? MaxTotal { get; set; }
    }
}
