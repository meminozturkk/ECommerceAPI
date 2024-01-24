using ECommerceAPI.Application.ViewModels.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstraction.Services
{
    public interface IBasketService
    {
       
            public Task<List<ECommerceAPI.Domain.Entities.BasketItem>> GetBasketItemsAsync();
            public Task AddItemToBasketAsync(VM_Create_BasketItem basketItem);
            public Task UpdateQuantityAsync(VM_Update_BasketItem basketItem);
            public Task RemoveBasketItemAsync(string basketItemId);
        
    }
}
