using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Handles the <see cref="GetSaleQuery"/> and returns <see cref="GetSaleResult"/>.
    /// </summary>
    public class GetSaleHandler : IRequestHandler<GetSaleQuery, GetSaleResult>
    {
        private readonly ISaleRepository _repository;
        private readonly ILogger<GetSaleHandler> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="GetSaleHandler"/>.
        /// </summary>
        /// <param name="repository">Repository for sale aggregates.</param>
        /// <param name="logger">Logger to record handler events.</param>
        public GetSaleHandler(ISaleRepository repository, ILogger<GetSaleHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Handles the query to retrieve a sale by its identifier.
        /// </summary>
        /// <param name="request">The get sale query containing the sale ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="GetSaleResult"/> DTO with sale details.</returns>
        public async Task<GetSaleResult> Handle(GetSaleQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetSaleValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
           
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var sale = await _repository.GetByIdAsync(request.Id);
            if (sale is null)
            {
                _logger.LogWarning("Sale with ID {SaleId} not found.", request.Id);
                throw new InvalidOperationException($"Sale with ID {request.Id} was not found.");
            }

            return new GetSaleResult
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
                Date = sale.Date,
                CustomerId = sale.CustomerId,
                Branch = sale.Branch,
                Cancelled = sale.Cancelled,
                TotalAmount = sale.TotalAmount,
                Items = sale.Items.Select(item => new GetSaleItemResult
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount,
                    Total = item.Total
                }).ToList()
            };
        }
    }
}
