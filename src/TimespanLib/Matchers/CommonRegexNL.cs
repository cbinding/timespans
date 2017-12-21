using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Timespans.CommonRegex
{
    public class nl
    {
        private static string[] circa = new string[] {
            @"v\.Chr\." // voor Christus (BC)
        };

        private static string[] dateprefix = new string[] {
            "VROEG",    // Early
            "MIDDEN",   // Mid
            "LAAT"      // Late
        };

        private static string[] datesuffix = new string[] {
            @"n\.Chr\."     // naar Christus (AD)
        };

        // Month names (Dutch)
        private static string[] monthnames = new string[] {   
            @"Jan(?:\.|uari)?",     // Jan | Jan. | Januari        
            @"Feb(?:\.|ruari)?",    // Feb | Feb. | Februari 
            @"Maart",               // Maart
            @"Apr(?:\.|il)?",       // Apr | Apr. | April
            @"Mei",                 // Mei
            @"Juni",                // Juni
            @"Juli",                // Juli
            @"Aug(?:\.|ustus)?",    // Aug | Aug. | Augustus
            @"Sept(?:\.|ember)?",   // Sept | Sept. | September
            @"O(?:k|c)t(?:\.|ober)?",     // Okt | Oct | Okt. | Oct. | Oktober
            @"Nov(?:\.|ember)?",    // Nov | Nov. | November
            @"Dec(?:\.|ember)?"     // Dec | Dec. | December
        };

        // Ordinals (Dutch)
        private static string[] named_ordinals = new string[] { 
            @"eerste",          // first
            @"tweede",          // second
            @"derde",           // third
            @"vierde",          // fourth
            @"vijfe",           // fifth
            @"zesde",           // sixth
            @"zevende",         // seventh
            @"achtse",          // eighth
            @"negende",         // ninth
            @"tiende",          // tenth
            @"elfde",           // eleventh
            @"twaalfde",        // twelfth
            @"dertiende",       // thirteenth
            @"veertiende",      // fourteenth
            @"vijftiende",      // fifteenth
            @"zestiende",       // sixteenth
            @"zeventiende",     // seventeenth
            @"achttiende",      // eighteenth
            @"negentiende",     // nineteenth
            @"twintigste"       // twentieth
        };

        private static string[] numeric_ordinals = new string[] { 
            @"\d+(?:e|ste|de)"
        };

        public static readonly string[] seasonnamepatterns = new string[] {
            @"De lente",    // Spring
            @"Zomer",       // Summer
            @"Herfst",      // Autumn
            @"Winter"       // Winter      
        };
        public static EnumSeason parseSeasonName(string input)
        {
            RegexOptions options = RegexOptions.IgnoreCase;
            input = input.Trim();

            if (Regex.IsMatch(input, seasonnamepatterns[0], options))
                return EnumSeason.SPRING;
            else if (Regex.IsMatch(input, seasonnamepatterns[1], options))
                return EnumSeason.SUMMER;
            else if (Regex.IsMatch(input, seasonnamepatterns[2], options))
                return EnumSeason.AUTUMN;
            else if (Regex.IsMatch(input, seasonnamepatterns[3], options))
                return EnumSeason.WINTER;
            else
                return EnumSeason.NONE;
        }

    }
}