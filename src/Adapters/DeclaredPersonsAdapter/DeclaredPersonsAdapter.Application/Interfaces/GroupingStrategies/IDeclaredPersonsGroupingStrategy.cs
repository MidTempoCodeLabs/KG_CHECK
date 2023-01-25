using DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;
using EPakapojumiDataServiceContext;

namespace DeclaredPersonsAdapter.Application.Interfaces.GroupingStrategies;

public interface IDeclaredPersonsGroupingStrategy
{
    IEnumerable<IGrouping<string, GetDeclaredPersonResponse>> GroupBy(IEnumerable<GetDeclaredPersonResponse> data);
}