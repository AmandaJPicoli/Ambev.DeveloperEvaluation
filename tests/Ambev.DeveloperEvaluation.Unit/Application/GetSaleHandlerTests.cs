using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    /// <summary>
    /// Unit tests for GetSaleHandler.
    /// </summary>
    public class GetSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ILogger<GetSaleHandler> _logger;
        private readonly GetSaleHandler _handler;

        public GetSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _logger = Substitute.For<ILogger<GetSaleHandler>>();
            _handler = new GetSaleHandler(_saleRepository, _logger);
        }

        [Fact(DisplayName = "Given valid sale ID When getting sale Then returns sale details")]
        public async Task Handle_ValidRequest_ReturnsSaleDetails()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var query = new GetSaleQuery(saleId);
            var sale = SaleTestData.CreateValidSale();

            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                .Returns(sale);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(sale.Id);
            result.SaleNumber.Should().Be(sale.SaleNumber);
            result.Items.Should().HaveCount(sale.Items.Count);
        }

        [Fact(DisplayName = "Given non-existent sale ID When getting sale Then throws InvalidOperationException")]
        public async Task Handle_NonExistentSale_ThrowsInvalidOperationException()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var query = new GetSaleQuery(saleId);

            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                .Returns((Sale?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(query, CancellationToken.None));

            exception.Message.Should().Contain($"Sale with ID {saleId} was not found");
        }

        [Fact(DisplayName = "Given empty ID When getting sale Then throws validation exception")]
        public async Task Handle_EmptyId_ThrowsValidationException()
        {
            // Arrange
            var query = new GetSaleQuery(Guid.Empty);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}