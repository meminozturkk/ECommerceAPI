﻿using ECommerceAPI.Application.Abstraction.Storage;
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
using MediatR;
using System.Net;
using ECommerceAPI.Application.Features.Commands.Product.CreateProduct;
using ECommerceAPI.Application.Features.Queries.Product.GetAllProduct;
using ECommerceAPI.Application.Features.Queries.Product.GetByIdProduct;
using ECommerceAPI.Application.Features.Commands.Product.UpdateProduct;
using ECommerceAPI.Application.Features.Commands.Product.RemoveProduct;
using ECommerceAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;
using ECommerceAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using ECommerceAPI.Application.Features.Queries.Product.ProductImageFile.GetProductImages;

using Microsoft.AspNetCore.Authorization;
using ECommerceAPI.Application.Features.Commands.ProductImageFile.ChangeShowcaseImage;
using ECommerceAPI.Application.Consts;
using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.Enums;
using ECommerceAPI.Application.Abstraction.Services;
using ECommerceAPI.Application.Features.Commands.Product.UpdateStockQrCodeToProduct;
using ECommerceAPI.Application.Features.Queries.Product.GetProductSales;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

      
        readonly IMediator _mediator;
        readonly ILogger<ProductController> _logger;
        readonly IProductService _productService;


        public ProductController(IMediator mediator, ILogger<ProductController> logger, IProductService productService)
        {

            _mediator = mediator;
            _logger = logger;
            _productService = productService;
        }


        [HttpPut("qrcode")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> UpdateStockQrCodeToProduct(UpdateStockQrCodeToProductCommandRequest updateStockQrCodeToProductCommandRequest)
        {
            UpdateStockQrCodeToProductCommandResponse response = await _mediator.Send(updateStockQrCodeToProductCommandRequest);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
        {
            _logger.LogInformation("asmdlasmdlamsd");
            GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);
        }
        [HttpGet("qrcode/{productId}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> GetQrCodeToProduct([FromRoute] string productId)
        {
            var data = await _productService.QrCodeToProductAsync(productId);
            return File(data, "image/png");
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] GetByIdProductQueryRequest getByIdProductQueryRequest)
        {
           GetByIdProductQueryResponse response =  await _mediator.Send(getByIdProductQueryRequest);
            return Ok(response);
        }

        
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Create Product")]

        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating, Definition = "Update Product")]

        public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
        {
            UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting, Definition = "Delete Product")]

        public async Task<IActionResult> Delete([FromRoute] RemoveProductCommandRequest removeProductCommandRequest)
        {
            RemoveProductCommandResponse response = await _mediator.Send(removeProductCommandRequest);
            return Ok();
        }
        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Upload Product File")]

        public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            uploadProductImageCommandRequest.Files = Request.Form.Files;
            await _mediator.Send(uploadProductImageCommandRequest);
            return Ok();
        }


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest)
        {
            List<GetProductImagesQueryResponse> responses =  await _mediator.Send(getProductImagesQueryRequest);
            return Ok(responses);
        }


        [HttpDelete("[action]/{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting, Definition = "Delete Product Image")]
        public async Task<IActionResult> DeleteProductImage([FromRoute] RemoveProductImageCommandRequest removeProductImageCommandRequest,
            [FromQuery] string imageId)
        {
            removeProductImageCommandRequest.ImageId = imageId;
            RemoveProductImageCommandResponse response = await _mediator.Send(removeProductImageCommandRequest);
            return Ok();
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating, Definition = "Change Showcase Image")]
        public async Task<IActionResult> ChangeShowcaseImage([FromQuery] ChangeShowcaseImageCommandRequest changeShowcaseImageCommandRequest)
        {
            ChangeShowcaseImageCommandResponse response = await _mediator.Send(changeShowcaseImageCommandRequest);
            return Ok(response);
        }
        [HttpGet("[action]")]
      
        public async Task<IActionResult> GetProductSales([FromQuery] GetProductSalesRequest getProductSalesRequest)
        {
            GetProductSalesResponse response = await _mediator.Send(getProductSalesRequest);
            return Ok(response);
        }
    }
}
