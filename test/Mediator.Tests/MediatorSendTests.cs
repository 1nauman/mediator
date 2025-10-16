using Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator.Tests;

public class MediatorSendTests
{
    [Fact]
    public async Task Send_ShouldResolveAndExecuteCorrectHandler()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddScoped<IRequestHandler<TestRequest, string>, TestRequestHandler>();
        var serviceProvider = services.BuildServiceProvider();
        var sut = new Mediator(serviceProvider);
        var request = new TestRequest { Message = "Hello, Mediator!" };

        // Act
        var result = await sut.SendAsync(request);

        // Assert
        Assert.Equal("Hello, Mediator!", result);
    }

    [Fact]
    public async Task Send_ShouldThrowException_WhenNoHandlerIsRegistered()
    {
        // Arrange
        var services = new ServiceCollection(); // No handler registered
        var serviceProvider = services.BuildServiceProvider();

        var sut = new Mediator(serviceProvider);
        var request = new TestRequest { Message = "This will fail" };

        // Act
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.SendAsync(request));
    }
}