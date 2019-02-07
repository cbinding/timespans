
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace TimespanLib.Rx
{
    public class Decade : Matcher<IYearSpan>
    {            
        // format pattern as (?:pattern)
        public static string Pattern(EnumLanguage language)
        {
            return group(GetPattern(language));
        }
        // format pattern as (?<groupname>pattern)
        public static string Pattern(EnumLanguage language, string groupname)
        {
            return group(GetPattern(language), groupname);
        }

        private static string GetPattern(EnumLanguage language)
        {
            string pattern = "";
            
            switch (language)
            {
                case EnumLanguage.CY:
                    // e.g. 1550au
                    pattern = String.Concat(
                        START,                       // ^
                        maybe(DateCirca.Pattern(language) + SPACE),                         
                        group(@"\d+0", "decade"),    // (?<decade>\d+0)
                        @"au",                         
                        END                         
                    );
                    break;
                case EnumLanguage.DE:
                    // ^(?:Um\s)?(?<decade>\d+0)\'?s$
                    // e.g. Um 1850's
                    pattern = String.Concat(
                        START,                       // ^
                        maybe(DateCirca.Pattern(language) + SPACE),  // (?:Um\s)?                        
                        group(@"\d+0", "decade"),    // (?<decade>\d+0)
                        @"\'?s",                        // \'?s
                        END                          // $
                    );
                    break;
                case EnumLanguage.FR:
                    // ^(?:Vers\s)?(?<decade>\d+0)\'?s$
                    // e.g. Vers 1850's
                    pattern = String.Concat(
                        START,                       // ^
                        maybe(DateCirca.Pattern(language) + SPACE),  // (?:Vers\s)?
                        @"les années\s",
                        group(@"\d+0", "decade"),    // (?<decade>\d+0)
                        END                          // $
                    );
                    break;                
                case EnumLanguage.NL:
                    // ^(?:Circa\s)?(?<decade>\d+0)\'?s$ (not right)
                    pattern = String.Concat(
                        START,                       // ^
                        maybe(DateCirca.Pattern(language) + SPACE),  // (?:Circa\s)
                        group(@"\d+0", "decade"),
                        @"\'?s",
                        END
                    );
                    break;
                case EnumLanguage.SV:
                    // ^(?:C(?:\.|irka)?\s)?(?<decade>\d+0)\-talet$
                    pattern = String.Concat(
                        START,
                        maybe(DateCirca.Pattern(language) + SPACE),
                        group(@"\d+0", "decade"),
                        @"\-talet",
                        END
                    );
                    break;
                case EnumLanguage.IT:
                    // ^(?:Intorno\sal|circa)?\s*decennio\s(?<decade>\d+[1-9]0)(?:esimo)?$
                    // e.g. "Intorno al decennio 1850esimo" => "around the decade of 1850's"
                    pattern = String.Concat(
                       START,
                       maybe(DateCirca.Pattern(language) + SPACE),
                       @"decennio\s",
                       group(@"\d+0", "decade"),
                       maybe("esimo"),
                       END
                    );
                    break;
                default:
                    // ^(?:c(?:irca|\.|)\s*)?(?<decade>\d+[1-9]0)\'?s$
                    // e.g. "1850s" "circa 1920's"
                    pattern = String.Concat(
                       START,
                       maybe(DateCirca.Pattern(language) + SPACE),
                       group(@"\d+0", "decade"),
                       @"\'?s",
                       END
                   );                  
                   break;
            }

            return pattern;
        }

        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {            
            return Regex.IsMatch(input.Trim(), GetPattern(language), options);
        }              

        // input: "1930s", "C. 1930s", "1930's", "decennio 1930"
        // output: { min: 1930, max: 1939 , label: "1930s" }                              
        public static IYearSpan Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {                          
            Match m = Regex.Match(input.Trim(), GetPattern(language), options);
            if (!m.Success) return null;

            int decade = 0;
            int.TryParse(m.Groups["decade"].Value, out decade);

            return new YearSpan(decade, decade + 9, input, "RxDecade");
        }

    }
}