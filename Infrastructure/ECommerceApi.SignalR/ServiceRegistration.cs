using ECommerceAPI.Application.Abstraction.Hubs;
using ECommerceApi.SignalR.HubServices;
using Microsoft.Extensions.DependencyInjection;


namespace ECommerceApi.SignalR
{
    public static class ServiceRegistration
    {
        public static void AddSignalRServices(this IServiceCollection collection)
        {
            collection.AddTransient<IProductHubService, ProductHubService>();
            collection.AddTransient<IOrderHubService, OrderHubService>();

            collection.AddSignalR();
        }
    }
}
