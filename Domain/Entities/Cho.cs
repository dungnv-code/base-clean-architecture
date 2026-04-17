using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Cho : BaseEntity
{
    public string Ten { get; set; } = string.Empty;
    public string DiaChi { get; set; } = string.Empty;
}
