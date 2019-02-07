using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace TimespanLib.Rx
{
    public class OrdinalMillennium : Matcher<IYearSpan>
    {
        private static string GetPattern(EnumLanguage language = EnumLanguage.NONE)
        {
            string pattern = "";
            switch (language)
            {
                case EnumLanguage.CY:
                    pattern = String.Concat(
                        START,
                        maybe(DateCirca.Pattern(language) + SPACE),
                        maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE),
                        oneof(Lookup<EnumOrdinal>.Patterns(language), "ordinal"),
                        SPACE,
                        "mileniwm",
                        maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),
                        END
                    );
                    break;
                case EnumLanguage.FR:
                    pattern = String.Concat(
                       START,
                       maybe(DateCirca.Pattern(language) + SPACE),
                       maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE),
                       oneof(Lookup<EnumOrdinal>.Patterns(language), "ordinal"),
                       SPACE,
                       "millénaire",
                       maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),
                       END
                   );
                    break;
                case EnumLanguage.IT:
                    pattern = String.Concat(
                       START,                                               // ^
                       maybe(DateCirca.Pattern(language) + SPACE), // (?:(?:C(?:\.|irca)|Intorno al)\s)?
                       maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE),
                       oneof(new string[]{
                            @"millennio\s" + oneof(Lookup<EnumOrdinal>.Patterns(language), "ordinal"), 
                            oneof(Lookup<EnumOrdinal>.Patterns(language), "ordinal") + @"\smillennio" 
                       }),
                       maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")), // (?:\s(?<suffix>a\.?C\.?|d\.?C\.?))?
                      END                                                  // $
                   );
                    break;
                default:
                    pattern = String.Concat(
                        START,                                               // ^
                        maybe(DateCirca.Pattern(language) + SPACE), 
                        maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE),   // (?:
                        oneof(Lookup<EnumOrdinal>.Patterns(language), "ordinal"),
                        SPACE,
                        "millennium",
                        maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),
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



        // "THIRD CENTURY AD"
        public static IYearSpan Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {           
            string pattern = GetPattern(language);

            Match m = Regex.Match(input.Trim(), pattern, options);
            if (!m.Success) return null;

            int millenniumNo = m.Groups["ordinal"] != null ? (int)Lookup<EnumOrdinal>.Match(m.Groups["ordinal"].Value, language) : 0;
            EnumDatePrefix prefix = m.Groups["prefix"] != null ? Lookup<EnumDatePrefix>.Match(m.Groups["prefix"].Value, language) : EnumDatePrefix.NONE;
            EnumDateSuffix suffix = m.Groups["suffix"] != null ? Lookup<EnumDateSuffix>.Match(m.Groups["suffix"].Value, language) : EnumDateSuffix.NONE;

            IYearSpan span = getMillenniumYearSpan(millenniumNo, prefix, suffix);
            span.label = input.Trim();
            span.note = "RxOrdinalMillennium";
            return span;

        }

        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            return Regex.IsMatch(input.Trim(), GetPattern(language), options);
        }
    }
}