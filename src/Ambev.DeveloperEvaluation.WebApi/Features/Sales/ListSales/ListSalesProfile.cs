using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    /// <summary>
    /// AutoMapper profile for the ListSales feature.
    /// </summary>
    public class ListSalesProfile : Profile
    {
        public ListSalesProfile()
        {
            // Map request to application query
            CreateMap<ListSalesRequest, ListSalesQuery>()
                .ForMember(dest => dest.MinTotal, opt => opt.MapFrom(src => src.MinTotal))
                .ForMember(dest => dest.MaxTotal, opt => opt.MapFrom(src => src.MaxTotal));

            // Map application result to API response
            CreateMap<ListSalesResult, ListSalesResponse>();
            CreateMap<SaleSummaryDto, SaleSummary>();
        }
    }
}
