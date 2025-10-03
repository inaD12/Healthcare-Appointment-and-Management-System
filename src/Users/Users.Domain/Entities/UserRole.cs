using Shared.Domain.Entities;

namespace Users.Domain.Entities;

public sealed class UserRole
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public string RoleName { get; set; } = null!;
    public Role Role { get; set; } = null!;
}