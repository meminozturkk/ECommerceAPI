using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Product.GetProductSales
{
    public class GetProductSalesHandler : IRequestHandler<GetProductSalesRequest, GetProductSalesResponse>
    {
        readonly IOrderReadRepository _orderReadRepository;
        readonly IProductReadRepository _productReadRepository;

        public GetProductSalesHandler(IOrderReadRepository orderReadRepository, IProductReadRepository productReadRepository)
        {
            _orderReadRepository = orderReadRepository;
            _productReadRepository = productReadRepository;
        }

        public async Task<GetProductSalesResponse> Handle(GetProductSalesRequest request, CancellationToken cancellationToken)
        {
            
                var productSales = new Dictionary<Guid, int>();

                // Order tablosundan, tamamlanmış olanları ve ilişkili ürünleri al
                var completedOrders = await _orderReadRepository.GetAll(false)
                    .Include(o => o.Basket)
                        .ThenInclude(b => b.BasketItems)
                            .ThenInclude(bi => bi.Product)
                    .Where(o => o.CompletedOrder != null)
                    .ToListAsync();

                // Her bir order için, basket içerisindeki ürünleri ve miktarlarını alarak satış sayısını hesapla
                foreach (var order in completedOrders)
                {
                    foreach (var basketItem in order.Basket.BasketItems)
                    {
                        var productId = basketItem.ProductId;
                        var quantity = basketItem.Quantity;

                        if (productSales.ContainsKey(productId))
                        {
                            productSales[productId] += quantity;
                        }
                        else
                        {
                            productSales[productId] = quantity;
                        }
                    }
                }

                // Ürün ID'leri ile adlarını eşleştirme
                var productNames = await _productReadRepository.GetAll(false)
                    .Where(p => productSales.Keys.Contains(p.Id))
                    .ToDictionaryAsync(p => p.Id, p => p.Name);

                // İsim ve satış miktarını içeren yeni bir sözlük oluşturma
                var productSalesWithName = productSales
                    .OrderByDescending(ps => ps.Value)
                    .ToDictionary(ps => productNames[ps.Key], ps => ps.Value);

                return new GetProductSalesResponse
                {
                    ProductSales = productSalesWithName
                };
            }
        }
}

