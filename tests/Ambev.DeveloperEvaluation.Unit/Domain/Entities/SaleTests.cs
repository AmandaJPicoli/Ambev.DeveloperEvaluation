using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    /// <summary>
    /// Contains unit tests for the Sale entity class.
    /// </summary>
    public class SaleTests
    {
        [Fact(DisplayName = "Sale total should be correctly calculated based on items and discounts")]
        public void TotalAmount_WithValidItems_CalculatesCorrectly()
        {
            // Arrange
            var sale = SaleTestData.CreateValidSale();

            // Act
            var total = sale.TotalAmount;

            // Assert
            total.Should().BeGreaterOrEqualTo(0);
            total.Should().Be(sale.Items.Sum(i => i.Total));
        }

        [Fact(DisplayName = "Sale should start as not cancelled")]
        public void NewSale_WhenCreated_ShouldNotBeCancelled()
        {
            // Arrange & Act
            var sale = SaleTestData.CreateValidSale();

            // Assert
            sale.Cancelled.Should().BeFalse();
        }

        [Fact(DisplayName = "Sale should be cancellable")]
        public void CancelSale_WhenCalled_ShouldMarkAsCancelled()
        {
            // Arrange
            var sale = SaleTestData.CreateValidSale();

            // Act
            sale.CancelSale();

            // Assert
            sale.Cancelled.Should().BeTrue();
            sale.UpdatedAt.Should().NotBeNull();
        }

        [Fact(DisplayName = "UpdateBranch should update branch and timestamp")]
        public void UpdateBranch_WithValidBranch_ShouldUpdateBranchAndTimestamp()
        {
            // Arrange
            var sale = SaleTestData.CreateValidSale();
            var newBranch = "New Branch";
            var originalUpdatedAt = sale.UpdatedAt;

            // Act
            sale.UpdateBranch(newBranch);

            // Assert
            sale.Branch.Should().Be(newBranch);
            sale.UpdatedAt.Should().NotBe(originalUpdatedAt);
            sale.UpdatedAt.Should().NotBeNull();
        }

        [Fact(DisplayName = "ReplaceItems should replace all items and update timestamp")]
        public void ReplaceItems_WithValidItems_ShouldReplaceItemsAndUpdateTimestamp()
        {
            // Arrange
            var sale = SaleTestData.CreateValidSale();
            var originalItems = sale.Items.ToList(); // ✅ Capturar itens originais
            var originalUpdatedAt = sale.UpdatedAt;

            var newItems = new[]
            {
                new SaleItem(Guid.NewGuid(), 5, 10.00m),
                new SaleItem(Guid.NewGuid(), 3, 15.00m)
            };

            // Act
            sale.ReplaceItems(newItems);

            // Assert
            sale.Items.Should().HaveCount(2);

            // ✅ Verificar que os itens foram realmente substituídos
            sale.Items.Should().NotBeEquivalentTo(originalItems);
            sale.Items.Should().BeEquivalentTo(newItems);

            sale.UpdatedAt.Should().NotBe(originalUpdatedAt);
            sale.UpdatedAt.Should().NotBeNull();
        }

        [Fact(DisplayName = "Constructor should throw when sale number is empty")]
        public void Constructor_WithEmptySaleNumber_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            var action = () => new Sale(
                string.Empty,
                DateTime.UtcNow,
                Guid.NewGuid(),
                "Branch",
                new[] { new SaleItem(Guid.NewGuid(), 1, 10.00m) }
            );

            action.Should().Throw<ArgumentException>()
                .WithMessage("*Sale number cannot be null or empty*");
        }

        [Fact(DisplayName = "Constructor should throw when customer ID is empty")]
        public void Constructor_WithEmptyCustomerId_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            var action = () => new Sale(
                "VN-001",
                DateTime.UtcNow,
                Guid.Empty,
                "Branch",
                new[] { new SaleItem(Guid.NewGuid(), 1, 10.00m) }
            );

            action.Should().Throw<ArgumentException>()
                .WithMessage("*Customer ID is required*");
        }

        [Fact(DisplayName = "Constructor should throw when branch is empty")]
        public void Constructor_WithEmptyBranch_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            var action = () => new Sale(
                "VN-001",
                DateTime.UtcNow,
                Guid.NewGuid(),
                string.Empty,
                new[] { new SaleItem(Guid.NewGuid(), 1, 10.00m) }
            );

            action.Should().Throw<ArgumentException>()
                .WithMessage("*Branch cannot be null or empty*");
        }

        [Fact(DisplayName = "Constructor should throw when items collection is empty")]
        public void Constructor_WithEmptyItems_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            var action = () => new Sale(
                "VN-001",
                DateTime.UtcNow,
                Guid.NewGuid(),
                "Branch",
                new SaleItem[0]
            );

            action.Should().Throw<ArgumentException>()
                .WithMessage("*At least one sale item is required*");
        }

        [Fact(DisplayName = "UpdateBranch should throw when branch is empty")]
        public void UpdateBranch_WithEmptyBranch_ShouldThrowArgumentException()
        {
            // Arrange
            var sale = SaleTestData.CreateValidSale();

            // Act & Assert
            var action = () => sale.UpdateBranch(string.Empty);

            action.Should().Throw<ArgumentException>()
                .WithMessage("*Branch cannot be null or empty*");
        }

        [Fact(DisplayName = "ReplaceItems should throw when items collection is empty")]
        public void ReplaceItems_WithEmptyItems_ShouldThrowArgumentException()
        {
            // Arrange
            var sale = SaleTestData.CreateValidSale();

            // Act & Assert
            var action = () => sale.ReplaceItems(new SaleItem[0]);

            action.Should().Throw<ArgumentException>()
                .WithMessage("*At least one sale item is required*");
        }

        [Fact(DisplayName = "CreatedAt should be set when sale is created")]
        public void Constructor_WhenCalled_ShouldSetCreatedAt()
        {
            // Arrange
            var beforeCreation = DateTime.UtcNow;

            // Act
            var sale = SaleTestData.CreateValidSale();

            // Assert
            var afterCreation = DateTime.UtcNow;
            sale.CreatedAt.Should().BeAfter(beforeCreation.AddSeconds(-1));
            sale.CreatedAt.Should().BeBefore(afterCreation.AddSeconds(1));
        }

        [Fact(DisplayName = "UpdatedAt should be null for new sale")]
        public void Constructor_WhenCalled_UpdatedAtShouldBeNull()
        {
            // Arrange & Act
            var sale = SaleTestData.CreateValidSale();

            // Assert
            sale.UpdatedAt.Should().BeNull();
        }
    }
}