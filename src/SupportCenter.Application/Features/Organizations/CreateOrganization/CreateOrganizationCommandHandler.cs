using SupportCenter.Application.Abstractions.Messaging;
using SupportCenter.Application.Abstractions.Repositories;
using SupportCenter.Domain.Organizations;

namespace SupportCenter.Application.Features.Organizations.CreateOrganization;

public sealed class CreateOrganizationCommandHandler
    : ICommandHandler<CreateOrganizationCommand, Guid>
{
    private readonly IOrganizationRepository _repository;

    public CreateOrganizationCommandHandler(
        IOrganizationRepository repository)
    {
        _repository = repository;
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

        return organization.Id;
    }
}