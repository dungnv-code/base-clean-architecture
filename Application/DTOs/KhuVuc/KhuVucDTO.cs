#nullable disable

using Application.DTOs.Common;

namespace Application.DTOs.KhuVuc;

public class KhuVucRequestDTO
{
    public Guid? ChoId { get; set; }
    public string Ten { get; set; }
    public string Loai { get; set; }
    public string MoTa { get; set; }
}

public class KhuVucResponseDTO
{
    public Guid Id { get; set; }
    public Guid? ChoId { get; set; }
    public string Ten { get; set; }
    public string Loai { get; set; }
    public string MoTa { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class KhuVucQueryDTO : BaseQueryDTO
{
    public Guid? ChoId { get; set; }
    public string Loai { get; set; }
}
