using Shared.Domain.Entities;

namespace Users.Domain.Entities;

public sealed class RolePermission
{
    public string RoleName { get; set; } = null!;
    public Role Role { get; set; } = null!;

    public string PermissionCode { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
}
