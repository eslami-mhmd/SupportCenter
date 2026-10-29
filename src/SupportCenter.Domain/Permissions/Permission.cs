using SupportCenter.Domain.Common;

namespace SupportCenter.Domain.Permissions;

public sealed class Permission : Entity
{
    private Permission()
        : base(Guid.Empty)
    {
    }


    private Permission(
        Guid id,
        string name)
        : base(id)
    {
        Name = name;
    }


    public string Name { get; private set; } = string.Empty;


    public static Permission Create(
        string name)
    {
        return new Permission(
            Guid.NewGuid(),
            name);
    }
}