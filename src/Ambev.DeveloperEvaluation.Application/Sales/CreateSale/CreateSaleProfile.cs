using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// AutoMapper profile for mapping CreateSaleCommand DTOs to the Sale aggregate.
    /// </summary>
    public class CreateSaleProfile : Profile
    {
        /// <summary>
        /// Initializes mappings between CreateSaleCommand and the Sale entity, including items.
        /// </summary>
        public CreateSaleProfile()
        {
            CreateMap<CreateSaleCommand, Sale>()
                .ConstructUsing(cmd => new Sale(cmd.SaleNumber, cmd.Date, cmd.CustomerId, cmd.Branch,
                    cmd.Items.Select(i => new SaleItem(i.ProductId, i.Quantity, i.UnitPrice))));

            CreateMap<CreateSaleItemDto, SaleItem>();
        }
    }
}
