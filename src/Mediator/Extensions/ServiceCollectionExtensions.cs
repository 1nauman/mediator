using System.Reflection;
using Mediator;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Mediator services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMediator(this IServiceCollection services) =>
        AddMediator(services, Assembly.GetCallingAssembly());

    /// <summary>
    /// Adds Mediator services to the specified IServiceCollection.
    /// Scans the specified assembly for request handlers.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
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