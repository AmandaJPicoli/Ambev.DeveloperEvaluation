using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Logging
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly ILogger<DomainEventDispatcher> _logger;

        public DomainEventDispatcher(ILogger<DomainEventDispatcher> logger)
        {
            _logger = logger;
        }

        public Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain event occurred: {EventType} at {OccurredAt}",
                domainEvent.GetType().Name, domainEvent.OccurredAt);
            return Task.CompletedTask;
        }
    }
}
