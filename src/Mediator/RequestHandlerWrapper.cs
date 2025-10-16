using Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator;

/// <summary>
/// Abstract Wrapper for request handlers.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
internal abstract class RequestHandlerWrapper<TResponse>
{
    public abstract Task<TResponse> HandleAsync(IRequest<TResponse> request,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Wrapper implementation for request handlers.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
internal class RequestHandlerWrapperImpl<TRequest, TResponse> : RequestHandlerWrapper<TResponse>
    where TRequest : IRequest<TResponse>
{
    public override Task<TResponse> HandleAsync(IRequest<TResponse> request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        // Resolve the handler from the service provider
        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        // Resolve all pipeline behaviours for this request/response pair from the service provider
        var behaviours = serviceProvider.GetServices<IPipelineBehaviour<TRequest, TResponse>>().ToList();

        // The actual handler delegate
        RequestHandlerDelegate<TResponse> handlerDelegate =
            () => handler.HandleAsync((TRequest)request, cancellationToken);

        var pipeline = behaviours.Aggregate(handlerDelegate, (next, behaviour) =>
            () => behaviour.Handle((TRequest)request, next, cancellationToken));

        return pipeline();
    }
}