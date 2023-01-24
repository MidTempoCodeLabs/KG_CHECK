namespace Adapters.Shared.Constants;

public static class ODataSourceConstants
{
    public static class Epakalpojumi
    {
        public const string DataSourceUriString = "https://www.epakalpojumi.lv/odata/service";

        public const string DeclaredPersons = $"{DataSourceUriString}/DeclaredPersons";
        public const string BodyOfWater = $"{DataSourceUriString}/BodyOfWater";
        public const string KindergartenApp = $"{DataSourceUriString}/KindergartenApp";
        public const string GovernmentBankAccounts = $"{DataSourceUriString}/GovernmentBankAccounts";
        public const string MarriageRegister = $"{DataSourceUriString}/MarriageRegister";
        public const string District = $"{DataSourceUriString}/District";
    }
}
