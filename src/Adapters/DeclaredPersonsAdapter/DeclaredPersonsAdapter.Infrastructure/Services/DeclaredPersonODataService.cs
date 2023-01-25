using System.Diagnostics;
using AutoMapper;
using DeclaredPersonsAdapter.Application.Enums;
using DeclaredPersonsAdapter.Application.Interfaces.Repositories;
using DeclaredPersonsAdapter.Application.Interfaces.Services;
using DeclaredPersonsAdapter.Application.Requests.DeclaredPersonAnalyser;
using DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;
using DeclaredPersonsAdapter.Infrastructure.GroupingStrategies;
using DeclaredPersonsAdapter.Infrastructure.Helpers;
using EPakapojumiDataServiceContext;
using Microsoft.Extensions.Logging;
using Shared.Extensions;
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

            var summaryResults = GetSummaryResults(request, declaredPersonsResponse, districtName);

            var maxValue = summaryResults.Max(s => s.Value);
            var minValue = summaryResults.Min(s => s.Value);
            var averageValue = (int)Math.Round(summaryResults.Average(s => s.Value));

            var maxIncreaseInSummaryResults = summaryResults.OrderByDescending(s => s.Change).First();
            var maxIncrease = new MaxIncrease(maxIncreaseInSummaryResults.Change, districtName, maxIncreaseInSummaryResults.GroupFullName);

            var maxDropInSummaryResults = summaryResults.OrderBy(s => s.Change).First();
            var maxDrop = new MaxDrop(maxDropInSummaryResults.Change, districtName, maxDropInSummaryResults.GroupFullName);

            return SummaryResult<GetDeclaredPersonGroupedResponse>.Success(summaryResults, maxValue, minValue, averageValue, maxDrop, maxIncrease);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    private IEnumerable<IGrouping<string, GetDeclaredPersonResponse>> GroupDeclaredPersonsByGroupRequest(IEnumerable<GetDeclaredPersonResponse> data,
        string groupParam)
    {
        var groupingService = new DeclaredPersonsGroupingHelperService();

        switch (groupParam)
        {
            case "y":
                return groupingService.GroupBy(data, new YearGroupingStrategy());
            case "m":
                return groupingService.GroupBy(data, new MonthGroupingStrategy());
            case "d":
                return groupingService.GroupBy(data, new DayGroupingStrategy());
            case "ym":
            case "my":
                return groupingService.GroupBy(data, new YearMonthGroupingStrategy());
            case "yd":
            case "dy":
                return groupingService.GroupBy(data, new YearDayGroupingStrategy());
            case "md":
            case "dm":
                return groupingService.GroupBy(data, new MonthDayGroupingStrategy());
            default:
                throw new Exception("Invalid value for req parameter");
        }
    }

    private List<GetDeclaredPersonGroupedResponse> GetSummaryResults(DeclaredPersonAnalyserOptionsRequest request, IEnumerable<DeclaredPersons> declaredPersonsResponse, string districtName)
    {
        var resultMapped = _mapper.Map<List<GetDeclaredPersonResponse>>(declaredPersonsResponse);

        if (request.Group != null)
        {
            var declaredPersonsGroupingType =
                DeclaredPersonsGroupingTypeMethods.GetEnumFromDescription(request.Group);

            var groupedData = GroupDeclaredPersonsByGroupRequest(resultMapped, request.Group).ToList();

            return groupedData
                .Select((g, idx) =>
                {
                    Debug.Assert(declaredPersonsGroupingType != null, nameof(declaredPersonsGroupingType) + " != null");
                    return new GetDeclaredPersonGroupedResponse
                    {
                        Value = (int)g.Sum(d => d.Value),
                        Change = idx == 0 ? 0 : (int)g.Sum(d => d.Value) - (int)groupedData.ElementAt(idx - 1).Sum(d => d.Value),
                        Year = declaredPersonsGroupingType == DeclaredPersonsGroupingType.ByYear ? int.Parse(g.Key) : request.Year,
                        Month = declaredPersonsGroupingType == DeclaredPersonsGroupingType.ByMonth ? int.Parse(g.Key) : request.Month,
                        Day = declaredPersonsGroupingType == DeclaredPersonsGroupingType.ByDay ? int.Parse(g.Key) : request.Day,
                        DeclaredPersonsGroupingType = declaredPersonsGroupingType.Value,
                        DistrictId = request.District,
                        DistrictName = districtName
                    };
                }).ToList();
        }

        return resultMapped.Select((r, idx) => new GetDeclaredPersonGroupedResponse()
        {
            Value = (int)r.Value,
            Change = idx == 0 ? 0 : (int)r.Value - (int)resultMapped.ElementAt(idx - 1).Value,
            Year = r.Year,
            Month = r.Month,
            Day = r.Day,
            DistrictId = request.District,
            DistrictName = districtName
        }).ToList();
    }
}
