using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    /// <summary>
    /// Validator for <see cref="ListSalesRequest"/>.
    /// </summary>
    public class ListSalesRequestValidator : AbstractValidator<ListSalesRequest>
    {
        public ListSalesRequestValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.Size)
                .GreaterThan(0).WithMessage("Size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Size must not exceed 100.");
        }
    }
}
