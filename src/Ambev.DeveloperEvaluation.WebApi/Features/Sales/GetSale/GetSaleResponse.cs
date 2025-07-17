namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// Response DTO returned after retrieving a sale.
    /// </summary>
    public class GetSaleResponse
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; } = default!;
        public DateTime Date { get; set; }
        public Guid CustomerId { get; set; }
        public string Branch { get; set; } = default!;
        public bool Cancelled { get; set; }
        public decimal TotalAmount { get; set; }
        public List<GetSaleItemResponse> Items { get; set; } = new();
    }

    /// <summary>
    /// DTO representing an individual item within the sale response.
    /// </summary>
    public class GetSaleItemResponse
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }
}
