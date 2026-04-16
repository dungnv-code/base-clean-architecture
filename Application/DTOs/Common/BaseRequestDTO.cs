#nullable disable

namespace Application.DTOs.Common;

public class BaseRequestDTO
{
}

public class BaseRequestDTO<T> : BaseRequestDTO
{
    public T Data { get; set; }
}
