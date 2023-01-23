using AutoMapper;
using DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;
using EPakapojumiDataServiceContext;

namespace DeclaredPersonsAdapter.Infrastructure.Mappings;

public class DeclaredPersonMappingProfile : Profile
{
    public DeclaredPersonMappingProfile()
    {
        CreateMap<DeclaredPersons, GetDeclaredPersonResponse>()
            .ForMember(d => d.Day, o => o.MapFrom(s => s.day))
            .ForMember(d => d.Month, o => o.MapFrom(s => s.month))
            .ForMember(d => d.Year, o => o.MapFrom(s => s.year))
            .ForMember(d => d.Id, o => o.MapFrom(s => s.id))
            .ForMember(d => d.DistrictId, o => o.MapFrom(s => s.district_id))
            .ForMember(d => d.DistrictName, o => o.MapFrom(s => s.district_name))
            .ForMember(d => d.Value, o => o.MapFrom(s => s.value));

    }
}
