using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    /// <summary>
    /// AutoMapper profile for UpdateSale feature.
    /// </summary>
    public class UpdateSaleProfile : Profile
    {
        public UpdateSaleProfile()
        {
            // Map API request to application command
            CreateMap<UpdateSaleRequest, UpdateSaleCommand>();

            // Map application result (Unit) to API response - use AfterMap
            CreateMap<UpdateSaleRequest, UpdateSaleResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
