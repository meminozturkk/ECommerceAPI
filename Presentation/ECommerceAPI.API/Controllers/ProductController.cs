using ECommerceAPI.Application.Abstraction.Storage;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.RequestParameters;
using ECommerceAPI.Application.Services;
using ECommerceAPI.Application.ViewModels.Products;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        readonly private IProductWriteRepository _productrepositoryWrite;
        readonly private IProductReadRepository _productrepositoryRead;
        readonly private IWebHostEnvironment _webHostEnvironment;
        readonly private IFileService _fileservice;
        readonly IStorageService _storageService;
        readonly IProductImageFileWriteRepository _productimagefilewriterepository;
        readonly IConfiguration configuration;

        public ProductController(IProductWriteRepository productrepositoryWrite, IProductReadRepository productrepositoryRead,
            IWebHostEnvironment webHostEnvironment, IStorageService storageService, IProductImageFileWriteRepository productimagefilewriterepository, IConfiguration configuration)
        {
            _productrepositoryWrite = productrepositoryWrite;
            _productrepositoryRead = productrepositoryRead;
            _webHostEnvironment = webHostEnvironment;

            _storageService = storageService;
            _productimagefilewriterepository = productimagefilewriterepository;
            this.configuration = configuration;
        }




        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination)
        {
            int totalCount = _productrepositoryRead.GetAll(false).Count();
            var products = _productrepositoryRead.GetAll(false).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate
            }).Skip(pagination.Page * pagination.Size).Take(pagination.Size);

            return Ok(
                new { totalCount, products });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _productrepositoryRead.GetByIdAsync(id, false));
        }
        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {
            if (ModelState.IsValid) {

            }
            await _productrepositoryWrite.AddAsync(new()
            {
                Name = model.Name,
                Stock = model.Stock,
                Price = model.Price,
            });
            await _productrepositoryWrite.SaveAsync();
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            Product product = await _productrepositoryRead.GetByIdAsync(model.Id);
            product.Stock = model.Stock;
            product.Price = model.Price;
            product.Name = model.Name;
            await _productrepositoryWrite.SaveAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productrepositoryWrite.DeleteAsync(id);
            await _productrepositoryWrite.SaveAsync();

            return Ok();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(string id)
        {
            List<(string fileName, string pathOrContainerName)> data= await _storageService.UploadAsync("photo-images", Request.Form.Files);
            Product product = await _productrepositoryRead.GetByIdAsync(id);

            var images = data.Select(x => new ProductImageFile
            {
                FileName = x.fileName,
                Path = x.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Product>() { product }
            }).ToList();
            await _productimagefilewriterepository.AddRangeAsync(images);
            await _productimagefilewriterepository.SaveAsync();
            return Ok();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductImages(string id)
        {
            Product? product = await _productrepositoryRead.Table.Include(p => p.ProductImageFile)
                    .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            return Ok(product.ProductImageFile.Select(p => new
            {
                Path = $"{configuration["BaseStorageUrl"]}/{p.Path}",
                p.FileName,
                p.Id
            }));
        }
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProductImage(string id, string imageId)
        {
            Product? product = await _productrepositoryRead.Table.Include(p => p.ProductImageFile)
                  .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

            ProductImageFile productImageFile = product.ProductImageFile.FirstOrDefault(p => p.Id == Guid.Parse(imageId));
            product.ProductImageFile.Remove(productImageFile);
            await _productrepositoryWrite.SaveAsync();
            return Ok();
        }
    }
}
