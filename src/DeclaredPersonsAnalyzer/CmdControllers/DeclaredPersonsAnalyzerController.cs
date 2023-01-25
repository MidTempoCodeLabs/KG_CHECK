using AutoMapper;
using DeclaredPersonsAdapter.Application.Interfaces.Services;
using DeclaredPersonsAdapter.Application.Requests.DeclaredPersonAnalyser;
using DeclaredPersonsAnalyzer.Models;
using DeclaredPersonsAnalyzer.Validations.DeclaredPersonAnalyserCmdArguments;
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

        return await Result.SuccessAsync();
    }
}

public interface IDeclaredPersonsAnalyzerController
{
    Task<IResult> Execute(DeclaredPersonAnalyzerCmdArguments request);
}