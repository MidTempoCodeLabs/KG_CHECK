using Adapters.Shared.Interfaces.Repositories;
using EPakapojumiDataServiceContext;

namespace DeclaredPersonsAdapter.Application.Interfaces.Repositories;

public interface IDeclaredPersonODataRepository : IAsyncBaseODataRepository<DeclaredPersons>
{

}
