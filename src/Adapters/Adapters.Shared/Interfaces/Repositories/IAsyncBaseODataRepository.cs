using System.Linq.Expressions;

namespace Adapters.Shared.Interfaces.Repositories;

public interface IAsyncBaseODataRepository<T> where T : class
{
    Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where);

    Task<IEnumerable<T>> GetAllAsync();
}
