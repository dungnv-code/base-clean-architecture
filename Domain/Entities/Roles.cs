
namespace Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public ICollection<UserRole> Users { get; set; } = new List<UserRole>();
}