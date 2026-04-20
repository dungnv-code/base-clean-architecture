#nullable disable

using Application.DTOs.Common;

namespace Application.DTOs.Cho;

public class ChoRequestDTO
{
    public string Ten { get; set; }
    public string DiaChi { get; set; }
}

public class ChoResponseDTO
{
    public Guid Id { get; set; }
    public string Ten { get; set; }
    public string DiaChi { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ChoQueryDTO : BaseQueryDTO
{
}
