using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Timespans.Rx
{
    public class DatePrefix: Matcher<EnumDatePrefix>
    {
        /* e.g.
         Early 18th century
         Mid 18th century
         Late 18th century
         First half 18th century
         Second half 18th century
         First quarter 18th century
         Second quarter 18th century
         Third quarter 18th century
         Fourth quarter 18th century
        */
        private static string[] patterns_cy = 
        { 
            @"dechrau\'r",                  // early
            @"canol y",                     // middle
            @"(?:tua\s)?d?diwedd y",        // late
            @"hanner cyntaf",               // first half
            @"ail hanner",                  // second half
            @"chwarter (?:1|cynt)af",       // first quarter
            @"[2a]il chwarter",             // second quarter
            @"(?:3|tryd)ydd chwarter",      // third quarter
            @"(?:4|pedwer)ydd chwarter"     // fourth quarter
        };
        private static string[] patterns_de = 
        {
            @"Anfang des",
            @"Mitte des",
            @"Ende des",
            @"Erstes halbes",
            @"Zweite Hälfte des",
            @"Erstes Viertel des",
            @"Zweites Viertel des",
            @"Drittes Viertel des",
            @"Viertes Viertel"
        };
        private static string[] patterns_en = 
        {             
            @"EARLY", 
            @"MID(?:DLE)?", 
            @"LATE",
            @"(?:1|FIR)ST HALF",
            @"(?:2|SECO)ND HALF",
            @"(?:1|FIR)ST QUARTER",
            @"(?:2|SECO)ND QUARTER",
            @"(?:3|THI)RD QUARTER",
            @"(?:(?:4|FOUR)TH|LAST) QUARTER"              
        };
        private static string[] patterns_es = 
        {
            @"Principios del",
            @"Mediados del",
            @"Finales del",
            @"Primera mitad del",
            @"Segunda mitad del",
            @"Primer cuarto del",
            @"Segundo cuarto del",
            @"Tercer trimestre",
            @"Cuarto cuarto"
        };
        private static string[] patterns_fr = 
        {
            @"Début du",
            @"Milieu du",
            @"Fin du",
            @"Première moitié du",
            @"Deuxième moitié du",
            @"Premier quart du",
            @"Deuxième siècle",
            @"Troisième siècle",
            @"Quatrième trimestre"
        };
        private static string[] patterns_it = 
        {
            @"^Prima Età",                                 // E
            @"^Metà del",                                  // M (^ avoids matching this before seconda meta del)
            @"Tarda Età",                                  // L
            @"(?:1\s?(?:°|º)|prima) metà del",             // H1
            @"(?:2\s?(?:°|º)|seconda) metà del",           // H2
            @"(?:1\s?(?:°|º)|primo) quarto del",           // Q1
            @"(?:2\s?(?:°|º)|secondo) quarto del",         // Q2
            @"(?:3\s?(?:°|º)|terzo) quarto del",           // Q3
            @"(?:4\s?(?:°|º)|quarto|ultimo) quarto del"    // Q4
        };
        private static string[] patterns_nl = 
        {
            @"Vroeg",
            @"Midden",
            @"(?:Eind|Laat)",
            @"Eerste helft",
            @"Tweede helft",
            @"Eerste kwartier",
            @"Tweede kwartaal",
            @"Derde kwartaal",
            @"Vierde kwart"
        };
        private static string[] patterns_sv = 
        {
            @"Tidigt",
            @"Mitten av",
            @"Slutet av",
            @"Första hälften av",
            @"Andra halvan",
            @"Första kvartalet",
            @"Andra kvartalet",
            @"Tredje kvartalet",
            @"Fjärde kvartalet"
        };

        // get array of regex patterns for the specified language
        private static string[] Patterns(EnumLanguage language = EnumLanguage.NONE)
        {
            switch (language)
            {
                case EnumLanguage.CY: return patterns_cy;
                case EnumLanguage.DE: return patterns_de;
                case EnumLanguage.EN: return patterns_en;
                case EnumLanguage.ES: return patterns_es;
                case EnumLanguage.FR: return patterns_fr;
                case EnumLanguage.IT: return patterns_it;
                case EnumLanguage.NL: return patterns_nl;
                case EnumLanguage.SV: return patterns_sv;
                default: throw new NotImplementedException();
            }
        }
        
        public static string Pattern(EnumLanguage language = EnumLanguage.NONE, string groupname = "") 
        {
            return oneof(Patterns(language), groupname);
        }

        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            return (Regex.IsMatch(input.Trim(), oneof(Patterns(language)), options));
        }

        public static EnumDatePrefix Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {            
            RegexOptions options = RegexOptions.IgnoreCase;
            input = input.Trim();
            string[] patterns = Patterns(language);

            
            for (int i = 0; i < patterns.Length; i++)
            {
                if (Regex.IsMatch(input, patterns[i], options))
                {
                    switch (i)
                    {
                        case 0: return EnumDatePrefix.EARLY;
                        case 1: return EnumDatePrefix.MID;
                        case 2: return EnumDatePrefix.LATE;
                        case 3: return EnumDatePrefix.HALF1;
                        case 4: return EnumDatePrefix.HALF2;
                        case 5: return EnumDatePrefix.QUARTER1;
                        case 6: return EnumDatePrefix.QUARTER2;
                        case 7: return EnumDatePrefix.QUARTER3;
                        case 8: return EnumDatePrefix.QUARTER4;
                    }
                }
            }
            return EnumDatePrefix.NONE; // not matched
        }
    }    
}