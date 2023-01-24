namespace Shared.Wrapper;

public class SummaryResult<T> : Result
{
    public SummaryResult(List<T> data)
    {
        Data = data;
    }

    public List<T> Data { get; set; }

    internal SummaryResult(
        bool succeeded,
        List<T> data,
        List<string>? messages = null,
        int max = 0,
        int min = 0,
        int average = 0,
        MaxDrop? maxDrop = null,
        MaxIncrease? maxIncrease = null)
    {
        Succeeded = succeeded;
        Data = data;
        Max = max;
        Min = min;
        Average = average;
        MaxDrop = maxDrop;
        MaxIncrease = maxIncrease;
        Messages = messages;
    }

    public static SummaryResult<T> Failure(List<string>? messages)
    {
        return new SummaryResult<T>(false, new List<T>(), messages);
    }

    public static SummaryResult<T> Success(List<T> data, int max,
        int min,
        int average,
        MaxDrop maxDrop,
        MaxIncrease maxIncrease)
    {
        return new SummaryResult<T>(true, data, null, max, min, average, maxDrop, maxIncrease);
    }

    public int Max { get; set; }
    public int Min { get; set; }
    public int Average { get; set; }

    public MaxDrop? MaxDrop { get; set; }
    public MaxIncrease? MaxIncrease { get; set; }
}

public class MaxDrop : SummaryValueChange
{
}

public class MaxIncrease : SummaryValueChange
{
}

public abstract class SummaryValueChange
{
    public int Value { get; set; }
    public string Group { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}