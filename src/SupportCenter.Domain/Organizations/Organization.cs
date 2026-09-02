using SupportCenter.Domain.Common;
using SupportCenter.Domain.Exceptions;

namespace SupportCenter.Domain.Organizations;

public sealed class Organization : AggregateRoot
{
    private Organization()
        : base(Guid.Empty)
    {
    }


    private Organization(
        Guid id,
        string name,
        string slug)
        : base(id)
    {
        Name = name;
        Slug = slug;
        Status = OrganizationStatus.Active;
        CreatedDate = DateTime.UtcNow;
    }


    public string Name { get; private set; } = null!;


    public string Slug { get; private set; } = null!;


    public OrganizationStatus Status { get; private set; }


    public DateTime CreatedDate { get; private set; }


    public static Organization Create(
        string name,
        string slug)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException(
                "Organization name is required.");


        if (string.IsNullOrWhiteSpace(slug))
            throw new DomainException(
                "Organization slug is required.");


        return new Organization(
            Guid.NewGuid(),
            name,
            slug);
    }


    public void Suspend()
    {
        if (Status == OrganizationStatus.Deleted)
            throw new InvalidOperationException(
                "Deleted organization cannot be suspended.");


        Status = OrganizationStatus.Suspended;
    }


    public void Activate()
    {
        if (Status == OrganizationStatus.Deleted)
            throw new InvalidOperationException(
                "Deleted organization cannot be activated.");


        Status = OrganizationStatus.Active;
    }
}