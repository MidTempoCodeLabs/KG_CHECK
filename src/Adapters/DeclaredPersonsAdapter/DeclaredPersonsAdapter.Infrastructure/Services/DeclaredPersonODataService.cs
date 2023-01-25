using AutoMapper;
using DeclaredPersonsAdapter.Application.Enums;
using DeclaredPersonsAdapter.Application.Interfaces.Repositories;
using DeclaredPersonsAdapter.Application.Interfaces.Services;
using DeclaredPersonsAdapter.Application.Requests.DeclaredPersonAnalyser;
using DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;
using DeclaredPersonsAdapter.Infrastructure.GroupingStrategies;
using DeclaredPersonsAdapter.Infrastructure.Helpers;
using Microsoft.Extensions.Logging;
using Shared.Wrapper;
using Shared.Wrapper.Interfaces;

namespace DeclaredPersonsAdapter.Infrastructure.Services;

public class DeclaredPersonODataService : IDeclaredPersonODataService
{
    private readonly IDeclaredPersonODataRepository _declaredPersonODataRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeclaredPersonODataService> _logger;

    public DeclaredPersonODataService(
        IDeclaredPersonODataRepository declaredPersonODataRepository,
        IMapper mapper,
        ILogger<DeclaredPersonODataService> logger
        )
    {
        _declaredPersonODataRepository = declaredPersonODataRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IResult<List<GetDeclaredPersonResponse>>> GetAll()
    {
        try
        {
            var declaredPersonOData = await _declaredPersonODataRepository.GetAllAsync();

            var resultMapped = _mapper.Map<List<GetDeclaredPersonResponse>>(declaredPersonOData);

            return await Result<List<GetDeclaredPersonResponse>>.SuccessAsync(resultMapped);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    public async Task<SummaryResult<GetDeclaredPersonGroupedResponse>> GetGroupedSummary(DeclaredPersonAnalyserOptionsRequest request)
    {
        try
        {
            var declaredPersonsResponse = (await _declaredPersonODataRepository.GetAllByRequestedOptionsAsync(request)).ToList();
            var districtName = declaredPersonsResponse.First().district_name;

            var declaredPersonsResponseMapped = _mapper.Map<List<GetDeclaredPersonResponse>>(declaredPersonsResponse);

            var declaredPersonsWithValueCalculated = (request.DeclaredPersonsGroupingType != null
                ? GetDeclaredPersonsGroupedAndCalculated(request, declaredPersonsResponseMapped, districtName)
                : GetDeclaredPersonsCalculated(request, declaredPersonsResponseMapped, districtName)).ToList();

            return BuildSummaryResultFromDeclaredPersonsAndReturn(declaredPersonsWithValueCalculated, districtName);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    private IEnumerable<GetDeclaredPersonGroupedResponse> GetDeclaredPersonsGroupedAndCalculated(
        DeclaredPersonAnalyserOptionsRequest request,
        IEnumerable<GetDeclaredPersonResponse> declaredPersons,
        string districtName
        )
    {
        var groupedData = GroupDeclaredPersonsByGroupRequest(declaredPersons, request.DeclaredPersonsGroupingType!.Value).ToList();

        return groupedData
            .Select((g, idx) =>
            {
                return new GetDeclaredPersonGroupedResponse
                {
                    Value = (int)g.Sum(d => d.Value),
                    Change = idx == 0
                        ? 0
                        : (int)g.Sum(d => d.Value) - (int)groupedData.ElementAt(idx - 1).Sum(d => d.Value),

                    Year = request.IsGroupingByYearIncluded() ? g.First().Year : null,

                    Month = request.IsGroupingByMonthIncluded() ? g.First().Month : null,

                    Day = request.IsGroupingByDayIncluded() ? g.First().Day : null,

                    DeclaredPersonsGroupingType = request.DeclaredPersonsGroupingType.Value,
                    DistrictId = request.District,
                    DistrictName = districtName
                };
            });
    }


    private IEnumerable<GetDeclaredPersonGroupedResponse> GetDeclaredPersonsCalculated(
        DeclaredPersonAnalyserOptionsRequest request,
        IReadOnlyCollection<GetDeclaredPersonResponse> declaredPersons,
        string districtName
        )
    {
        return declaredPersons.Select((r, idx) => new GetDeclaredPersonGroupedResponse()
        {
            Value = (int)r.Value,
            Change = idx == 0 ? 0 : (int)r.Value - (int)declaredPersons.ElementAt(idx - 1).Value,
            Year = r.Year,
            Month = r.Month,
            Day = r.Day,
            DistrictId = request.District,
            DistrictName = districtName
        });
    }

    private IEnumerable<IGrouping<string, GetDeclaredPersonResponse>> GroupDeclaredPersonsByGroupRequest(IEnumerable<GetDeclaredPersonResponse> data,
        DeclaredPersonsGroupingType groupingType)
    {
        var groupingService = new DeclaredPersonsGroupingHelperService();

        return groupingType switch
        {
            DeclaredPersonsGroupingType.ByYear => groupingService.GroupBy(data, new YearGroupingStrategy()),
            DeclaredPersonsGroupingType.ByMonth => groupingService.GroupBy(data, new MonthGroupingStrategy()),
            DeclaredPersonsGroupingType.ByDay => groupingService.GroupBy(data, new DayGroupingStrategy()),
            DeclaredPersonsGroupingType.ByYearAndMonth => groupingService.GroupBy(data, new YearMonthGroupingStrategy()),
            DeclaredPersonsGroupingType.ByYearAndDay => groupingService.GroupBy(data, new YearDayGroupingStrategy()),
            DeclaredPersonsGroupingType.ByMonthAndDay => groupingService.GroupBy(data, new MonthDayGroupingStrategy()),
            _ => throw new Exception("Invalid value for grouping type parameter")
        };
    }

    private static SummaryResult<GetDeclaredPersonGroupedResponse> BuildSummaryResultFromDeclaredPersonsAndReturn(List<GetDeclaredPersonGroupedResponse> gropedSummaryResult,
        string districtName)
    {
        var maxValue = gropedSummaryResult.Max(s => s.Value);
        var minValue = gropedSummaryResult.Min(s => s.Value);
        var averageValue = (int)Math.Round(gropedSummaryResult.Average(s => s.Value));

        var maxIncreaseInSummaryResults = gropedSummaryResult.OrderByDescending(s => s.Change).First();
        var maxIncrease = new MaxIncrease(maxIncreaseInSummaryResults.Change, maxIncreaseInSummaryResults.GroupFullName, districtName);

        var maxDropInSummaryResults = gropedSummaryResult.OrderBy(s => s.Change).First();
        var maxDrop = new MaxDrop(maxDropInSummaryResults.Change, maxDropInSummaryResults.GroupFullName, districtName);

        return SummaryResult<GetDeclaredPersonGroupedResponse>.Success(gropedSummaryResult, maxValue, minValue, averageValue, maxDrop, maxIncrease);
    }
}
