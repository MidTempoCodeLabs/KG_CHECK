using Adapters.Shared.Constants;
using Adapters.Shared.Repositories;
using DeclaredPersonsAdapter.Application.Interfaces.Repositories;
using EPakapojumiDataServiceContext;

namespace DeclaredPersonsAdapter.Infrastructure.Repositories;

public class DeclaredPersonODataRepository : AsyncBaseODataRepository<DeclaredPersons>, IDeclaredPersonODataRepository
{
    public DeclaredPersonODataRepository()
        : base(ODataSourceConstants.Epakalpojumi.DataSourceUriString)
    {
    }
}
