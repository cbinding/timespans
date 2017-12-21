using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace TimespanLib.Rx
{
    // Used when all other matchers failed - just look for any numbers in the string
    public class LastChance : Matcher<IYearSpan>
    {
        public static string Pattern(EnumLanguage language = EnumLanguage.NONE, string groupname = "")
        {
            // whole numbers anywhere within in the input string 
            // (not considering roman numeral centuries here)
            return @"(?<number>\d+)";
        }

        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            return Regex.IsMatch(input.Trim(), Pattern(language), options);            
        }

        public static IYearSpan Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            MatchCollection mc = Regex.Matches(input.Trim(), Pattern(language), options);
            
            int yearMin = 0;
            int yearMax = 0;

            if (mc.Count == 0) return null;
            if (mc.Count > 0)
                int.TryParse(mc[0].Value, out yearMin);
            if (mc.Count > 1)
                int.TryParse(mc[1].Value, out yearMax);
            else
                yearMax = yearMin;

            return new YearSpan(yearMin, yearMax, input, "RxLastChance");

        }
    }
}