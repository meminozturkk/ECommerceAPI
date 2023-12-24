using ECommerceAPI.Application.Abstraction.Storage;
using Microsoft.Extensions.DependencyInjection;
using ECommerceAPI.Infrastructure.Services.Storage;
using ECommerceAPI.Application.Abstraction.Token;
using ECommerceAPI.Infrastructure.Services.Token;

namespace ECommerceAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructurServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService, StorageService>();
            serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
        }
        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : Services.Storage.Storage, IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();

        }

    }
}
