namespace Mediator.Abstractions;

/// <summary>
/// Represents a request that can be sent through the mediator.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IRequest<out TResponse>
{
}