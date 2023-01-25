using DeclaredPersonsAdapter.Application.Interfaces.GroupingStrategies;
using DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;
using EPakapojumiDataServiceContext;

namespace DeclaredPersonsAdapter.Infrastructure.Helpers;

internal class DeclaredPersonsGroupingHelperService
{
    public IEnumerable<IGrouping<string, GetDeclaredPersonResponse>> GroupBy(IEnumerable<GetDeclaredPersonResponse> data, IDeclaredPersonsGroupingStrategy strategy)
    {
        return strategy.GroupBy(data);
    }
}
