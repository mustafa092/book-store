using application.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using persistence.Data;
using persistence.Data.Repositories;

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

        // Add repositories
        DefineRepositories(services);
    }

    private static void DefineRepositories(IServiceCollection services)
    {
        services.AddTransient<IBookRepository, BookRepository>();
    }
}