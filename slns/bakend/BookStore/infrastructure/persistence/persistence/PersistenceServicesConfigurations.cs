using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using persistence.Context;

namespace persistence;

public static class PersistenceServicesConfigurations
{
    public static void AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        
        var connectionString = configuration.GetConnectionString("Elm_DB_CS") ??
                               throw new InvalidOperationException(
                                   "Connection string section 'Elm_DB_CS' not found.");
        services.AddDbContext<ElmDbContext>(options =>
            options.UseSqlServer(connectionString, optionsBuilder => { }));
    }
}