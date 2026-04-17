using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class KhuVuc : BaseEntity
{

    public string Ten { get; set; } = string.Empty;
    public string Loai { get; set; } = string.Empty;
    public string? MoTa { get; set; }
    
}
