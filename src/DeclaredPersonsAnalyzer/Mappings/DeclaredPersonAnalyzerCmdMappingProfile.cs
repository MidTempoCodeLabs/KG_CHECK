using AutoMapper;
using DeclaredPersonsAdapter.Application.Requests.DeclaredPersonAnalyser;
using DeclaredPersonsAnalyzer.Models;

namespace DeclaredPersonsAnalyzer.Mappings;

internal class DeclaredPersonAnalyzerCmdMappingProfile : Profile
{
    public DeclaredPersonAnalyzerCmdMappingProfile()
    {
        CreateMap<DeclaredPersonAnalyzerCmdArguments, DeclaredPersonAnalyserOptionsRequest>().ReverseMap();
    }
}
