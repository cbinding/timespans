using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TimespanLib.Rx
{
    public class FromCenturyToCentury : Matcher<IYearSpan> // AbstractYearSpanParser
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
                case EnumLanguage.CY:
                {
                    pattern = String.Concat(
                        START,
                        maybe(DateCirca.Pattern(language) + SPACE),
                        group(@"\d+", "centuryMin"),
                        oneof(new string[] { "af", "a?il", "ydd", "f?ed", "eg", "ain" }),
                        maybe(SPACE + "ganrif"),
                        @"\s*[\s\;\-\/]\s*",
                        group(@"\d+", "centuryMax"),
                        oneof(new string[] { "af", "a?il", "ydd", "f?ed", "eg", "ain" }),
                        SPACE,
                        "ganrif",
                        maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),
                        END
                    );
                    break;
                }
                case EnumLanguage.IT:
                {
                    pattern = String.Concat(
                        START,
                        maybe(@"tra(?:\slo)?" + SPACE),
                        maybe(DateCirca.Pattern(language) + SPACE),      // (?:(?:Intorno\sal|circa)\s)?
                        group(ROMAN, "centuryMin"),                      // (?<centuryMin>[MCDLXVI]+)
                        oneof(new string[]{@"\s*[e\;\-\/]\s*", @"\sed?\slo\s"}),   // separator
                        group(ROMAN, "centuryMax"),                     // (?<centuryMax>[MCDLXVI]+)
                        maybe(SPACE + @"sec(?:\.|olo)"),
                        maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),  // (?:\s(?<suffix>a\.?C\.?|d\.?C\.?))?
                        END
                    );
                    break;
                }
                default:
                {
                    pattern = String.Concat(
                        START,                                                  // ^
                        maybe(DateCirca.Pattern(language) + SPACE),
                        maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix1") + SPACE),   //                            
                        group(@"\d+", "centuryMin"),                        // (?<centuryMin>\d+)
                        oneof(new string[] { "st", "nd", "rd", "th" }),
                        @"\s*(?:[e\;\-\/]|to)\s*",                                      // separator
                        maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix2") + SPACE), 
                        group(@"\d+", "centuryMax"),                      // (?<centuryMax>\d+)
                        oneof(new string[] { "st", "nd", "rd", "th" }),
                        SPACE,
                        "century",                                         // century
                        maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")), // (?:\s(?<suffix>A\.?D\.?|B\.?C\.?))?
                        END                                             // $
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

            string century = "";
            int centuryMin = 0;
            int centuryMax = 0;
            
            century = m.Groups["centuryMin"].Value;
            centuryMin = Regex.IsMatch(century, ROMAN) ? RomanToNumber.Parse(century) : Int32.Parse(century);

            century = m.Groups["centuryMax"].Value;
            centuryMax = Regex.IsMatch(century, ROMAN) ? RomanToNumber.Parse(century) : Int32.Parse(century);
            
            EnumDatePrefix prefix1 = m.Groups["prefix1"] != null ? Lookup<EnumDatePrefix>.Match(m.Groups["prefix1"].Value, language) : EnumDatePrefix.NONE;
            EnumDatePrefix prefix2 = m.Groups["prefix2"] != null ? Lookup<EnumDatePrefix>.Match(m.Groups["prefix2"].Value, language) : EnumDatePrefix.NONE;
            EnumDateSuffix suffix = m.Groups["suffix"] != null ? Lookup<EnumDateSuffix>.Match(m.Groups["suffix"].Value, language) : EnumDateSuffix.NONE;
            
            IYearSpan centuryMinSpan = getCenturyYearSpan(centuryMin, prefix1, suffix);
            IYearSpan centuryMaxSpan = getCenturyYearSpan(centuryMax, prefix2, suffix); 
            
            return new YearSpan(Math.Min(centuryMinSpan.min, centuryMaxSpan.min), Math.Max(centuryMinSpan.max,centuryMaxSpan.max), input, "RxFromCenturyToCentury");
        }
    }
}
