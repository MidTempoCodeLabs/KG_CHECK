using Adapters.Shared.Interfaces.Repositories;
using DeclaredPersonsAdapter.Application.Requests.DeclaredPersonAnalyser;
using EPakapojumiDataServiceContext;

namespace DeclaredPersonsAdapter.Application.Interfaces.Repositories;

public interface IDeclaredPersonODataRepository : IAsyncBaseODataRepository<DeclaredPersons>
{
    Task<IEnumerable<DeclaredPersons>> GetAllByRequestedOptionsAsync(DeclaredPersonAnalyserOptionsRequest request);
}
