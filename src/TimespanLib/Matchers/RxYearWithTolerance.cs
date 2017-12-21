using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace TimespanLib.Rx
{
    public class YearWithTolerance : Matcher<IYearSpan> // AbstractYearSpanParser
    {        
        // format pattern as (?:pattern) or (?<groupname>pattern)
        public static string Pattern(EnumLanguage language = EnumLanguage.NONE, string groupname = "")
        {
            return group(GetPattern(language), groupname);
        }

        private static string GetPattern(EnumLanguage language = EnumLanguage.NONE)
        {            
            string[] patterns = 
            {
                String.Concat(
                    START,                                               
                    maybe(DateCirca.Pattern(language) + SPACE),        
                    group(@"\d+", "year"),
                    @"\s*\±\s*",
                    group(@"\d+", "tolerance"), 
                    END
                ),
                String.Concat(
                    START,                                               
                    maybe(DateCirca.Pattern(language) + SPACE),        
                    group(@"\d+", "year"),
                    maybe(SPACE),
                    group(@"[+-]\d+", "tolerance1"), 
                    maybe(SPACE),
                    group(@"[+-]\d+", "tolerance2"),
                    END
                ),
                String.Concat(
                    START,                                               
                    maybe(DateCirca.Pattern(language) + SPACE),        
                    group(@"\d+", "year"),
                    maybe(SPACE),
                    group(@"[+-]\d+", "tolerance"),                 
                    END
                )
                // @"^(?:c(?:irca|\.|)\s)?(?<year>\d+)\s*\±\s*(?<tolerance>\d+)$",
                // @"^(?:c(?:irca|\.|)\s)?(?<year>\d+)\s*(?<tolerance>[+-]\d+)$",
                // @"^(?:c(?:irca|\.|)\s)?(?<year>\d+)\s*(?<tolerance1>[+-]\d+)\s*(?<tolerance2>[+-]\d+)$"
            };

            return oneof(patterns); // (?:pattern1|pattern2|pattern3)
        }

        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            return Regex.IsMatch(input.Trim(), Pattern(language), options);
        }

        public static IYearSpan Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            string pattern;
            Match m;                        
            
            // look for plusminus symbol tolerance pattern e.g. "1540±9" => { min: 1540, max: 1549, label: "1540+9" } 
            // pattern = @"^(?:c(?:irca|\.|)\s)?(?<year>\d+)\s*\±\s*(?<tolerance>\d+)$";
            pattern = String.Concat(
                START,                                               
                maybe(DateCirca.Pattern(language) + SPACE),        
                group(@"\d+", "year"),
                @"\s*\±\s*",
                group(@"\d+", "tolerance"), 
                END
            );
            m = Regex.Match(input.Trim(), pattern, options);
            if (m.Success)
            {
                int year = 0;
                int tolerance = 0;
                int.TryParse(m.Groups["year"].Value, out year);
                int.TryParse(m.Groups["tolerance"].Value, out tolerance);
                return new YearSpan(year - tolerance, year + tolerance, input, "RxYearWithTolerance(1)");
            }

            // look for plus minus tolerance pattern e.g. "1540+9-5" => { min: 1535, max: 1549, label: "1540+9-5" } 
            //pattern = @"^(?:c(?:irca|\.|)\s)?(?<year>\d+)\s*(?<tolerance1>[+-]\d+)\s*(?<tolerance2>[+-]\d+)$";
            pattern = String.Concat(
                START,                                               
                maybe(DateCirca.Pattern(language) + SPACE),        
                group(@"\d+", "year"),
                maybe(SPACE),
                group(@"[+-]\d+", "tolerance1"), 
                maybe(SPACE),
                group(@"[+-]\d+", "tolerance2"),
                END
            );
            m = Regex.Match(input.Trim(), pattern, options);
            if (m.Success)
            {
                int year = 0;
                int tolerance1 = 0;
                int tolerance2 = 0;
                int.TryParse(m.Groups["year"].Value, out year);
                int.TryParse(m.Groups["tolerance1"].Value, out tolerance1);
                int.TryParse(m.Groups["tolerance2"].Value, out tolerance2);
                return new YearSpan(year + tolerance1, year + tolerance2, input, "RxYearWithTolerance(2)");
            }

            // look for single tolerance pattern e.g. "1540+9" => { min: 1540, max: 1549, label: "1540+9" } 
            //pattern = @"^(?:c(?:irca|\.|)\s)?(?<year>\d+)\s*(?<tolerance>[+-]\d+)$";
            pattern = String.Concat(
                START,                                               
                maybe(DateCirca.Pattern(language) + SPACE),        
                group(@"\d+", "year"),
                maybe(SPACE),
                group(@"[+-]\d+", "tolerance"),                 
                END
            );
            m = Regex.Match(input.Trim(), pattern, options);
            if (m.Success)
            {
                int year = 0;
                int tolerance = 0;
                int.TryParse(m.Groups["year"].Value, out year);
                int.TryParse(m.Groups["tolerance"].Value, out tolerance);
                return new YearSpan(year, year + tolerance, input, "RxYearWithTolerance(3)");
            }
            
            // if we reach here nothing matched
            return null;
        }
    }
}