﻿using ECommerceAPI.Application.Repositories.Basket;
using ECommerceAPI.Application.Repositories.BasketItem;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.ViewModels.Baskets;
using ECommerceAPI.Domain.Entities.Identity;
using ECommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAPI.Application.Abstraction.Services;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Persistence.Services
{
    public class BasketService : IBasketService
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly UserManager<AppUser> _userManager;
        readonly IOrderReadRepository _orderReadRepository;
        readonly IBasketWriteRepository _basketWriteRepository;
        readonly IBasketReadRepository _basketReadRepository;
        readonly IBasketItemWriteRepository _basketItemWriteRepository;
        readonly IBasketItemReadRepository _basketItemReadRepository;
        readonly IProductReadRepository _productReadRepository;
        public BasketService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IOrderReadRepository orderReadRepository, IBasketWriteRepository basketWriteRepository, IBasketItemWriteRepository basketItemWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketReadRepository basketReadRepository, IProductReadRepository productReadRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _orderReadRepository = orderReadRepository;
            _basketWriteRepository = basketWriteRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
            _basketReadRepository = basketReadRepository;
            _productReadRepository = productReadRepository;
        }

        private async Task<Basket?> ContextUser()
        {
            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(username))
            {
                AppUser? user = await _userManager.Users
                         .Include(u => u.Baskets)
                         .FirstOrDefaultAsync(u => u.UserName == username);

                var _basket = from basket in user.Baskets
                              join order in _orderReadRepository.Table
                              on basket.Id equals order.Id into BasketOrders
                              from order in BasketOrders.DefaultIfEmpty()
                              select new
                              {
                                  Basket = basket,
                                  Order = order
                              };

                Basket? targetBasket = null;
                if (_basket.Any(b => b.Order is null))
                    targetBasket = _basket.FirstOrDefault(b => b.Order is null)?.Basket;
                else
                {
                    targetBasket = new();
                    user.Baskets.Add(targetBasket);
                }

                await _basketWriteRepository.SaveAsync();
                return targetBasket;
            }
            throw new Exception("Beklenmeyen bir hatayla karşılaşıldı...");
        }

        public async Task AddItemToBasketAsync(VM_Create_BasketItem basketItem)
        {
            Basket? basket = await ContextUser();
            if (basket != null)
            {
                BasketItem _basketItem = await _basketItemReadRepository.GetSingleAsync(bi => bi.BasketId == basket.Id && bi.ProductId == Guid.Parse(basketItem.ProductId));
                Product product = await _productReadRepository.GetByIdAsync(basketItem?.ProductId);
                if (_basketItem != null)
                {
                    
                    if (product.Stock < basketItem.Quantity)
                    {
                        throw new Exception("Stok sayısından fazla sepete ürün eklenemez");
                    }
                    if (product.Stock > 0)
                    {
                        _basketItem.Quantity++;
                        product.Stock--;
                    }
                    else
                    {
                        throw new Exception("Stok sayısından fazla sepete ürün eklenemez");
                    }

                }
                else
                {
                    if (product.Stock < basketItem.Quantity)
                    {
                        throw new Exception("Stok sayısından fazla sepete ürün eklenemez");
                    }
                    if (product.Stock > 0)
                    {
                        await _basketItemWriteRepository.AddAsync(new()
                        {
                            BasketId = basket.Id,
                            ProductId = Guid.Parse(basketItem.ProductId),
                            Quantity = basketItem.Quantity
                        });

                       
                        product.Stock--;
                    }
                    else
                    {
                        throw new Exception("Stok sayısından fazla sepete ürün eklenemez");
                    }
                   
                }
                await _basketItemWriteRepository.SaveAsync();
            }
        }

        public async Task<List<BasketItem>> GetBasketItemsAsync()
        {
            Basket? basket = await ContextUser();
            Basket? result = await _basketReadRepository.Table
                 .Include(b => b.BasketItems)
                 .ThenInclude(bi => bi.Product)
                 .FirstOrDefaultAsync(b => b.Id == basket.Id);

            return result.BasketItems
                .ToList();
        }

        public async Task RemoveBasketItemAsync(string basketItemId)
        {
            
            BasketItem? basketItem = await _basketItemReadRepository.GetByIdAsync(basketItemId);
            if (basketItem != null)
            {
                Product product = await _productReadRepository.GetByIdAsync(basketItem.ProductId.ToString());
                product.Stock += basketItem.Quantity;
                _basketItemWriteRepository.Delete(basketItem);
                await _basketItemWriteRepository.SaveAsync();
            }
        }

        public async Task UpdateQuantityAsync(VM_Update_BasketItem basketItem)
        {
            BasketItem? _basketItem = await _basketItemReadRepository.GetByIdAsync(basketItem.BasketItemId);
            if (_basketItem != null)
            {
                Product product = await _productReadRepository.GetByIdAsync(_basketItem.ProductId.ToString());
                product.Stock -= basketItem.Quantity - _basketItem.Quantity;
                _basketItem.Quantity = basketItem.Quantity;
               
                if (product.Stock < 0 || product.Stock < (_basketItem.Quantity - basketItem.Quantity) )
                {
                    throw new Exception("Stok sayısından fazla sepete ürün eklenemez");
                }
                await _basketItemWriteRepository.SaveAsync();
            }
        }

        public Basket? GetUserActiveBasket
        {
            get
            {
                Basket? basket = ContextUser().Result;
                return basket;
            }
        }
    }
}
