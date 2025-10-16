using Mediator.Abstractions;
using Mediator.Extensions.FluentValidation;
using FluentValidation;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds FluentValidation support to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddFluentValidation(this IServiceCollection services) =>
        AddFluentValidation(services, Assembly.GetCallingAssembly());

    /// <summary>
    /// Adds FluentValidation support to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static IServiceCollection AddFluentValidation(this IServiceCollection services, Assembly assembly)
    {
        // Register the pipeline behaviour.
        services.AddScoped(typeof(IPipelineBehaviour<,>), typeof(ValidationBehaviour<,>));

        // Register the validators.
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}