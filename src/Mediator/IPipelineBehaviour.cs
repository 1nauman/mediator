namespace Mediator;

public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

public interface IPipelineBehaviour<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default);
}