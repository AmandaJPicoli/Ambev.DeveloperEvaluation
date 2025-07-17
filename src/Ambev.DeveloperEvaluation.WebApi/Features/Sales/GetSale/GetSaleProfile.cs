using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// AutoMapper profile to map between API requests/responses and application queries/results.
    /// </summary>
    public class GetSaleProfile : Profile
    {
        public GetSaleProfile()
        {
            // Map API request to application query
            CreateMap<GetSaleRequest, GetSaleQuery>();

            // Map application result to API response
            CreateMap<GetSaleResult, GetSaleResponse>();
            CreateMap<GetSaleItemResult, GetSaleItemResponse>();
        }
    }
}
