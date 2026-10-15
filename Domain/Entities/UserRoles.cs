
namespace Domain.Entities;

public class UserRoles : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid RoleId { get; set; }
    public Roles Role { get; set; } = null!;
}