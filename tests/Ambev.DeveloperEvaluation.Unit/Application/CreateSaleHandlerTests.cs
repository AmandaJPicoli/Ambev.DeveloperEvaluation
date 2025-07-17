using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    /// <summary>
    /// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
    /// </summary>
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly CreateSaleHandler _handler;
        private readonly IDomainEventDispatcher _dispatcher;

        public CreateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _dispatcher = Substitute.For<IDomainEventDispatcher>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateSaleHandler(
               _saleRepository,
               _dispatcher,
               _mapper
           );
        }

        [Fact(DisplayName = "Given valid sale data When creating sale Then returns result and dispatches event")]
        public async Task Handle_ValidRequest_DispatchesEvent()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidCommand();
            var sale = new Sale(
                command.SaleNumber,
                command.Date,
                command.CustomerId,
                command.Branch,
                command.Items.Select(i => new SaleItem(i.ProductId, i.Quantity, i.UnitPrice))
            );
            var expectedResult = new CreateSaleResult(sale.Id, sale.TotalAmount);

            _saleRepository
                .ExistsAsync(command.SaleNumber, Arg.Any<CancellationToken>())
                .Returns(false);

            _mapper.Map<Sale>(command).Returns(sale);
            _saleRepository
                .CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                .Returns(sale);
            _mapper.Map<CreateSaleResult>(sale).Returns(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert: result mapping
            result.Should().BeEquivalentTo(expectedResult);

            // Assert: sale persisted
            await _saleRepository.Received(1)
                .CreateAsync(Arg.Is<Sale>(s => s.SaleNumber == command.SaleNumber),
                             Arg.Any<CancellationToken>());

            // Assert: domain event dispatched
            await _dispatcher.Received(1)
                .DispatchAsync(Arg.Is<IDomainEvent>(e => e is SaleCreatedEvent),
                               Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange: missing required fields
            var command = new CreateSaleCommand();

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact(DisplayName = "Given duplicate sale number When creating sale Then throws invalid operation exception")]
        public async Task Handle_DuplicateSaleNumber_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidCommand();
            _saleRepository
                .ExistsAsync(command.SaleNumber, Arg.Any<CancellationToken>())
                .Returns(true);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should()
                     .ThrowAsync<InvalidOperationException>()
                     .WithMessage($"Sale with number {command.SaleNumber} already exists.");
        }
    }
}
