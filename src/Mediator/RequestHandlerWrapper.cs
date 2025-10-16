using Microsoft.Extensions.DependencyInjection;

namespace Mediator;

internal abstract class RequestHandlerWrapper<TResponse>
{
    public abstract Task<TResponse> HandleAsync(IRequest<TResponse> request,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default);
}

internal class RequestHandlerWrapperImpl<TRequest, TResponse> : RequestHandlerWrapper<TResponse>
    where TRequest : IRequest<TResponse>
{
    public override Task<TResponse> HandleAsync(IRequest<TResponse> request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        var behaviours = serviceProvider.GetServices<IPipelineBehaviour<TRequest, TResponse>>().ToList();

        RequestHandlerDelegate<TResponse> handlerDelegate =
            () => handler.HandleAsync((TRequest)request, cancellationToken);

        var pipeline = behaviours.Aggregate(handlerDelegate, (next, behaviour) =>
            () => behaviour.Handle((TRequest)request, next, cancellationToken));

        return pipeline();
    }
}