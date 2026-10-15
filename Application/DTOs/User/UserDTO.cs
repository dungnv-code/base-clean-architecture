#nullable disable

using Application.DTOs.Common;

namespace Application.DTOs.User;

public class UserRequestDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Address {get;set;}
    public string Avatar { get; set; }
    public string PhoneNumber { get; set; }
}

public class UserResponseDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address {get;set;}
    public string Avatar { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class 
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address {get;set;}
    public string Avatar { get; set; }
}


public class UserQueryDTO : BaseQueryDTO
{
    
}