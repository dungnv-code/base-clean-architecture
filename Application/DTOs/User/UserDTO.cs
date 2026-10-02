#nullable disable

using Application.DTOs.Common;

namespace Application.DTOs.User;

public class UserRequestDTO
{
    public string Ten { get; set; }
    public string Email { get; set; }
    public string SoDienThoai { get; set; }
}

public class UserResponseDTO
{
    public Guid Id { get; set; }
    public string Ten { get; set; }
    public string Email { get; set; }
    public string SoDienThoai { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UserQueryDTO : BaseQueryDTO
{
}