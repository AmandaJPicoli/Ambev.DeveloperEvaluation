using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// AutoMapper profile for mapping Sale aggregate to GetSaleResult DTOs.
    /// </summary>
    public class GetSaleProfile : Profile
    {
        /// <summary>
        /// Initializes mappings between Sale, SaleItem and corresponding result DTOs.
        /// </summary>
        public GetSaleProfile()
        {
            CreateMap<Sale, GetSaleResult>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<SaleItem, GetSaleItemResult>();
        }
    }
}
