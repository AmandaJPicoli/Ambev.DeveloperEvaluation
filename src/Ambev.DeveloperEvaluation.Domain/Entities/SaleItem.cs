namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem
    {
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Total => (UnitPrice * Quantity) - Discount;

        private SaleItem() { }

        public SaleItem(Guid productId, int quantity, decimal unitPrice)
        {
            if (productId == Guid.Empty)
                throw new ArgumentException("Product ID is required.", nameof(productId));
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than 0.");
            if (quantity > 20)
                throw new InvalidOperationException("Cannot sell more than 20 units of a product.");
            if (unitPrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price must be greater than 0.");

            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = CalculateDiscount(quantity, unitPrice);
        }

        private decimal CalculateDiscount(int quantity, decimal unitPrice)
        {
            if (quantity < 4) return 0;
            if (quantity < 10) return quantity * unitPrice * 0.10m;
            return quantity * unitPrice * 0.20m;
        }
    }
}
