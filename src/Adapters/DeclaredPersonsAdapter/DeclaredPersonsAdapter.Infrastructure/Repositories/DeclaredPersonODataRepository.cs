using System.Linq.Expressions;
using Adapters.Shared.Constants;
using Adapters.Shared.Repositories;
using DeclaredPersonsAdapter.Application.Interfaces.Repositories;
using DeclaredPersonsAdapter.Application.Requests.DeclaredPersonAnalyser;
using EPakapojumiDataServiceContext;
using Shared.Extensions;

namespace DeclaredPersonsAdapter.Infrastructure.Repositories;

public class DeclaredPersonODataRepository : AsyncBaseODataRepository<DeclaredPersons>, IDeclaredPersonODataRepository
{
    public DeclaredPersonODataRepository()
        : base(ODataSourceConstants.Epakalpojumi.DataSourceUriString)
    {
    }

    public async Task<IEnumerable<DeclaredPersons>> GetAllByRequestedOptionsAsync(DeclaredPersonAnalyserOptionsRequest request)
    {
        return await BoundClient
            .Filter(r => r.district_id == request.District)
            .Filter(ApplyDayMonthYearFilter(request.Year, request.Month, request.Day))
            .OrderBy(r => r.year)
            .OrderBy(r => r.month)
            .OrderBy(r => r.day)
            // .Top(request.Limit) "Cik ierakstus izvadīt nenozīmē cik daudz ierakstus paņemt no OData Service"
            .FindEntriesAsync();
    }

    private Expression<Func<DeclaredPersons, bool>> ApplyDayMonthYearFilter(int? year, int? month, int? day)
    {
        Expression<Func<DeclaredPersons, bool>> filter = d => true;

        // my custom expression - And
        if (year.HasValue)
        {
            filter = filter.And(d => d.year == year);
        }

        if (month.HasValue)
        {
            filter = filter.And(d => d.month == month);
        }

        if (day.HasValue)
        {
            filter = filter.And(d => d.day == day);
        }

        return filter;
    }
}
