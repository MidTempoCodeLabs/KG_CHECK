using DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;

namespace DeclaredPersonsAdapter.Application.Interfaces.GroupingStrategies;

public interface IDeclaredPersonsGroupingStrategy
{
    IEnumerable<IGrouping<string, GetDeclaredPersonResponse>> GroupBy(IEnumerable<GetDeclaredPersonResponse> data);
}