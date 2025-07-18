namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a sale transaction in the system following DDD principles.
    /// </summary>
    public class Sale
    {
        /// <summary>
        /// Gets the unique identifier for the sale.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the unique sale number for business identification.
        /// </summary>
        public string SaleNumber { get; private set; }

        /// <summary>
        /// Gets the date and time when the sale was made.
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the customer.
        /// </summary>
        public Guid CustomerId { get; private set; }

        /// <summary>
        /// Gets the branch where the sale was made.
        /// </summary>
        public string Branch { get; private set; }

        /// <summary>
        /// Gets the collection of items included in this sale.
        /// </summary>
        public List<SaleItem> Items { get; private set; } = new();

        /// <summary>
        /// Gets the total amount of the sale including all discounts.
        /// </summary>
        public decimal TotalAmount => Items.Sum(i => i.Total);

        /// <summary>
        /// Gets a value indicating whether the sale has been cancelled.
        /// </summary>
        public bool Cancelled { get; private set; }

        /// <summary>
        /// Gets the date and time when the sale was created.
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Gets the date and time when the sale was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; private set; }

        /// <summary>
        /// Private constructor for ORM use.
        /// </summary>
        private Sale() { }

        /// <summary>
        /// Initializes a new instance of the Sale class.
        /// </summary>
        /// <param name="saleNumber">The unique sale number</param>
        /// <param name="date">The date of the sale</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="branch">The branch where the sale was made</param>
        /// <param name="items">The collection of sale items</param>
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
            CreatedAt = DateTime.UtcNow;
            Items.AddRange(items);
        }

        /// <summary>
        /// Updates the branch where the sale was made.
        /// </summary>
        /// <param name="branch">The new branch identifier</param>
        public void UpdateBranch(string branch)
        {
            if (string.IsNullOrWhiteSpace(branch))
                throw new ArgumentException("Branch cannot be null or empty.", nameof(branch));

            Branch = branch;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Replaces all items in the sale with a new collection.
        /// </summary>
        /// <param name="items">The new collection of sale items</param>
        public void ReplaceItems(IEnumerable<SaleItem> items)
        {
            if (items == null || !items.Any())
                throw new ArgumentException("At least one sale item is required.", nameof(items));

            Items.Clear();
            Items.AddRange(items);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Cancels the sale, marking it as cancelled.
        /// </summary>
        public void CancelSale()
        {
            Cancelled = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}