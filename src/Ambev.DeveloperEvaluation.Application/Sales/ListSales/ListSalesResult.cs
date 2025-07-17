using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales
{
    /// <summary>
    /// Result DTO returned when listing sales with pagination, sorting, and filtering.
    /// </summary>
    public class ListSalesResult
    {
        /// <summary>
        /// The current page number (1-based).
        /// </summary>
        public int Page { get; init; }

        /// <summary>
        /// The size of each page (number of items per page).
        /// </summary>
        public int Size { get; init; }

        /// <summary>
        /// The total count of sales matching the query.
        /// </summary>
        public int TotalCount { get; init; }

        /// <summary>
        /// The list of sales summaries for the current page.
        /// </summary>
        public List<SaleSummaryDto> Sales { get; init; } = new();
    }

    /// <summary>
    /// DTO representing a summary of a sale for listing purposes.
    /// </summary>
    public class SaleSummaryDto
    {
        /// <summary>
        /// The unique identifier of the sale.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// The sale number.
        /// </summary>
        public string SaleNumber { get; init; } = default!;

        /// <summary>
        /// The date and time when the sale occurred.
        /// </summary>
        public DateTime Date { get; init; }

        /// <summary>
        /// The branch where the sale was made.
        /// </summary>
        public string Branch { get; init; } = default!;

        /// <summary>
        /// The total amount of the sale.
        /// </summary>
        public decimal TotalAmount { get; init; }
    }
}
