using System.Reflection;
using Mediator;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<IMediator, Mediator.Mediator>();

        var requestHandlerTypes = assembly.GetTypes().Where(t =>
                t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
            .ToList();

        foreach (var requestHandlerType in requestHandlerTypes)
        {
            var types = requestHandlerType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                .ToList();

            foreach (var type in types)
            {
                services.AddScoped(type, requestHandlerType);
            }
        }

        return services;
    }
}