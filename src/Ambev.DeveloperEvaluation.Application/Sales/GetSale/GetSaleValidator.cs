using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Validator for {@link GetSaleQuery}. Ensures the sale identifier is provided.
    /// </summary>
    public class GetSaleValidator : AbstractValidator<GetSaleQuery>
    {
        /// <summary>
        /// Initializes validation rules for GetSaleQuery.
        /// </summary>
        public GetSaleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Sale ID is required.");
        }
    }
}
