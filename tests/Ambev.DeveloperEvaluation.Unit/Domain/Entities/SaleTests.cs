using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    /// <summary>
    /// Contains unit tests for the Sale entity class.
    /// Tests cover cancellation, total calculation, and validation scenarios.
    /// </summary>
    public class SaleTests
    {
        [Fact(DisplayName = "Sale total should be correctly calculated based on items and discounts")]
        public void Given_ValidSale_When_CalculatingTotal_Then_TotalAmountIsCorrect()
        {
            // Arrange
            var sale = SaleTestData.CreateValidSale();

            // Act
            var total = sale.TotalAmount;

            // Assert
            Assert.True(total >= 0);
            Assert.Equal(sale.Items.Sum(i => (i.UnitPrice * i.Quantity) - i.Discount), total);
        }

        [Fact(DisplayName = "Sale should start as not cancelled")]
        public void Given_NewSale_When_Created_Then_CancelledIsFalse()
        {
            // Arrange & Act
            var sale = SaleTestData.CreateValidSale();

            // Assert
            Assert.False(sale.Cancelled);
        }

        [Fact(DisplayName = "Sale should be cancellable")]
        public void Given_ValidSale_When_Cancelled_Then_CancelledIsTrue()
        {
            // Arrange
            var sale = SaleTestData.CreateValidSale();

            // Act
            sale.CancelSale();

            // Assert
            Assert.True(sale.Cancelled);
        }

        [Fact(DisplayName = "Validation should throw for invalid quantity")]
        public void Given_InvalidQuantity_When_CreatingSale_Then_ThrowsInvalidOperationException()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(SaleTestData.CreateInvalidQuantitySale);
        }

        [Fact(DisplayName = "Validation should throw for missing sale number")]
        public void Given_EmptySaleNumber_When_CreatingSale_Then_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Sale(
                    string.Empty,
                    DateTime.UtcNow,
                    Guid.NewGuid(),
                    "BranchX",
                    SaleTestData.CreateValidSale().Items
                ));
        }
    }
}
