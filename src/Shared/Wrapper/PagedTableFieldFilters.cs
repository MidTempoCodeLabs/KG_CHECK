namespace Shared.Wrapper;

public class PagedTableFieldFilters
{
    public string? PropertyName { get; set; }
    public string? InputLabelName { get; set; }
    public List<FieldFilterValue> Values { get; set; }
    public List<string>? SelectedValues { get; set; }

    public PagedTableFieldFilters(string propertyName, List<FieldFilterValue> values, string? inputLabelName = null)
    {
        PropertyName = propertyName;
        Values = values;
        InputLabelName = inputLabelName;
    }
}

public class FieldFilterValue
{
    public string? Value { get; set; }
}
