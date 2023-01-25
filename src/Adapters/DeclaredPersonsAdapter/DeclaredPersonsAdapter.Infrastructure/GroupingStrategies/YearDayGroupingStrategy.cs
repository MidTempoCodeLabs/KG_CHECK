using DeclaredPersonsAdapter.Application.Interfaces.GroupingStrategies;
using DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;
using EPakapojumiDataServiceContext;

namespace DeclaredPersonsAdapter.Infrastructure.GroupingStrategies;

internal class YearDayGroupingStrategy : IDeclaredPersonsGroupingStrategy
{
    public IEnumerable<IGrouping<string, GetDeclaredPersonResponse>> GroupBy(IEnumerable<GetDeclaredPersonResponse> data)
    {
        return data.GroupBy(d => d.Year.ToString()).SelectMany(g => g.GroupBy(d => d.Day.ToString()));
    }
}