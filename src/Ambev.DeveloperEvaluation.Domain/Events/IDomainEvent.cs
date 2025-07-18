using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    /// <summary>
    /// Base interface for domain events.
    /// </summary>
    public interface IDomainEvent
    {
        DateTime OccurredAt { get; }
    }
}
