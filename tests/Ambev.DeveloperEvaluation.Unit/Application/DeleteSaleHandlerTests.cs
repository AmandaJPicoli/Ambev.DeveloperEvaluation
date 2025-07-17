using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    /// <summary>
    /// Unit tests for <see cref="DeleteSaleHandler"/>
    /// </summary>
    public class DeleteSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly DeleteSaleHandler _handler;

        public DeleteSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _handler = new DeleteSaleHandler(_saleRepository);
        }

        [Fact(DisplayName = "Given valid sale ID When deleting sale Then should return success response")]
        public async Task Handle_ValidRequest_ShouldReturnSuccess()
        {
            // Arrange
            var command = DeleteSaleHandlerTestData.GenerateValidCommand();
            _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>()).Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            await _saleRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given invalid sale ID When deleting sale Then should throw KeyNotFoundException")]
        public async Task Handle_NonExistingSale_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var command = DeleteSaleHandlerTestData.GenerateValidCommand();
            _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>()).Returns(false);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Sale with ID {command.Id} not found");
        }

        [Fact(DisplayName = "Given empty ID When deleting sale Then should throw validation exception")]
        public async Task Handle_EmptyId_ShouldThrowValidationException()
        {
            // Arrange
            var command = DeleteSaleHandlerTestData.GenerateInvalidCommand();

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>()
                .WithMessage("*Sale ID is required*");
        }
    }
}
