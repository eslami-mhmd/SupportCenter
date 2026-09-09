using SupportCenter.Domain.Exceptions;

namespace SupportCenter.Domain.Users;

public sealed class User
{
    private User()
    {
    }

    private User(
        Guid id,
        Guid organizationId,
        string email,
        string displayName,
        UserRole role)
    {
        Id = id;
        OrganizationId = organizationId;
        Email = email;
        DisplayName = displayName;
        Role = role;
        CreatedDate = DateTime.UtcNow;
    }


    public Guid Id { get; private set; }

    public Guid OrganizationId { get; private set; }

    public string Email { get; private set; } = string.Empty;

    public string DisplayName { get; private set; } = string.Empty;

    public UserRole Role { get; private set; }

    public DateTime CreatedDate { get; private set; }


    public static User Create(
        Guid organizationId,
        string email,
        string displayName,
        UserRole role)
    {
        if (organizationId == Guid.Empty)
        {
            throw new DomainException(
                "Organization is required.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException(
                "Email is required.");
        }

        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new DomainException(
                "Display name is required.");
        }


        return new User(
            Guid.NewGuid(),
            organizationId,
            email,
            displayName,
            role);
    }
}