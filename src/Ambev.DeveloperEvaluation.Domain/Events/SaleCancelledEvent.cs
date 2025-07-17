using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public sealed class SaleCancelledEvent : IDomainEvent
    {
        public Guid SaleId { get; }
        public DateTime OccurredAt { get; }

        public SaleCancelledEvent(Guid saleId)
        {
            SaleId = saleId;
            OccurredAt = DateTime.UtcNow;
        }
    }
}
