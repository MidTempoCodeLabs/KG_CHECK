using Adapters.Shared.Interfaces.Services;
using DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;

namespace DeclaredPersonsAdapter.Application.Interfaces.Services;

public interface IDeclaredPersonODataService : IBaseODataService<GetDeclaredPersonResponse>
{
}
