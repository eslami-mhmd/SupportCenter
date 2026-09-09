using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Sla;

namespace SupportCenter.Application.Features.Sla.CreateSlaPolicy;

public sealed class CreateSlaPolicyCommandHandler
    : ICommandHandler<CreateSlaPolicyCommand, Guid>
{
    private readonly ISlaPolicyRepository _repository;

    public CreateSlaPolicyCommandHandler(
        ISlaPolicyRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(
        CreateSlaPolicyCommand command,
        CancellationToken cancellationToken)
    {
        var policy = SlaPolicy.Create(
            command.OrganizationId,
            command.Priority,
            command.ResponseTimeMinutes,
            command.ResolutionTimeMinutes);

        await _repository.AddAsync(
            policy,
            cancellationToken);

        return policy.Id;
    }
}