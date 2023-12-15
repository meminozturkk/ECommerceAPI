using ECommerceAPI.Application.Abstraction.Storage;
using ECommerceAPI.Infrastructure.Storage;
using Microsoft.Extensions.DependencyInjection;


namespace ECommerceAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructurServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService, StorageService>();
        }
        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : class , IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();

        }
    }
}
