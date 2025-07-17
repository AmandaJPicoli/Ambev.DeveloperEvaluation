namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    /// <summary>
    /// Response DTO for listing sales.
    /// </summary>
    public class ListSalesResponse
    {
        /// <summary>Current page number.</summary>
        public int Page { get; set; }

        /// <summary>Page size.</summary>
        public int Size { get; set; }

        /// <summary>Total number of sales matching filter.</summary>
        public int TotalCount { get; set; }

        /// <summary>List of sales summaries.</summary>
        public List<SaleSummary> Sales { get; set; } = new();
    }

    /// <summary>
    /// Summary DTO for an individual sale in the list.
    /// </summary>
    public class SaleSummary
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; } = default!;
        public DateTime Date { get; set; }
        public string Branch { get; set; } = default!;
        public decimal TotalAmount { get; set; }
    }
}
