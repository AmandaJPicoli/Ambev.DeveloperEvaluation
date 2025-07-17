using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public sealed class ItemCancelledEvent : IDomainEvent
    {
        public Guid SaleId { get; }
        public Guid ItemId { get; }
        public DateTime OccurredAt { get; }

        public ItemCancelledEvent(Guid saleId, Guid itemId)
        {
            SaleId = saleId;
            ItemId = itemId;
            OccurredAt = DateTime.UtcNow;
        }
    }
}
