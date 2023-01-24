using DeclaredPersonsAdapter.Application.Requests.DeclaredPersonAnalyser;
using DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;
using DeclaredPersonsAnalyzer.Validations.DeclaredPersonAnalyser;
using Shared.Wrapper;
using Shared.Wrapper.Interfaces;

namespace DeclaredPersonsAnalyzer.CmdControllers;

public class DeclaredPersonsAnalyzerController : IDeclaredPersonsAnalyzerController
{
    private readonly DeclaredPersonAnalyserOptionsRequestValidator _validator;

    public DeclaredPersonsAnalyzerController(DeclaredPersonAnalyserOptionsRequestValidator validator)
    {
        _validator = validator;
    }

    public async Task<IResult> Execute(DeclaredPersonAnalyserOptionsRequest request)
    {
        var reqValidationResult = await _validator.ValidateAsync(request);
        if (!reqValidationResult.IsValid)
        {
            return await Result.FailAsync(reqValidationResult.Errors);
        }
    }
}

public interface IDeclaredPersonsAnalyzerController
{
    Task<IResult> Execute(DeclaredPersonAnalyserOptionsRequest request);
}