using SupportCenter.Application.Abstractions.Messaging;

namespace SupportCenter.Application.Features.Test;

public class TestCommandHandler
    : ICommandHandler<TestCommand, string>
{
    public Task<string> Handle(
        TestCommand command,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(
            $"Hello {command.Name}");
    }
}