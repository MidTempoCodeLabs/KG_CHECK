using DeclaredPersonsAdapter.Application.Interfaces.GroupingStrategies;
using DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;

namespace DeclaredPersonsAdapter.Infrastructure.GroupingStrategies;

internal class YearMonthGroupingStrategy : IDeclaredPersonsGroupingStrategy
{
    public IEnumerable<IGrouping<string, GetDeclaredPersonResponse>> GroupBy(IEnumerable<GetDeclaredPersonResponse> data)
    {
        return data.GroupBy(d => d.Year.ToString())
            .SelectMany(g => g.GroupBy(d => d.Month.ToString()));
    }
}
