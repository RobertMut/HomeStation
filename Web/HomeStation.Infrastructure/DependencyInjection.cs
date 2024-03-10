using HomeStation.Application.Common.Enums;
using HomeStation.Application.Common.Interfaces;
using HomeStation.Application.Common.Options;
using HomeStation.Infrastructure.Persistence;
using HomeStation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HomeStation.Infrastructure;

public static partial class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        DatabaseOptions? databaseOptions = serviceProvider.GetService<DatabaseOptions>();
        
        services.AddDbContext<AirDbContext>(options =>
        {
            switch (databaseOptions?.DatabaseType)
            {
                case DatabaseType.SqlServer:
                    options.UseSqlServer(databaseOptions.ConnectionString);
                    break;
                case DatabaseType.PostgreSql:
                    options.UseSqlServer(databaseOptions.ConnectionString);
                    break;
                case DatabaseType.MySql:
                    options.UseNpgsql(databaseOptions.ConnectionString);
                    break;
                default:
                    throw new NotSupportedException($"Database type {databaseOptions.DatabaseType} not supported");
            }
        });

        services.AddScoped<IAirDbContext>(provider => provider.GetService<IAirDbContext>());
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    
        return services;
    }
}