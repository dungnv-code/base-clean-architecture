
namespace Domain.Entities;

public class Roles : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public ICollection<UserRoles> Users { get; set; } = new List<UserRoles>();
}