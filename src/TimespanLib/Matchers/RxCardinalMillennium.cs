using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace TimespanLib.Rx
{
    public class CardinalMillennium: Matcher<IYearSpan> // AbstractYearSpanParser
    {
        private static string GetPattern(EnumLanguage language = EnumLanguage.NONE)
        {
            string pattern = "";
            switch (language)
            {
                case EnumLanguage.CY:
                {
                    pattern = String.Concat(
                        START,
                        maybe(DateCirca.Pattern(language) + SPACE),
                        maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE),   
                        group(@"\d+", "millennium"),
                        oneof(new string[] { "af", @"a?il", "ain", "ydd", @"f?ed", "eg" }),
                        SPACE,
                        "mileniwm",
                        maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),
                        END
                   );
                   break;
                }
                case EnumLanguage.IT:
                    // ^(?:(?:C(?:\.|irca)|Intorno al)\s)?(?<millennium>[MCDLXVI]+)\s?(?:esimo|mo|°|º)?\smillennio(?:\s(?<suffix>a\.?C\.?|d\.?C\.?))?$
                    pattern = String.Concat(
                        START,
                        maybe(DateCirca.Pattern(language) + SPACE),
                        maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE),   // (?:(?:C(?:\.|irca)|Intorno al)\s)?
                        oneof(new string[] {
                            String.Concat(                                
                                "millennio",
                                SPACE,
                                oneof(new string[]{ROMAN, @"\d+"}, "millennium"),                                
                                maybe(SPACE),
                                maybe(oneof(new string[] { "esimo", "mo", "°", "º" })) // (?:esimo|mo|°|º)?                            
                            ),
                            String.Concat(
                                oneof(new string[]{ROMAN, @"\d+"}, "millennium"),
                                maybe(SPACE),
                                maybe(oneof(new string[] { "esimo", "mo", "°", "º" })), // (?:esimo|mo|°|º)?     
                                "millennio"                                     
                            )                                
                        }),
                        maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")), // (?:\s(?<suffix>a\.?C\.?|d\.?C\.?))?
                        END                                                  // $
                    );                   
                    break;
                default:
                    // ^(?:(?:C(?:\.|irca)|around|approx(?:\.|imately))\s)?(?<millennium>\d+)(?:st|nd|rd|th)\smillennium(?:\s(?<suffix>AD|BC|CE|BP))?$
                    pattern = String.Concat(
                        START,
                        maybe(DateCirca.Pattern(language) + SPACE),
                        maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE),   // (?:(?:C(?:\.|irca)|around|approx(?:\.|imately))\s)?
                        group(@"\d+", "millennium"),                            // (?<millennium>\d+)
                        oneof(new string[] { "st", "nd", "rd", "th" }),         // (?:st|nd|rd|th)
                        SPACE,                                                  // \s
                        "millennium",                                           // millennium  
                        maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")), // (?:\s(?<suffix>AD|BC|CE|BP))?
                        END                                                     // $
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
            return Regex.IsMatch(input.Trim(), Pattern(language), options);
        }   
        
        // input: "1st millennium" "I millennio a.C." (with language parameter)
        // output: { min: 1, max: 1000, label: "1st millennium" } 
        public static IYearSpan Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {                
            string pattern = Pattern(language);
            Match m = Regex.Match(input, pattern, options);      
            // no patterns matched?
            if (!m.Success) return null;

            // if we reach here we matched so get the represented year span 
            string s = m.Groups["millennium"] != null ? m.Groups["millennium"].Value.Trim() : "0";
            int millennium = Regex.IsMatch(s, ROMAN) ? RomanToNumber.Parse(s) : Int32.Parse(s);
            EnumDatePrefix prefix = m.Groups["prefix"] != null ? Lookup<EnumDatePrefix>.Match(m.Groups["prefix"].Value, language) : EnumDatePrefix.NONE;  
            EnumDateSuffix suffix = m.Groups["suffix"] != null ? Lookup<EnumDateSuffix>.Match(m.Groups["suffix"].Value, language) : EnumDateSuffix.NONE;

            IYearSpan span = getMillenniumYearSpan(millennium, prefix, suffix);
            span.label = input.Trim();
            span.note = "RxOrdinalNumberedMillennium";
            return span; 
        }    
    }
}