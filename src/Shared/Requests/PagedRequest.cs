using Shared.Attributes.Validation;

namespace Shared.Requests;

public abstract class PagedRequest
{
    [RequiredPositiveVal]
    public int PageSize { get; set; } = 10;
    
    [RequiredPositiveVal]
    public int PageNumber { get; set; } = 1;

    public string[]? Orderby { get; set; }
}
