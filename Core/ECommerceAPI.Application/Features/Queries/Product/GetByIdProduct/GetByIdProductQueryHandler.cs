using ECommerceAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using P = ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Application.Features.Queries.Product.GetByIdProduct
{
    internal class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
    {

        readonly IProductReadRepository _productReadRepository;
        public GetByIdProductQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
        {
           
            P.Product product = await _productReadRepository.Table
                .FirstOrDefaultAsync(p => p.Id.ToString() == request.Id, cancellationToken);
            
            return new()
            {
                
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                ProductImageFile = product.ProductImageFile.Select(pif => new P.ProductImageFile
                {
                    Path = pif.Path,
                }).ToList()
            };
        }
    }
}
