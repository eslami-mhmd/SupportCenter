namespace SupportCenter.Application.Security;

[AttributeUsage(
    AttributeTargets.Class |
    AttributeTargets.Method)]
public sealed class RequirePermissionAttribute : Attribute
{
    public RequirePermissionAttribute(
        string permission)
    {
        Permission = permission;
    }


    public string Permission { get; }
}