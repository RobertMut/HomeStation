using HomeStation.Application.Common.Interfaces;
using HomeStation.Domain.Common.CQRS;
using HomeStation.Domain.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HomeStation.Application;

public static partial class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.TryAddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.TryAddSingleton<IQueryDispatcher, QueryDispatcher>();
        
        services.Scan(selector =>
        {
            selector.FromCallingAssembly()
                .AddClasses(filter =>
                {
                    filter.AssignableTo(typeof(IQueryHandler<,>));
                }).AsImplementedInterfaces()
                .WithSingletonLifetime();
        });
        
        services.Scan(selector =>
        {
            selector.FromCallingAssembly()
                .AddClasses(filter =>
                {
                    filter.AssignableTo(typeof(ICommandHandler<,>));
                }).AsImplementedInterfaces()
                .WithSingletonLifetime();
        });
        
        return services;
    }
}