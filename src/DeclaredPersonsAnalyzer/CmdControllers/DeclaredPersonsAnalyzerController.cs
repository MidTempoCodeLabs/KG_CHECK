using AutoMapper;
using BetterConsoleTables;
using DeclaredPersonsAdapter.Application.Enums;
using DeclaredPersonsAdapter.Application.Interfaces.Services;
using DeclaredPersonsAdapter.Application.Requests.DeclaredPersonAnalyser;
using DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;
using DeclaredPersonsAnalyzer.Models;
using DeclaredPersonsAnalyzer.Validations.DeclaredPersonAnalyserCmdArguments;
using Newtonsoft.Json;
using Shared.Extensions;
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

        var request = _mapper.Map<DeclaredPersonAnalyserOptionsRequest>(cmdArguments);

        var res = await _declaredPersonODataService.GetGroupedSummary(request);

        if (!res.Succeeded)
            return await Result.FailAsync();

        DisplayDataInTable(request, res);

        var json = JsonConvert.SerializeObject(res, Formatting.Indented,
            new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });

        await File.WriteAllTextAsync(request.Out ?? "res.json", json);

        return await Result.SuccessAsync();
    }

    private static void DisplayDataInTable(DeclaredPersonAnalyserOptionsRequest request, SummaryResult<GetDeclaredPersonGroupedResponse> res)
    {
        var headers = new List<ColumnHeader>()
        {
            new("district_name"),
        };

        if (request.Group.Contains(DeclaredPersonsGroupingType.ByYear.GetDescription()))
            headers.Add(new ColumnHeader("year"));

        if (request.Group.Contains(DeclaredPersonsGroupingType.ByMonth.GetDescription()))
            headers.Add(new ColumnHeader("month"));

        if (request.Group.Contains(DeclaredPersonsGroupingType.ByDay.GetDescription()))
            headers.Add(new ColumnHeader("day"));

        headers.Add(new ColumnHeader("value"));
        headers.Add(new ColumnHeader("change"));

        Table table = new Table(headers.ToArray());

        res.Data.ForEach(d =>
        {
            if (d.Year != null && d.Month != null && d.Day != null)
                table.AddRow(d.DistrictName, d.Year, d.Month, d.Day, d.Value, d.Change);
            else if (d.Year != null && d.Month != null)
                table.AddRow(d.DistrictName, d.Year, d.Month, d.Value, d.Change);
            else if (d.Year != null && d.Day != null)
                table.AddRow(d.DistrictName, d.Year, d.Value, d.Change);
            else if (d.Month != null && d.Day != null)
                table.AddRow(d.DistrictName, d.Month, d.Value, d.Change);
            else if (d.Month != null)
                table.AddRow(d.DistrictName, d.Month, d.Value, d.Change);
            else if (d.Year != null)
                table.AddRow(d.DistrictName, d.Year, d.Value, d.Change);
            else
                table.AddRow(d.Day, d.Value, d.Change);
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