using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    /// <summary>
    /// Event triggered when a sale is modified.
    /// </summary>
    public class SaleModifiedEvent : IDomainEvent
    {
        public Sale Sale { get; }
        public DateTime OccurredAt { get; }

        public SaleModifiedEvent(Sale sale)
        {
            Sale = sale;
            OccurredAt = DateTime.UtcNow;
        }
    }
}
