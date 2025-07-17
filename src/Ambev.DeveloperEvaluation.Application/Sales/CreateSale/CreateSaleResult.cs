namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleResult
    {
        public Guid Id { get; init; }
        public decimal TotalAmount { get; init; }

        public CreateSaleResult(Guid id, decimal totalAmount)
        {
            Id = id;
            TotalAmount = totalAmount;
        }
    }
}
