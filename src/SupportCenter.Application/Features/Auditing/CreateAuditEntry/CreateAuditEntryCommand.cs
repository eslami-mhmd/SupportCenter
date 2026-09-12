using SupportCenter.Application.Abstractions.Messaging;

namespace SupportCenter.Application.Features.Auditing.CreateAuditEntry;

public sealed record CreateAuditEntryCommand(
    Guid? UserId,
    string Action,
    string EntityName,
    Guid EntityId,
    string? OldValues,
    string? NewValues)
    : ICommand<Guid>;