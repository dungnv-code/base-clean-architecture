using Application.DTOs.Common;

namespace Application.DTOs.Role;

public class RoleDTO
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}

public class RoleQueryDTO : BaseQueryDTO
{

}
