using cachingManager.services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace cachingManager;

public static class CachingServicesConfigurations
{
    public static void AddCachingServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();
    }
}