#nullable disable

namespace Application.Common;

public class PagedResult<T>
{
    public List<T> Items { get; init; }
    public int Total { get; init; }

    public PagedResult(List<T> items, int total)
    {
        Items = items;
        Total = total;
    }
}
