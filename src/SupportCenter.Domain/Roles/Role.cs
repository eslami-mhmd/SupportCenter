namespace SupportCenter.Domain.Roles;

public sealed class Role
{
    private readonly List<RolePermission> _permissions = new();


    private Role()
    {
    }


    private Role(
        Guid id,
        string name)
    {
        Id = id;
        Name = name;
    }


    public Guid Id { get; private set; }


    public string Name { get; private set; } = string.Empty;


    public IReadOnlyCollection<RolePermission> Permissions =>
        _permissions.AsReadOnly();


    public static Role Create(
        string name)
    {
        return new Role(
            Guid.NewGuid(),
            name);
    }


    public void AddPermission(
        RolePermission permission)
    {
        _permissions.Add(permission);
    }
}