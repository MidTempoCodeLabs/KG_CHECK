using Shared.Wrapper.Interfaces;

namespace Adapters.Shared.Interfaces.Services;

public interface IBaseODataService<T> where T : class
{
    Task<IResult<List<T>>> GetAll();
}
