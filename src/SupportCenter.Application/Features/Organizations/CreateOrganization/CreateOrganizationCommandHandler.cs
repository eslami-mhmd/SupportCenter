using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Organizations;
using SupportCenter.Application.Features.Auditing.CreateAuditEntry;

namespace SupportCenter.Application.Features.Organizations.CreateOrganization;

public sealed class CreateOrganizationCommandHandler
    : ICommandHandler<CreateOrganizationCommand, Guid>
{
    private readonly IOrganizationRepository _repository;
    private readonly IDispatcher _dispatcher;
    public CreateOrganizationCommandHandler(
        IOrganizationRepository repository,
        IDispatcher dispatcher)
    {
        _repository = repository;
        _dispatcher = dispatcher;
    }

    public async Task<Guid> Handle(
        CreateOrganizationCommand command,
        CancellationToken cancellationToken)
    {
        var organization = Organization.Create(
            command.Name,
            command.Slug);

        await _repository.AddAsync(
            organization,
            cancellationToken);

        await _dispatcher.Send<Guid>(
            new CreateAuditEntryCommand(
                null,
                "ORGANIZATION_CREATED",
                "Organization",
                organization.Id,
                null,
                $"{{\"Name\":\"{organization.Name}\"}}"),
        cancellationToken);

        return organization.Id;
    }
}