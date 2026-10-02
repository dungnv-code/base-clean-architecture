using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string Ten { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
}