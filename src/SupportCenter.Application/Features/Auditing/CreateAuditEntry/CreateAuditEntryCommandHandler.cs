using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Auditing;

namespace SupportCenter.Application.Features.Auditing.CreateAuditEntry;

public sealed class CreateAuditEntryCommandHandler
    : ICommandHandler<CreateAuditEntryCommand, Guid>
{
    private readonly IAuditRepository _repository;


    public CreateAuditEntryCommandHandler(
        IAuditRepository repository)
    {
        _repository = repository;
    }


    public async Task<Guid> Handle(
        CreateAuditEntryCommand command,
        CancellationToken cancellationToken)
    {
        var auditEntry =
            AuditEntry.Create(
                command.UserId,
                command.Action,
                command.EntityName,
                command.EntityId,
                command.OldValues,
                command.NewValues);


        await _repository.AddAsync(
            auditEntry,
            cancellationToken);


        return auditEntry.Id;
    }
}