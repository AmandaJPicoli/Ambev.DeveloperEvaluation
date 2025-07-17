using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale
{
    /// <summary>
    /// AutoMapper profile for DeleteSale feature.
    /// </summary>
    public class DeleteSaleProfile : Profile
    {
        public DeleteSaleProfile()
        {
            // Map API request to application command
            CreateMap<DeleteSaleRequest, DeleteSaleCommand>();

            // Map result (unit) to API response
            CreateMap<DeleteSaleRequest, DeleteSaleResponse>()
                .ForMember(dest => dest.Success, opt => opt.MapFrom(_ => true));
        }
    }
}
