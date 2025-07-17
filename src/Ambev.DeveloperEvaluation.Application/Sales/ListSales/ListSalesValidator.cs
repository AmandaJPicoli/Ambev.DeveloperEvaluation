using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales
{
    /// <summary>
    /// Validator for <see cref="ListSalesQuery"/>.
    /// </summary>
    public class ListSalesValidator : AbstractValidator<ListSalesQuery>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ListSalesValidator"/>.
        /// </summary>
        public ListSalesValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.Size)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100)
                .WithMessage("Page size must not exceed 100.");
        }
    }
}
