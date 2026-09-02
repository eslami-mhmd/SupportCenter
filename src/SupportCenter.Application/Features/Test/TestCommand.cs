using SupportCenter.Application.Abstractions.Messaging;

namespace SupportCenter.Application.Features.Test;

public sealed record TestCommand(string Name)
    : ICommand<string>;