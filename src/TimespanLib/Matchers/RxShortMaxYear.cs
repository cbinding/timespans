using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Timespans.Rx
{
    public class ShortMaxYear : Matcher<IYearSpan> // AbstractYearSpanParser
    {
        private static string GetPattern(EnumLanguage language = EnumLanguage.NONE)
        {
            string pattern = "";

            switch (language)
            {
                case EnumLanguage.IT:
                    // ^(?:(?:Intorno\sal|circa)\s)?(?<yearMin>\d{2,})(?:\s*[\;\-\/]\s*)(?<yearMax>\d{1,2})(?:\s(?<suffix>a\.?C\.?|d\.?C\.?))?$
                    pattern = String.Concat(
                       START,                                            // ^
                       maybe(DateCirca.Pattern(language) + SPACE), // (?:(?:Intorno\sal|circa)\s)?
                       group(@"\d{4,}", "yearMin"),                      // (?<yearMin>\d{2,})
                       @"\s*[\;\-\/]\s*",                                   // separators
                       group(@"\d{1,2}", "yearMax"),                     // (?<yearMax>\d{1,2})
                       maybe(SPACE + DateSuffix.Pattern(language, "suffix")), // (?:\s(?<suffix>a\.?C\.?|d\.?C\.?))?
                       END
                    );
                    break;
                default:
                    // ^(?:(?:C(?:\.|irca)|around|approx(?:\.|imately))\s)?(?<yearMin>\d{2,})(?:\s*[\;\-\/]\s*)(?<yearMax>\d{1,2})(?:\s(?<suffix>AD|BC|BP|CE))?$
                    pattern = String.Concat(
                      START,                                             // ^
                      maybe(DateCirca.Pattern(language) + SPACE),  // (?:(?:C(?:\.|irca)|around|approx(?:\.|imately))\s)?
                      group(@"\d{4,}", "yearMin"),                       // (?<yearMin>\d{2,})
                      @"\s*[\;\-\/]\s*",                                    // separators
                      group(@"\d{1,2}", "yearMax"),                      // (?<yearMax>\d{1,2})
                      maybe(SPACE + DateSuffix.Pattern(language, "suffix")), // (?:\s(?<suffix>AD|BC|BP|CE))?
                      END
                   );
                    break;
            }
            return pattern;
        }
       
        public static string Pattern(EnumLanguage language = EnumLanguage.NONE, string groupname = "")
        {
            return group(GetPattern(language), groupname);
        }

        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            return (Regex.IsMatch(input, Pattern(language), options));
        }

        // input: "1521-7" or "1521/7" or "c.1521/7 AD" or "1521-27 AD"
        // output: { min: 1521, max: 1527, label: "1521/7" }
        public static IYearSpan Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {                
            Match m = Regex.Match(input.Trim(), Pattern(language), options);
            if (!m.Success) return null;

            int yearMin = 0;
            int yearMax = 0;
            int.TryParse(m.Groups["yearMin"].Value, out yearMin);
            int.TryParse(m.Groups["yearMax"].Value, out yearMax);
            if(yearMax <=9)
                yearMax = yearMin - (yearMin % 10) + yearMax;
            else
                yearMax = yearMin - (yearMin % 100) + yearMax;

            EnumDateSuffix suffix = m.Groups["suffix"] != null ? DateSuffix.Match(m.Groups["suffix"].Value, language) : EnumDateSuffix.NONE;
                       
            switch (suffix)
            {
                case EnumDateSuffix.BC:
                    yearMin *= -1;
                    yearMax *= -1;
                    break;
                case EnumDateSuffix.BP:
                    yearMin = PRESENT - yearMin;
                    yearMax = PRESENT - yearMax;
                    break;
            }

            return new YearSpan(yearMin, yearMax, input, "RxShortMaxYear");
        }
    }
}
