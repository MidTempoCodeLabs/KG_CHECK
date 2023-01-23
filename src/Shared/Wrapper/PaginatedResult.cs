using Shared.Wrapper.Enums;

namespace Shared.Wrapper;

public class PaginatedResult<T> : Result
{
    public PaginatedResult(List<T> data)
    {
        Data = data;
    }

    public List<T> Data { get; set; }

    public List<PagedTableFieldFilters>? Filters { get; set; }

    internal PaginatedResult(bool succeeded, List<T> data = default, List<string>? messages = null, int count = 0, int page = 1, int pageSize = 10, List<PagedTableFieldFilters>? filters = null)
    {
        Data = data;
        CurrentPage = page;
        Succeeded = succeeded;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Filters = filters;
    }

    public static PaginatedResult<T> Failure(List<string>? messages)
    {
        return new PaginatedResult<T>(false, default, messages);
    }

    public static PaginatedResult<T> Failure(ResultMessageType messageType)
    {
        return new PaginatedResult<T>(false, default, new List<string>() { GetMessage(messageType) });
    }

    public static PaginatedResult<T> Success(List<T> data, int count, int page, int pageSize, List<PagedTableFieldFilters>? filters = null)
    {
        return new PaginatedResult<T>(true, data, null, count, page, pageSize, filters);
    }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalCount { get; set; }
    public int PageSize { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;
}
