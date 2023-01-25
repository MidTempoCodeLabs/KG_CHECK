using AutoMapper;
using BetterConsoleTables;
using DeclaredPersonsAdapter.Application.Enums;
using DeclaredPersonsAdapter.Application.Interfaces.Services;
using DeclaredPersonsAdapter.Application.Requests.DeclaredPersonAnalyser;
using DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;
using DeclaredPersonsAnalyzer.Models;
using DeclaredPersonsAnalyzer.Validations.DeclaredPersonAnalyserCmdArguments;
using Newtonsoft.Json;
using Shared.Wrapper;
using Shared.Wrapper.Interfaces;

namespace DeclaredPersonsAnalyzer.CmdControllers;

public class DeclaredPersonsAnalyzerController : IDeclaredPersonsAnalyzerController
{
    private readonly DeclaredPersonAnalyzerCmdArgumentsValidator _validator;
    private readonly IDeclaredPersonODataService _declaredPersonODataService;
    private readonly IMapper _mapper;

    public DeclaredPersonsAnalyzerController(
        DeclaredPersonAnalyzerCmdArgumentsValidator validator,
        IDeclaredPersonODataService declaredPersonODataService,
        IMapper mapper
        )
    {
        _validator = validator;
        _declaredPersonODataService = declaredPersonODataService;
        _mapper = mapper;
    }

    public async Task<IResult> Execute(DeclaredPersonAnalyzerCmdArguments cmdArguments)
    {
        var reqValidationResult = await _validator.ValidateAsync(cmdArguments);
        if (!reqValidationResult.IsValid)
        {
            return await Result.FailAsync(reqValidationResult.Errors);
        }

        cmdArguments.Out = FixProvidedOutFileName(cmdArguments);

        var request = _mapper.Map<DeclaredPersonAnalyserOptionsRequest>(cmdArguments);

        request.DeclaredPersonsGroupingType = cmdArguments.Group == null
            ? null
            : DeclaredPersonsGroupingTypeMethods.GetEnumFromDescription(cmdArguments.Group);

        var response = await _declaredPersonODataService.GetGroupedSummary(request);

        if (!response.Succeeded)
            return await Result.FailAsync();

        DisplayDataInTable(request, response);

        var json = JsonConvert.SerializeObject(new
        {
            data = response.Data,
            summary = new
            {
                response.Max,
                response.Min,
                response.Average,
                response.MaxDrop,
                response.MaxIncrease,
            }

        }, Formatting.Indented,
            new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });

        await File.WriteAllTextAsync(cmdArguments.Out, json);

        return await Result.SuccessAsync();
    }

    private string FixProvidedOutFileName(DeclaredPersonAnalyzerCmdArguments cmdArguments)
    {
        if (cmdArguments.Out == null)
        {
            return "res.json";
        }

        return cmdArguments.Out.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase)
            ? cmdArguments.Out
            : cmdArguments.Out + ".json";
    }

    private static void DisplayDataInTable(DeclaredPersonAnalyserOptionsRequest request, SummaryResult<GetDeclaredPersonGroupedResponse> res)
    {
        var headers = new List<ColumnHeader>()
        {
            new("district_name"),
        };

        if (request.IsGroupingByYearIncluded())
            headers.Add(new ColumnHeader("year"));

        if (request.IsGroupingByMonthIncluded())
            headers.Add(new ColumnHeader("month"));

        if (request.IsGroupingByDayIncluded())
            headers.Add(new ColumnHeader("day"));

        headers.Add(new ColumnHeader("value"));
        headers.Add(new ColumnHeader("change"));

        Table table = new Table(headers.ToArray());

        res.Data.ForEach(d =>
        {
            if (request.IsGroupingByYearIncluded() && request.IsGroupingByMonthIncluded() && request.IsGroupingByDayIncluded())
            {
                table.AddRow(d.DistrictName, d.Year, d.Month, d.Day, d.Value, d.Change);
            }
            else if (request.IsGroupingByYearIncluded() && request.IsGroupingByMonthIncluded())
            {
                table.AddRow(d.DistrictName, d.Year, d.Month, d.Value, d.Change);
            }
            else if (request.IsGroupingByYearIncluded() && request.IsGroupingByDayIncluded())
            {
                table.AddRow(d.DistrictName, d.Year, d.Day, d.Value, d.Change);
            }
            else if (request.IsGroupingByMonthIncluded() && request.IsGroupingByDayIncluded())
            {
                table.AddRow(d.DistrictName, d.Month, d.Day, d.Value, d.Change);
            }
            else if (request.IsGroupingByMonthIncluded())
            {
                table.AddRow(d.DistrictName, d.Month, d.Value, d.Change);
            }
            else if (request.IsGroupingByYearIncluded())
            {
                table.AddRow(d.DistrictName, d.Year, d.Value, d.Change);
            }
            else
                table.AddRow(d.DistrictName, d.Day, d.Value, d.Change);
        });

        table.Config = TableConfiguration.MySqlSimple(); // Sets table formatting

        Console.Write(table.ToString());

        Console.WriteLine($"\nMax:\t\t {res.Max}");
        Console.WriteLine($"Min:\t\t {res.Min}");
        Console.WriteLine($"Average:\t\t {res.Average}\n\n");

        if (res.MaxDrop != null)
            Console.WriteLine($"Max drop:\t {res.MaxDrop.Value} {res.MaxDrop.Group}");

        if (res.MaxIncrease != null)
            Console.WriteLine($"Max Increase:\t {res.MaxIncrease.Value} {res.MaxIncrease.Group}");
    }
}

public interface IDeclaredPersonsAnalyzerController
{
    Task<IResult> Execute(DeclaredPersonAnalyzerCmdArguments request);
}