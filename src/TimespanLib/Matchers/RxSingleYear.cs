using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;


namespace TimespanLib.Rx
{
    public class SingleYear : Matcher<IYearSpan> 
    {        
        // format pattern as (?:pattern) or (?<groupname>pattern)
        public static string Pattern(EnumLanguage language = EnumLanguage.NONE, string groupname = "")
        {
            return group(GetPattern(language), groupname);
        }

        private static string GetPattern(EnumLanguage language = EnumLanguage.NONE)
        {
            string pattern = "";
            switch (language) 
            {
                case EnumLanguage.IT:
                {
                    // ^(?:(?:dal?|in|nel)\s)?(?:(?:C(?:\.|irca)|Intorno al)\s)?(?<year>\d+)(?:(?<suffix>a\.?C\.?|d\.?C\.?|B\.?P\.?|C\.?E\.?))?$
                    pattern = String.Concat(
                        START,                                                      // ^
                        maybe(oneof(new string[]{@"dal?", "in", "nel"}) + SPACE),   // (?:(?:dal?|in|nel)\s)?
                        maybe(DateCirca.Pattern(language) + SPACE),                 // (?:(?:C(?:\.|irca)|Intorno al)\s)?
                        group(@"\d+", "year"),                                      // (?<year>\d+)
                        maybe(SPACE),                                               // (?:\s)?
                        maybe(oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),              // (?:(?<suffix>a\.?C\.?|d\.?C\.?|B\.?P\.?|C\.?E\.?))?
                        END                                                         // $
                    );
                    break;
                }
                default:
                {
                    // ^(?:(?:C(?:\.|irca)|around|approx(?:\.|imately)?)\s)?(?<year>\d+)(?:(?<suffix>B\.?C\.?|A\.?D\.?|B\.?P\.?|C\.?E\.?)\s)?$
                    pattern = String.Concat(    
                        START,                                                      // ^
                        maybe(DateCirca.Pattern(language)),                         // (?:C(?:\.|irca)|around|approx(?:\.|imately)?)?
                        maybe(SPACE),                                               // (?:\s)?
                        oneof(new string[]{
                            String.Concat(
                                group(@"\d+", "year"),                              // (?<year>\d+)
                                maybe(oneof(new string[]{@"\+", @"\?"})),           // approximation indicator                                
                                maybe(SPACE),                                       // (?:\s)?
                                maybe(oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix"))       // (?<suffix>B\.?C\.?|A\.?D\.?|B\.?P\.?|C\.?E\.?)?
                            ),
                            String.Concat(
                                maybe(oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),      // (?<suffix>B\.?C\.?|A\.?D\.?|B\.?P\.?|C\.?E\.?)?
                                maybe(SPACE),                                       // (?:\s)?                                
                                group(@"\d+", "year"),                               // (?<year>\d+)  
                                maybe(oneof(new string[]{@"\+", @"\?"}))
                            )
                        }),
                        END                                                         // $
                    );
                    break;
                }               
            }
            return pattern;
        }

        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            return Regex.IsMatch(input.Trim(), Pattern(language), options);
        } 

        // input: "1521", "C.1521", "C.1521 AD", "C.1521 BC"
        // output: { min: 1521, max: 1521, label: "C. 1521 AD" }
        public static IYearSpan Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            string pattern = GetPattern(language);
            Match m = Regex.Match(input.Trim(), pattern, options);
            if (!m.Success) return null;
            
            int year = 0;
            int.TryParse(m.Groups["year"].Value, out year);
            EnumDateSuffix suffix = m.Groups["suffix"] != null ? Lookup<EnumDateSuffix>.Match(m.Groups["suffix"].Value, language) : EnumDateSuffix.NONE;               
            switch (suffix)
            {
                case EnumDateSuffix.BC:
                case EnumDateSuffix.BCE: 
                    year *= -1; 
                    break;
                case EnumDateSuffix.BP: 
                    year = PRESENT - year; 
                    break;
            }
            return new YearSpan(year, input, "RxSingleYear");
        }
    }
}