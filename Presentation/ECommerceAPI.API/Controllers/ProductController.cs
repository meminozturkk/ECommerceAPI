using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.RequestParameters;
using ECommerceAPI.Application.Services;
using ECommerceAPI.Application.ViewModels.Products;
using ECommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Cryptography.Xml;

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

        public ProductController(IProductWriteRepository productrepositoryWrite, IProductReadRepository productrepositoryRead, IWebHostEnvironment webHostEnvironment, IFileService fileservice = null)
        {
            _productrepositoryWrite = productrepositoryWrite;
            _productrepositoryRead = productrepositoryRead;
            _webHostEnvironment = webHostEnvironment;
            _fileservice = fileservice;
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
        public async Task<IActionResult> Upload()
        {
           await  _fileservice.UploadAsync("resoruce/product-images", Request.Form.Files);
            return Ok();
        }
    }
}
