using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Command to update an existing sale's branch and items following CQRS pattern.
    /// Implements IRequest to enable processing through MediatR pipeline with validation and event handling.
    /// </summary>
    public class UpdateSaleCommand : IRequest<UpdateSaleResult>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale to update.
        /// This ID is used to locate the existing sale entity in the repository.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the new branch identifier for the sale.
        /// This field allows correction of branch information or business process updates.
        /// </summary>
        public string Branch { get; set; } = default!;

        /// <summary>
        /// Gets or sets the collection of updated sale items.
        /// This collection completely replaces the existing items in the sale.
        /// </summary>
        public List<UpdateSaleItemDto> Items { get; set; } = new();
    }

    /// <summary>
    /// DTO representing an item in an update sale command.
    /// Contains the essential information needed to recreate or modify a sale item.
    /// </summary>
    public class UpdateSaleItemDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product being sold.
        /// References the product from external identity following DDD patterns.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product being purchased.
        /// This value determines discount eligibility and pricing calculations.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the product at the time of sale.
        /// This represents the base price before any quantity-based discounts are applied.
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}