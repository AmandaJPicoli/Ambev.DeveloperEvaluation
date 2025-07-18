using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    /// <summary>
    /// Event triggered when an item is cancelled.
    /// </summary>
    public class ItemCancelledEvent : IDomainEvent
    {
        public Guid SaleId { get; }
        public Guid ProductId { get; }
        public int Quantity { get; }
        public DateTime OccurredAt { get; }

        public ItemCancelledEvent(Guid saleId, Guid productId, int quantity)
        {
            SaleId = saleId;
            ProductId = productId;
            Quantity = quantity;
            OccurredAt = DateTime.UtcNow;
        }
    }
}
