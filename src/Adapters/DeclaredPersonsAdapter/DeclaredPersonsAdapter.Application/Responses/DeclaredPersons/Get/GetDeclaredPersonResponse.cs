namespace DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;

public class GetDeclaredPersonResponse
{
    /// <summary>
    /// Record identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The year the record was created
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// The month the record was created
    /// </summary>
    public int Month  { get; set; }

    /// <summary>
    /// The day the record was created
    /// </summary>
    public int Day  { get; set; }

    /// <summary>
    /// Number of declared persons
    /// </summary>
    public decimal Value  { get; set; }

    /// <summary>
    /// City id
    /// </summary>
    public int DistrictId  { get; set; }

    /// <summary>
    /// City name
    /// </summary>
    public string DistrictName  { get; set; } = string.Empty;
}
