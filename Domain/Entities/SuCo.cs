using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class SuCo : BaseEntity
{
    public string MucDo { get; set; } = string.Empty;
    public string MoTa { get; set; } = string.Empty;
    public string TrangThai { get; set; } = string.Empty;
}
