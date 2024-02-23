using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace application;

public static class ApplicationServicesConfigurations
{
    // adding the configuration for the application services
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddTransient<IBookService, BookService>();
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ApplicationServicesConfigurations).Assembly));
    }
}