using Adapters.Shared.Interfaces.Services;
using DeclaredPersonsAdapter.Application.Requests.DeclaredPersonAnalyser;
using DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;
using EPakapojumiDataServiceContext;
using Shared.Wrapper;

namespace DeclaredPersonsAdapter.Application.Interfaces.Services;

public interface IDeclaredPersonODataService : IBaseODataService<GetDeclaredPersonResponse>
{
    Task<SummaryResult<GetDeclaredPersonGroupedResponse>> GetGroupedSummary(DeclaredPersonAnalyserOptionsRequest request);
}
