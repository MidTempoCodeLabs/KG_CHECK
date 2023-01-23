using System.Linq.Expressions;
using Simple.OData.Client;

namespace Adapters.Shared.Repositories;

public abstract class AsyncBaseODataRepository<T> where T : class
{
    protected IBoundClient<T> BoundClient { get; set; }

    protected AsyncBaseODataRepository(string uriString)
    {
        BoundClient = new ODataClient(new Uri(uriString)).For<T>();
    }

    public virtual async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where)
    {
        return await BoundClient.Filter(where).FindEntriesAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await BoundClient.FindEntriesAsync();
    }
}
