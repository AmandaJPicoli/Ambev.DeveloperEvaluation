namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; private set; }
        public string SaleNumber { get; private set; }
        public DateTime Date { get; private set; }
        public Guid CustomerId { get; private set; }
        public string Branch { get; private set; }
        public List<SaleItem> Items { get; private set; } = new();
        public decimal TotalAmount => Items.Sum(i => i.Total);
        public bool Cancelled { get; private set; }

        private Sale() { }

        public Sale(string saleNumber, DateTime date, Guid customerId, string branch, IEnumerable<SaleItem> items)
        {
            if (string.IsNullOrWhiteSpace(saleNumber))
                throw new ArgumentException("Sale number cannot be null or empty.", nameof(saleNumber));
            if (string.IsNullOrWhiteSpace(branch))
                throw new ArgumentException("Branch cannot be null or empty.", nameof(branch));
            if (customerId == Guid.Empty)
                throw new ArgumentException("Customer ID is required.", nameof(customerId));
            if (items == null || !items.Any())
                throw new ArgumentException("At least one sale item is required.", nameof(items));

            Id = Guid.NewGuid();
            SaleNumber = saleNumber;
            Date = date;
            CustomerId = customerId;
            Branch = branch;
            Cancelled = false;
            Items.AddRange(items);
        }

        public void UpdateBranch(string branch)
        {
            if (string.IsNullOrWhiteSpace(branch))
                throw new ArgumentException("Branch cannot be null or empty.", nameof(branch));
            Branch = branch;
        }

        public void ReplaceItems(IEnumerable<SaleItem> items)
        {
            if (items == null || !items.Any())
                throw new ArgumentException("At least one sale item is required.", nameof(items));
            Items.Clear();
            Items.AddRange(items);
        }

        public void CancelSale()
        {
            Cancelled = true;
        }
    }
}
