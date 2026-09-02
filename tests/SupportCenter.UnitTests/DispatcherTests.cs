using SupportCenter.Application;
using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Features.Test;
using Microsoft.Extensions.DependencyInjection;

namespace SupportCenter.UnitTests;

public class DispatcherTests
{
    [Fact]
    public async Task Dispatcher_should_execute_command_handler()
    {
        var services = new ServiceCollection();

        services.AddApplication();

        var provider = services.BuildServiceProvider();

        var dispatcher =
            provider.GetRequiredService<IDispatcher>();

        var result = await dispatcher.Send(
            new TestCommand("Mohammad"));


        Assert.Equal(
            "Hello Mohammad",
            result);
    }
}