using DeclaredPersonsAdapter.Application.Interfaces.GroupingStrategies;
using DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;

namespace DeclaredPersonsAdapter.Infrastructure.GroupingStrategies;

internal class YearGroupingStrategy : IDeclaredPersonsGroupingStrategy
{
    public IEnumerable<IGrouping<string, GetDeclaredPersonResponse>> GroupBy(IEnumerable<GetDeclaredPersonResponse> data)
    {
        return data.GroupBy(d => d.Year.ToString());
    }
}
