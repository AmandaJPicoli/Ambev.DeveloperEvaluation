using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Command to update an existing sale's branch and items.
    /// </summary>
    public class UpdateSaleCommand : IRequest
    {
        /// <summary>
        /// Identifier of the sale to update.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// New branch for the sale.
        /// </summary>
        public string Branch { get; set; } = default!;

        /// <summary>
        /// Collection of updated sale items.
        /// </summary>
        public List<UpdateSaleItemDto> Items { get; set; } = new();
    }

    /// <summary>
    /// DTO representing an item in an update sale command.
    /// </summary>
    public class UpdateSaleItemDto
    {
        /// <summary>
        /// Product identifier.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Quantity of the product.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}
