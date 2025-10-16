namespace Mediator.Abstractions;

/// <summary>
/// Delegate for request handlers.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

/// <summary>
/// Interface for pipeline behaviours that can be applied to request handlers.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IPipelineBehaviour<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default);
}