using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Handler for processing UpdateSaleCommand requests.
    /// </summary>
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IDomainEventDispatcher _eventDispatcher;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateSaleHandler> _logger;

        /// <summary>
        /// Initializes a new instance of UpdateSaleHandler.
        /// </summary>
        /// <param name="saleRepository">The sale repository</param>
        /// <param name="eventDispatcher">The domain event dispatcher</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="logger">The logger instance</param>
        public UpdateSaleHandler(
            ISaleRepository saleRepository,
            IDomainEventDispatcher eventDispatcher,
            IMapper mapper,
            ILogger<UpdateSaleHandler> logger)
        {
            _saleRepository = saleRepository;
            _eventDispatcher = eventDispatcher;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Handles the UpdateSaleCommand request.
        /// </summary>
        /// <param name="command">The update sale command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The update sale result</returns>
        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing UpdateSaleCommand for Sale ID: {SaleId}", command.Id);

            var validator = new UpdateSaleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("UpdateSaleCommand validation failed for Sale ID: {SaleId}", command.Id);
                throw new ValidationException(validationResult.Errors);
            }

            var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (sale == null)
            {
                _logger.LogWarning("Sale not found for update. Sale ID: {SaleId}", command.Id);
                throw new KeyNotFoundException($"Sale with ID {command.Id} not found");
            }

            try
            {
                sale.UpdateBranch(command.Branch);
                sale.ReplaceItems(command.Items.Select(i => new SaleItem(i.ProductId, i.Quantity, i.UnitPrice)));

                await _saleRepository.UpdateAsync(sale, cancellationToken);
                _logger.LogInformation("Successfully updated sale. Sale ID: {SaleId}", command.Id);

                var modifiedEvent = new SaleModifiedEvent(sale);
                await _eventDispatcher.DispatchAsync(modifiedEvent, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating sale. Sale ID: {SaleId}", command.Id);
                throw;
            }

            return new UpdateSaleResult
            {
                Id = sale.Id,
                TotalAmount = sale.TotalAmount
            };
        }
    }
}