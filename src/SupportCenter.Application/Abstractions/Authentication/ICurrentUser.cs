namespace SupportCenter.Application.Abstractions.Authentication;

public interface ICurrentUser
{
    Guid? UserId { get; }

    Guid? OrganizationId { get; }
}