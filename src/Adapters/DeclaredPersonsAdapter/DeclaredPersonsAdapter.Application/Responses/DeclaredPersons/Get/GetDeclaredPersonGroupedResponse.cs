using DeclaredPersonsAdapter.Application.Enums;

namespace DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;

public class GetDeclaredPersonGroupedResponse
{
    public int? Year { get; set; }

    public int? Month { get; set; }

    public int? Day { get; set; }

    public int Value { get; set; }

    public int Change { get; set; }

    public int DistrictId { get; set; }

    public string DistrictName { get; set; } = string.Empty;

    public DeclaredPersonsGroupingType DeclaredPersonsGroupingType { get; set; }

    public string GroupFullName
    {
        get
        {
            var res = "";

            if (Year != null)
                res += $"y({Year}) ";

            if (Month != null)
                res += $"m({Month}) ";

            if (Day != null)
                res += $"d({Day})";

            return res;
        }
    }
}
