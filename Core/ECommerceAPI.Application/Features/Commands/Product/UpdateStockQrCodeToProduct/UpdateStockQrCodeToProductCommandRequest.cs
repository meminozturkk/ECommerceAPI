using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Commands.Product.UpdateStockQrCodeToProduct
{
    public class UpdateStockQrCodeToProductCommandRequest : IRequest<UpdateStockQrCodeToProductCommandResponse>
    {
        public string ProductId { get; set; }
        public int? Stock { get; set; }
        public string? Name { get; set; }
        public float? Price { get; set; }
    }
}
