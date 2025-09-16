using Microsoft.Extensions.DependencyInjection;

namespace Mediator;

internal abstract class RequestHandlerWrapper<TResponse>
{
    public abstract Task<TResponse> Handle(IRequest<TResponse> request,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken);
}

internal class RequestHandlerWrapperImpl<TRequest, TResponse> : RequestHandlerWrapper<TResponse>
    where TRequest : IRequest<TResponse>
{
    public override Task<TResponse> Handle(IRequest<TResponse> request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        var behaviours = serviceProvider.GetServices<IPipelineBehaviour<TRequest, TResponse>>().ToList();

        RequestHandlerDelegate<TResponse> handlerDelegate = () => handler.Handle((TRequest)request, cancellationToken);

        var pipeline = behaviours.Aggregate(handlerDelegate, (next, behaviour) =>
            () => behaviour.Handle((TRequest)request, next, cancellationToken));

        return pipeline();
    }
}