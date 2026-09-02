using SupportCenter.Application.Abstractions.Messaging;

namespace SupportCenter.Application.Features.Organizations.CreateOrganization;

public sealed record CreateOrganizationCommand(
    string Name,
    string Slug)
    : ICommand<Guid>;