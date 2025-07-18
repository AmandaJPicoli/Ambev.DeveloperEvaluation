using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    /// <summary>
    /// Contains unit tests for the SaleItem entity class.
    /// </summary>
    public class SaleItemTests
    {
        [Theory(DisplayName = "SaleItem should calculate discount based on quantity")]
        [InlineData(1, 10.00, 0.00)]      // No discount for quantity < 4
        [InlineData(3, 10.00, 0.00)]      // No discount for quantity < 4
        [InlineData(4, 10.00, 4.00)]      // 10% discount for quantity 4-9
        [InlineData(9, 10.00, 9.00)]      // 10% discount for quantity 4-9
        [InlineData(10, 10.00, 20.00)]    // 20% discount for quantity 10-20
        [InlineData(20, 10.00, 40.00)]    // 20% discount for quantity 10-20
        public void Constructor_WithValidQuantity_ShouldCalculateCorrectDiscount(int quantity, decimal unitPrice, decimal expectedDiscount)
        {
            // Arrange & Act
            var item = new SaleItem(Guid.NewGuid(), quantity, unitPrice);

            // Assert
            item.Discount.Should().Be(expectedDiscount);
        }

        [Theory(DisplayName = "SaleItem should calculate total correctly")]
        [InlineData(1, 10.00, 10.00)]     // (1 * 10.00) - 0.00[InlineData(3, 10.00, 30.00)]     // (3 * 10.00) - 0.00
        [InlineData(4, 10.00, 36.00)]     // (4 * 10.00) - 4.00 
        [InlineData(5, 15.00, 67.50)]     // (5 * 15.00) - 7.50
        [InlineData(10, 20.00, 160.00)]   // (10 * 20.00) - 40.00
        [InlineData(15, 10.00, 120.00)]   // (15 * 10.00) - 30.00
        public void Total_WithValidData_ShouldCalculateCorrectly(int quantity, decimal unitPrice, decimal expectedTotal)
        {
            // Arrange & Act
            var item = new SaleItem(Guid.NewGuid(), quantity, unitPrice);

            // Assert
            item.Total.Should().Be(expectedTotal);
        }

        [Fact(DisplayName = "Constructor should throw when product ID is empty")]
        public void Constructor_WithEmptyProductId_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            var action = () => new SaleItem(Guid.Empty, 5, 10.00m);

            action.Should().Throw<ArgumentException>()
                .WithMessage("*Product ID is required*");
        }

        [Theory(DisplayName = "Constructor should throw when quantity is invalid")]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-5)]
        public void Constructor_WithInvalidQuantity_ShouldThrowArgumentOutOfRangeException(int quantity)
        {
            // Arrange & Act & Assert
            var action = () => new SaleItem(Guid.NewGuid(), quantity, 10.00m);

            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*Quantity must be greater than 0*");
        }

        [Fact(DisplayName = "Constructor should throw when quantity exceeds maximum")]
        public void Constructor_WithQuantityAboveMaximum_ShouldThrowInvalidOperationException()
        {
            // Arrange & Act & Assert
            var action = () => new SaleItem(Guid.NewGuid(), 21, 10.00m);

            action.Should().Throw<InvalidOperationException>()
                .WithMessage("*Cannot sell more than 20 units of a product*");
        }

        [Theory(DisplayName = "Constructor should throw when unit price is invalid")]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10.50)]
        public void Constructor_WithInvalidUnitPrice_ShouldThrowArgumentOutOfRangeException(decimal unitPrice)
        {
            // Arrange & Act & Assert
            var action = () => new SaleItem(Guid.NewGuid(), 5, unitPrice);

            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*Unit price must be greater than 0*");
        }

        [Fact(DisplayName = "Constructor should set properties correctly")]
        public void Constructor_WithValidData_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var quantity = 5;
            var unitPrice = 15.50m;

            // Act
            var item = new SaleItem(productId, quantity, unitPrice);

            // Assert
            item.ProductId.Should().Be(productId);
            item.Quantity.Should().Be(quantity);
            item.UnitPrice.Should().Be(unitPrice);
        }
    }
}