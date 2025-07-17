using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Handler for processing <see cref="CreateSaleCommand"/> requests.
    /// </summary>
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IDomainEventDispatcher _eventDispatcher;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of <see cref="CreateSaleHandler"/>.
        /// </summary>
        /// <param name="saleRepository">Repository for sale aggregates.</param>
        /// <param name="mapper">AutoMapper instance.</param>
        public CreateSaleHandler(ISaleRepository saleRepository,
            IDomainEventDispatcher eventDispatcher,
            IMapper mapper)
        {
            _saleRepository = saleRepository;
            _eventDispatcher = eventDispatcher;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the <see cref="CreateSaleCommand"/> to create a new sale.
        /// </summary>
        /// <param name="command">The create sale command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="CreateSaleResult"/> with details of the created sale.</returns>
        /// <exception cref="ValidationException">Thrown when the command fails validation.</exception>
        /// <exception cref="InvalidOperationException">Thrown when a sale with the same number already exists.</exception>
        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            // Validate the command
            var validator = new CreateSaleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Check for duplicate sale number
            if (await _saleRepository.ExistsAsync(command.SaleNumber, cancellationToken))
                throw new InvalidOperationException($"Sale with number {command.SaleNumber} already exists.");

            // Map to domain entity
            var sale = _mapper.Map<Sale>(command);

            // Persist the new sale
            sale = await _saleRepository.CreateAsync(sale, cancellationToken);

            // Dispatch Domain Event
            var createdEvent = new SaleCreatedEvent(sale);
            await _eventDispatcher.DispatchAsync(createdEvent, cancellationToken);

            // Map to result DTO and return
            return _mapper.Map<CreateSaleResult>(sale);
        }
    }
}
