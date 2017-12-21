using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TimespanLib.Rx
{
    public class SeasonYear : Matcher<IYearSpan> //AbstractYearSpanParser
    {          
        private static string Pattern(EnumLanguage language = EnumLanguage.NONE, string groupname = "")           
        {
            // ^(?<season>Frühling|Sommer|Herbst|Winter)\s(?<year>\d+)$
            string pattern = String.Concat(
                START,                              // ^
                oneof(Lookup<EnumSeason>.Patterns(language), "season"), // (?<season>Frühling|Sommer|Herbst|Winter)
                SPACE,                              // \s
                group(@"\d+", "year"),              // (?<year>\d+)
                END                                 // $
            );
            return group(pattern, groupname);
        }
        
        // input: "Spring 1857"
        // output: { min: 1857, max: 1857, label: "Spring 1857" } 
        public static IYearSpan Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            Match m = Regex.Match(input.Trim(), Pattern(language), options);
            if (!m.Success) return null;

            int year = 0;
            int.TryParse(m.Groups["year"].Value, out year);

            return new YearSpan(year, input, "RxSeasonYear");
        }

        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            return Regex.IsMatch(input.Trim(), Pattern(language), options);
        }
    }
}