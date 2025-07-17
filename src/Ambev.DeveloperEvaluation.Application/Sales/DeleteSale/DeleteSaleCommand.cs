using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    /// <summary>
    /// Command to cancel (delete) a sale.
    /// </summary>
    public record DeleteSaleCommand(Guid Id) : IRequest<DeleteSaleResponse>;
}
