using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Timespans.CommonRegex
{
    public class sv
    {
        // Month names (Swedish)
        private static string[] monthnames = new string[] {  
            @"Jan(?:\.|uari)?",     // Jan | Jan. | Januari
            @"Febr(?:\.|uari)?",    // Febr | Febr. | Februari
            @"Mars",                // Mars
            @"April",               // April
            @"Maj",                 // Maj
            @"Juni",                // Juni
            @"Juli",                // Juli
            @"Aug(?:\.|usti)?",     // Aug | Aug. | Augusti
            @"Sept(?:\.|ember)?",   // Sept | Sept. | September
            @"Okt(?:\.|ober)?",     // Okt | Okt. | Oktober        
            @"Nov(?:\.|ember)?",    // Nov | Nov. | November
            @"Dec(?:\.|ember)?"     // Dec | Dec. | December
        };

        // Ordinals (Swedish)
        private static string[] named_ordinals = new string[] { 
            @"första",          // first
            @"andra",           // second
            @"tredje",          // third
            @"fjärde",          // fourth
            @"femte",           // fifth
            @"sjätte",          // sixth
            @"sjunde",          // seventh
            @"åttonde",         // eighth
            @"nionde",          // ninth
            @"tionde",          // tenth
            @"elfte",           // eleventh
            @"tolfte",          // twelfth
            @"trettonde",       // thirteenth
            @"fjortonde",       // fourteenth
            @"femtonde",        // fifteenth
            @"sextonde",        // sixteenth
            @"sjuttonde",       // seventeenth
            @"artonde",         // eighteenth
            @"nittonde",        // nineteenth
            @"tjugonde",        // twentieth
            @"tjugoförsta"      // twenty first
        };

        public static readonly string[] seasonnamepatterns = new string[] {
            @"Vår",         // Spring
            @"Sommar",      // Summer
            @"Höst",        // Autumn
            @"Vinter"       // Winter
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