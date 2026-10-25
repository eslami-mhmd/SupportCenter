namespace SupportCenter.Application.Abstractions.Identity;

public interface ICurrentUserService
{
    Guid? UserId { get; }

    Guid? OrganizationId { get; }

    bool IsAuthenticated { get; }
}