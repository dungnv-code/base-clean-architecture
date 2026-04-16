#nullable disable

namespace Application.DTOs.Common;

public class BaseQueryDTO
{
    public string Keyword { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int Skip => IsGetAll ? 0 : (Page - 1) * PageSize;
    public bool IsGetAll { get; set; }
    public int Total { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}

public class BaseQueryDTO<T> : BaseQueryDTO
{
    public T Filter { get; set; }
}

public class QueryDTO : BaseQueryDTO
{
}

public class QueryDTO<T> : BaseQueryDTO<T>
{
}
