using ECommerceAPI.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        readonly private IProductWriteRepository _productrepositoryWrite;
        readonly private IProductReadRepository _productrepositoryRead;

        public ProductController(IProductWriteRepository productrepositoryWrite, IProductReadRepository productrepositoryRead)
        {
            _productrepositoryRead = productrepositoryRead;
            _productrepositoryWrite = productrepositoryWrite;
        }
        [HttpGet]
        public async Task Get()
        {
           await _productrepositoryWrite.AddRangeAsync(new()
            {
                new() { Id= Guid.NewGuid(), Name="Product 1" , CreatedDate = DateTime.UtcNow, Price = 100, Stock = 10 },
                new() { Id= Guid.NewGuid(), Name="Product 2" , CreatedDate = DateTime.UtcNow, Price = 110, Stock = 10 },
                new() { Id= Guid.NewGuid(), Name="Product 3" , CreatedDate = DateTime.UtcNow, Price = 120, Stock = 10 }
            });
            await _productrepositoryWrite.SaveAsync();
        }

    }
}
