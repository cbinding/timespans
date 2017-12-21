using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TimespanLib.Rx
{
    public class MonthYear : Matcher<IYearSpan> 
    {        
        public static string Pattern(EnumLanguage language = EnumLanguage.NONE, string groupname = "")
        {
            return group(GetPattern(language), groupname);
        }

        private static string GetPattern(EnumLanguage language = EnumLanguage.NONE)
        {
            string pattern = String.Concat(
                START,
                maybe(DateCirca.Pattern(language) + SPACE),
                oneof(Lookup<EnumMonth>.Patterns(language), "month"),
                //Month.Pattern(language, "month"),
                SPACE,
                group(@"\d+", "year"),
                maybe(SPACE),
                maybe(oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),
                END
            );
            return pattern;
        }

        // input: "Jan 1857"
        // output: { min: 1857, max: 1857, label: "Jan 1857" } note: month currently ignored
        public static IYearSpan Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            Match m = Regex.Match(input.Trim(), GetPattern(language), options);
            if (!m.Success) return null;

            int year = 0;
            int.TryParse(m.Groups["year"].Value, out year);
            
            return new YearSpan(year, year, input, "RxMonthYear");
        }

        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            return Regex.IsMatch(input.Trim(), GetPattern(language), options);
        }
    }
}