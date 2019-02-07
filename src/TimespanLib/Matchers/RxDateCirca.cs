using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace TimespanLib.Rx
{
    public class DateCirca : Matcher<bool>
    {
        private static string[] patterns_cy = 
        {  
            @"C(?:\.|irca)",             // C. | Circa 
            @"tua\'r"
        };
        private static string[] patterns_de = 
        {
            @"C(?:\.|irca)",            // C. | Circa
            @"Um"            
        };
        private static string[] patterns_en = 
        {   
            @"C(?:\.|irca)?",            // C. | Circa
            @"around",                  // around
            @"approx(?:\.|imately)?"    // approx | approx. | approximately
        };
        private static string[] patterns_es = 
        {
            @"C(?:\.|irca)",            // C. | Circa
            @"Alrededor de(?:l año)?"   // Alrededor de | Alrededor del año
        };
        private static string[] patterns_fr = 
        {
            @"C(?:\.|irca)",            // C. | Circa
            @"(?:Vers le|vers)"
        };
        private static string[] patterns_it = 
        {
            @"C(?:\.|irca)",            // C. | Circa
            @"Intorno al?"              // Intorno a | Intorno al                
        };
        private static string[] patterns_nl = 
        {
            @"Circa"
        };
        private static string[] patterns_sv = 
        {
            @"C(?:\.|irka)"
        };

        // get regex patterns for the specified language
        private static string[] Patterns(EnumLanguage language = EnumLanguage.NONE)
        {
            switch (language)
            {
                case EnumLanguage.CY: return patterns_cy;
                case EnumLanguage.DE: return patterns_de;
                case EnumLanguage.ES: return patterns_es;
                case EnumLanguage.FR: return patterns_fr;
                case EnumLanguage.IT: return patterns_it;
                case EnumLanguage.NL: return patterns_nl;
                case EnumLanguage.SV: return patterns_sv;
                default: return patterns_en;
            }
        }
               
        // formats patterns as (?:p1|p2|p3) or (?<groupname>p1|p2|p3)
        public static string Pattern(EnumLanguage language = EnumLanguage.NONE, string groupname = "")
        {
            return oneof(Patterns(language), groupname);
        }

        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            return (Regex.IsMatch(input.Trim(), Pattern(language), options));
        }

        public static bool Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            return IsMatch(input, language);
        }
    }
}