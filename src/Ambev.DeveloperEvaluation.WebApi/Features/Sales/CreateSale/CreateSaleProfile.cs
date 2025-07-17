using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    /// <summary>
    /// AutoMapper profile to map between API requests/responses and application commands/results.
    /// </summary>
    public class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {
            // Map API request to application command
            CreateMap<CreateSaleRequest, CreateSaleCommand>();

            // Map application result to API response
            CreateMap<CreateSaleResult, CreateSaleResponse>();
        }
    }
}
