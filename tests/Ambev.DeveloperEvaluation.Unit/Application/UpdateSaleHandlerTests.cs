using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    /// <summary>
    /// Contains unit tests for the UpdateSaleHandler class.
    /// </summary>
    public class UpdateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IDomainEventDispatcher _eventDispatcher;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateSaleHandler> _logger;
        private readonly UpdateSaleHandler _handler;

        public UpdateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _eventDispatcher = Substitute.For<IDomainEventDispatcher>();
            _mapper = Substitute.For<IMapper>();
            _logger = Substitute.For<ILogger<UpdateSaleHandler>>();
            _handler = new UpdateSaleHandler(_saleRepository, _eventDispatcher, _mapper, _logger);
        }

        [Fact(DisplayName = "Given valid update command When updating sale Then returns updated result")]
        public async Task Handle_ValidRequest_ReturnsUpdatedResult()
        {
            // Arrange
            var (command, existingSale) = UpdateSaleHandlerTestData.GenerateValidCommandWithExistingSale();

            _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns(existingSale);
            _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                .Returns(existingSale);

            // ✅ Configurar o AutoMapper mock
            var expectedResult = new UpdateSaleResult { Id = command.Id };
            _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>()).Returns(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(command.Id);
            await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
            await _eventDispatcher.Received(1).DispatchAsync(Arg.Any<SaleModifiedEvent>(), Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given invalid command When updating sale Then throws validation exception")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var command = new UpdateSaleCommand(); // Empty command

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact(DisplayName = "Given non-existent sale When updating Then throws KeyNotFoundException")]
        public async Task Handle_NonExistentSale_ThrowsKeyNotFoundException()
        {
            // Arrange
            var command = UpdateSaleHandlerTestData.GenerateValidCommand();
            _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns((Sale?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None));

            exception.Message.Should().Contain($"Sale with ID {command.Id} not found");
        }
    }
}