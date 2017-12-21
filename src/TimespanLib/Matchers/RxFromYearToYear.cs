using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TimespanLib.Rx
{
    public class FromYearToYear : Matcher<IYearSpan> // AbstractYearSpanParser
    {       
        // format patterns as (?:p1|p2|p3) or (?<groupname>p1|p2|p3)
        public static string Pattern(EnumLanguage language = EnumLanguage.NONE, string groupname = "")
        {
            return group(GetPattern(language), groupname);
        }

        private static string GetPattern(EnumLanguage language = EnumLanguage.NONE)
        {            
            string pattern ="";

            switch (language)
            {
                case EnumLanguage.IT:
                {
                    // ^(?:(?:Intorno\sal|circa)\s)?(?<yearMin>\d{3,})(?:\s*[;\-\/]\s*)(?<yearMax>\d{2,})\s*(?<suffix>a\.?C\.?|d\.?C\.?)?$
                    pattern = String.Concat(
                        START,                                                  // ^
                        maybe(oneof(new string[]{"tra lo", "in"}) + SPACE),     // (?:(?:tra lo|in)\s)?
                        maybe(DateCirca.Pattern(language) + SPACE),             // (?:(?:Intorno\sal|circa)\s)?
                        group(@"\d+", "yearMin"),                   // (?<yearMin>\d{3,})
                        oneof(new string[] { @"\s*[\;\-\/]\s*", @"\sed?\slo\s" }),   // separator
                        group(@"\d+", "yearMax"),                   // (?<yearMax>\d{3,})
                        maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),  // (?:\s(?<suffix>a\.?C\.?|d\.?C\.?))?
                        END                                                     // $
                    );
                    break;
                }
                default:
                {
                    // ^(?:c(?:\.|irca)?)?\s*(?<yearMin>\d{1,})(?:\s*[;\-\/]\s*)(?<yearMax>\d{2,})\s*(?<suffix>AD|BC|CE|BP)?\s*\??$
                    pattern = String.Concat(
                        START,       
                        maybe(DateCirca.Pattern(language) + maybe(SPACE)),
                        oneof(new string[]{
                            String.Concat(
                                maybe(oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix1")),
                                maybe(SPACE),
                                group(@"\d+", "yearMin") 
                            ),
                            String.Concat(                               
                                group(@"\d+", "yearMin"),
                                maybe(SPACE),
                                maybe(oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix1"))
                            )
                       }),
                       @"\s*[\;\-\/]\s*", // separators
                       oneof(new string[]{
                            String.Concat(
                                maybe(oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix2")),
                                maybe(SPACE),
                                group(@"\d+", "yearMax") 
                            ),
                            String.Concat(                               
                                group(@"\d+", "yearMax"),
                                maybe(SPACE),
                                maybe(oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix2"))
                            ) 
                       }),
                       END 
                    );
                    break;
                }
            }
            return pattern;
        }

        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            return Regex.IsMatch(input.Trim(), GetPattern(language), options);
        }
        
        // input: "1521-1527" or "1521/1527" or "c.1521/1527 AD" or "1521-1527 AD"
        // output: { min: 1521, max: 1527, label: "1521-1527" }
        public static IYearSpan Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            Match m = Regex.Match(input.Trim(), GetPattern(language), options);
            if (!m.Success) return null;

            int yearMin = 0;
            int yearMax = 0;
            
            int.TryParse(m.Groups["yearMin"].Value, out yearMin);
            int.TryParse(m.Groups["yearMax"].Value, out yearMax);

            EnumDateSuffix suffix1 = m.Groups["suffix1"] != null ? Lookup<EnumDateSuffix>.Match(m.Groups["suffix1"].Value, language) : EnumDateSuffix.NONE;
            EnumDateSuffix suffix2 = m.Groups["suffix2"] != null ? Lookup<EnumDateSuffix>.Match(m.Groups["suffix2"].Value, language) : EnumDateSuffix.NONE;

            if (suffix1 == EnumDateSuffix.NONE && suffix2 != EnumDateSuffix.NONE)
                suffix1 = suffix2;
            if (suffix2 == EnumDateSuffix.NONE && suffix1 != EnumDateSuffix.NONE)
                suffix2 = suffix1;
            
            // handling possible short max year here e.g. "1560-89 AD"
            if (yearMax < yearMin && suffix1 != EnumDateSuffix.BC)
            {
                if (yearMax <= 9)
                    yearMax = yearMin - (yearMin % 10) + yearMax;
                else if (yearMax >= 10 && yearMax <= 99)
                    yearMax = yearMin - (yearMin % 100) + yearMax;
            }

            switch (suffix1)
            {
                case EnumDateSuffix.BC:
                case EnumDateSuffix.BCE: 
                    yearMin *= -1;                     
                    break;
                case EnumDateSuffix.BP: 
                    yearMin = PRESENT - yearMin;                   
                    break;
            }

            switch (suffix2)
            {
                case EnumDateSuffix.BC:
                case EnumDateSuffix.BCE:
                    yearMax *= -1;
                    break;
                case EnumDateSuffix.BP:
                    yearMax = PRESENT - yearMax;
                    break;
            }
            
            return new YearSpan(Math.Min(yearMin, yearMax), Math.Max(yearMin, yearMax), input, "RxFromYearToYear");
        }
    }
}
