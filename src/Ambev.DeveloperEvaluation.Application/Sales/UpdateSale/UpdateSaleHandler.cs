using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of <see cref="CreateSaleHandler"/>.
        /// </summary>
        /// <param name="saleRepository">The sale repository.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the <see cref="CreateSaleCommand"/> request.
        /// </summary>
        /// <param name="command">The create sale command.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The created sale result.</returns>
        /// <exception cref="ValidationException">Thrown when the command is invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when a sale with the same number already exists.</exception>
        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            // Validate command
            var validator = new CreateSaleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Check uniqueness
            if (await _saleRepository.ExistsAsync(command.SaleNumber))
                throw new InvalidOperationException($"Sale with number {command.SaleNumber} already exists.");

            // Map to domain entity
            var sale = _mapper.Map<Sale>(command);

            // Persist
            await _saleRepository.UpdateAsync(sale);

            // Map to result DTO
            return _mapper.Map<CreateSaleResult>(sale);
        }
    }
}
