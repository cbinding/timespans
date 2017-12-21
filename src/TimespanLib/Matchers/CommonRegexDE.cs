using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Timespans.CommonRegex
{
    public class de 
    {
        // Month names (German)
        private static string[] monthnamepatterns = new string[] { 
            @"J(?:a|ä)n(?:\.|uar)?",    // Jan | Jän | Jan. | Jän. | Januar | Jänuar
            @"Feb(?:\.|ruar)?",     // Feb | Feb. | Februar
            @"März",                // März
            @"Apr(?:\.|il)?",       // Apr | Apr. | April
            @"Mai",                 // Mai
            @"Juni",                // Juni
            @"Juli",                // Juli
            @"Aug(?:\.|ust)?",      // Aug | Aug. | August
            @"Sept(?:\.|ember)?",   // Sept | Sept. | September
            @"Okt(?:\.|ober)?",     // Okt | Okt. | Oktober
            @"Nov(?:\.|ember)?",    // Nov | Nov. | November        
            @"Dez(?:\.|ember)?"     // Dez | Dez. | Dezember
        };
        public static EnumMonth parseMonthName(string input)
        {
            RegexOptions options = RegexOptions.IgnoreCase;
            input = input.Trim();

            if (Regex.IsMatch(input, monthnamepatterns[0], options))
                return EnumMonth.JAN;
            else if (Regex.IsMatch(input, monthnamepatterns[1], options))
                return EnumMonth.FEB;
            else if (Regex.IsMatch(input, monthnamepatterns[2], options))
                return EnumMonth.MAR;
            else if (Regex.IsMatch(input, monthnamepatterns[3], options))
                return EnumMonth.APR;
            else if (Regex.IsMatch(input, monthnamepatterns[4], options))
                return EnumMonth.MAY;
            else if (Regex.IsMatch(input, monthnamepatterns[5], options))
                return EnumMonth.JUN;
            else if (Regex.IsMatch(input, monthnamepatterns[6], options))
                return EnumMonth.JUL;
            else if (Regex.IsMatch(input, monthnamepatterns[7], options))
                return EnumMonth.AUG;
            else if (Regex.IsMatch(input, monthnamepatterns[8], options))
                return EnumMonth.SEP;
            else if (Regex.IsMatch(input, monthnamepatterns[9], options))
                return EnumMonth.OCT;
            else if (Regex.IsMatch(input, monthnamepatterns[10], options))
                return EnumMonth.NOV;
            else if (Regex.IsMatch(input, monthnamepatterns[11], options))
                return EnumMonth.DEC;
            else
                return EnumMonth.NONE;
        }

        // Ordinals (German)
        private static string[] named_ordinals = new string[] { 
            @"erste",               // first
            @"zweite",              // second
            @"dritte",              // third
            @"vierte",              // fourth
            @"fünfte",              // fifth
            @"sechste",             // sixth
            @"siebte",              // seventh
            @"achte",               // eighth
            @"neunte",              // ninth
            @"zehnte",              // tenth
            @"elfte",               // eleventh
            @"zwölfte",             // twelfth
            @"dreizehnte",          // thirteenth
            @"vierzehnte",          // fourteenth
            @"fünfzehnte",          // fifteenth
            @"sechzehnte",          // sixteenth
            @"siebzehnte",          // seventeenth
            @"achtzehnte",          // eighteenth
            @"neunzehnte",          // nineteenth
            @"zwanzig",             // twentieth
            @"einundzwanzigste(?:r|s|n)", // twenty first
            @"zwanzig secunde",     // twenty second
            @"dreiundzwanzigster",  // twenty third
            @"vierundzwanzig",      // twenty fourth
            @"fünfundzwanzigster",  // twenty fifth
            @"sechsundzwanzig",     // twenty sixth
            @"siebenundzwanzigster",    // twenty seventh
            @"achtundzwanzig",      // twenty eighth
            @"neunundzwanzigste",   // twenty ninth
            @"Dreißigste",          // thirtieth
            @"(?:Dreißig zuerst|einunddreißigsten)",    // thirty first
        };

        private static string numeric_ordinals = Rx.group(@"\d+", "ordinal") + @"\."; // (?<ordinal>\d+)\.
        
        public static readonly string[] seasonnamepatterns = new string[] {
            @"Frühling",   // Spring     
            @"Sommer",     // Summer 
            @"Herbst",     // Autumn  
            @"Winter"      // Winter           
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