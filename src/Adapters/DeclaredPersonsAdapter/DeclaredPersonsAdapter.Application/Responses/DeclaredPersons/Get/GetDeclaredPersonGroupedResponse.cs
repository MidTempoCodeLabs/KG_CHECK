using DeclaredPersonsAdapter.Application.Enums;

namespace DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;

public class GetDeclaredPersonGroupedResponse
{
    public int? Year { get; set; }
    
    public int? Month  { get; set; }
    
    public int? Day  { get; set; }

    public int Value  { get; set; }

    public int Change { get; set; }

    public int DistrictId  { get; set; }
    
    public string DistrictName  { get; set; } = string.Empty;

    public DeclaredPersonsGroupingType DeclaredPersonsGroupingType { get; set; }
}
