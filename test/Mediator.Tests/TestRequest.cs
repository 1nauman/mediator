namespace Mediator.Tests;

internal class TestRequest : IRequest<string>
{
    public string Message { get; set; } = string.Empty;
}