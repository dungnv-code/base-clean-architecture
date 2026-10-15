using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? Avatar { get; set; }= string.Empty;
    public bool IsDeleted { get; set; } = false;

    public ICollection<UserRoles> Roles { get; set; } = new List<UserRoles>();    
}