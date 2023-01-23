using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeclaredPersonsAnalyzer.CommandLineHelpers.Constants;

internal static class CommandLineParamsConstants
{
    internal static class DeclaredPersonAnalyzer
    {
        public const string Source = "https://www.epakalpojumi.lv/odata/service/DeclaredPersons";
        public const int Limit = 100;
    }
}
