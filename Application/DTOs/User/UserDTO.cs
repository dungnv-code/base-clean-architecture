#nullable disable

using System.ComponentModel.DataAnnotations;
using Application.DTOs.Common;

namespace Application.DTOs.User;

public abstract class UserBaseDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string Avatar { get; set; }
}

public class UserRequestDTO : UserBaseDTO
{
    public string Password { get; set; }
}

public class UserResponseDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string Avatar { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UserResponseFullDTO : UserResponseDTO
{
    public string Password { get; set; }
}

public class UserUpdateRequestDTO : UserBaseDTO
{
    // public Guid Id { get; set; }
}

public class UserLoginDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserLoginReponseDTO
{
    public string accessToken { get; set; }
    public Guid IdUser { get; set; }
}

public class UserQueryDTO : BaseQueryDTO
{

}
