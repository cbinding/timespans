using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TimespanLib.Rx
{
    public class FromCenturyToCenturyOrdinal : Matcher<IYearSpan> // AbstractYearSpanParser
    {
        // format patterns as (?:p1|p2|p3) or (?<groupname>p1|p2|p3)
        public static string Pattern(EnumLanguage language = EnumLanguage.NONE, string groupname = "")
        {
            return group(GetPattern(language), groupname);
        }

        private static string GetPattern(EnumLanguage language = EnumLanguage.NONE)
        {
            string pattern = "";

            switch (language)
            {
                default:
                    {
                        // circa late eighth to early ninth century AD
                        pattern = String.Concat(
                            START,
                            maybe(DateCirca.Pattern(language) + SPACE),
                            maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix1") + SPACE),
                            oneof(Lookup<EnumOrdinal>.Patterns(language), "centuryMin"),
                            maybe(SPACE + "century"),
                            @"\s*(?:[e\;\-\/]|to|or)\s*",
                            maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix2") + SPACE),
                            oneof(Lookup<EnumOrdinal>.Patterns(language), "centuryMax"),
                            maybe(SPACE + "century"),
                            maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),
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

        // input: "VII-VI secolo" or "III-II secolo a.C."
        // output: { min: 601, max: 800, label: "VII-VI secolo" }
        public static IYearSpan Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            Match m = Regex.Match(input.Trim(), GetPattern(language), options);
            if (!m.Success) return null;

            int centuryMin = m.Groups["centuryMin"] != null ? (int)Lookup<EnumOrdinal>.Match(m.Groups["centuryMin"].Value, language) : 0;
            int centuryMax = m.Groups["centuryMax"] != null ? (int)Lookup<EnumOrdinal>.Match(m.Groups["centuryMax"].Value, language) : 0;
                      
            EnumDatePrefix prefix1 = m.Groups["prefix1"] != null ? Lookup<EnumDatePrefix>.Match(m.Groups["prefix1"].Value, language) : EnumDatePrefix.NONE;
            EnumDatePrefix prefix2 = m.Groups["prefix2"] != null ? Lookup<EnumDatePrefix>.Match(m.Groups["prefix2"].Value, language) : EnumDatePrefix.NONE;
            EnumDateSuffix suffix = m.Groups["suffix"] != null ? Lookup<EnumDateSuffix>.Match(m.Groups["suffix"].Value, language) : EnumDateSuffix.NONE;

            IYearSpan centuryMinSpan = getCenturyYearSpan(centuryMin, prefix1, suffix);
            IYearSpan centuryMaxSpan = getCenturyYearSpan(centuryMax, prefix2, suffix);

            return new YearSpan(Math.Min(centuryMinSpan.min, centuryMaxSpan.min), Math.Max(centuryMinSpan.max, centuryMaxSpan.max), input, "RxFromCenturyToCenturyOrdinal");
        }
    }
}