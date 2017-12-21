using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Timespans.Rx
{
    // see http://www.omniglot.com/language/numbers/
    public class Ordinal : Matcher<int>
    {
        private static string[] patterns_cy = 
        { 
            //@"(\d+)(?:af|ail|ydd|ed|fed|eg|ain)",   // short form numeric e.g. 1af, 2ail, 3ydd
            @"cyntaf",                  // 1st
            @"ail",                     // 2nd
            @"tryd[ye]dd",              // 3rd
            @"pedw(?:ery|are)dd",       // 4th
            @"pumed",                   // 5th
            @"chweched",                // 6th
            @"seithfed",                // 7th
            @"wythfed",                 // 8th
            @"nawfed",                  // 9th
            @"degfed",                  // 10th
            @"unfed ar ddeg",           // 11th
            @"deuddegfed",              // 12th
            @"trydydd ar ddeg",         // 13th
            @"pedwerydd ar ddeg",       // 14th
            @"[bp]ymthegfed",           // 15th
            @"unfed ar bymtheg",        // 16th
            @"ail ar bymtheg",          // 17th
            @"deunawfed",               // 18th
            @"pedwerydd ar bymtheg",    // 19th
            @"ugeinfed",                // 20th
            @"unfed ar hugain",         // 21st
            @"ail ar hugain",           // 22nd
            @"trydydd ar hugain",       // 23rd
            @"pedwerydd ar hugain",     // 24th
            @"pumed ar hugain",         // 25th
            @"chweched ar hugain",      // 26th
            @"seithfed ar hugain",      // 27th
            @"wythfed ar hugain",       // 28th
            @"nawfed ar hugain",        // 29th
            @"degfed ar hugain",        // 30th
            @"unfed ar ddeg ar hugain"  // 31st
        };
        private static string[] patterns_de = 
        {
            @"erste",               // 1st
            @"zweite",              // 2nd
            @"dritte",              // 3rd
            @"vierte",              // 4th
            @"fünfte",              // 5th
            @"sechste",             // 6th
            @"siebte",              // 7th
            @"achte",               // 8th
            @"neunte",              // 9th
            @"zehnte",              // 10th
            @"elfte",               // 11th
            @"zwölfte",             // 12th
            @"dreizehnte",          // 13th
            @"vierzehnte",          // 14th
            @"fünfzehnte",          // 15th
            @"sechzehnte",          // 16th
            @"siebzehnte",          // 17th
            @"achtzehnte",          // 18th
            @"neunzehnte",          // 19th
            @"zwanzigste",          // 20th
            @"einundzwanzigste",    // 21st
            @"zweiundzwanzigste",   // 22nd
            @"dreiundzwanzigste",   // 23rd
            @"vierundzwanzigste",   // 24th
            @"fünfundzwanzigste",   // 25th
            @"sechsundzwanzigste",  // 26th
            @"siebenundzwanzigste", // 27th
            @"achtundzwanzigste",   // 28th
            @"neunundzwanzigste",   // 29th
            @"dreißigste",          // 30th
            @"(?:Dreißig zuerst|einunddreißigsten)", // 31st
        };
        private static string[] patterns_en = 
        {   
            //@"(\d+)(?:st|nd|rd|th)",    // numeric
            @"first",        
            @"second",    
            @"third", 
            @"fourth", 
            @"fifth", 
            @"sixth",
            @"seventh",
            @"eighth",
            @"ninth",
            @"tenth",
            @"eleventh",
            @"twelfth",
            @"thirteenth",
            @"fourteenth",
            @"fifteenth",
            @"sixteenth",
            @"seventeenth",
            @"eighteenth",
            @"nineteenth",
            @"twentieth",
            @"twenty[\-\s]first",
            @"twenty[\-\s]second",
            @"twenty[\-\s]third",
            @"twenty[\-\s]fourth",
            @"twenty[\-\s]fifth",
            @"twenty[\-\s]sixth",
            @"twenty[\-\s]seventh",
            @"twenty[\-\s]eighth",
            @"twenty[\-\s]ninth",
            @"thirtieth",
            @"thirty[\-\s]first"              
        };
        private static string[] patterns_es = 
        {
            // @"\d+\.(?:º|ª|er) // short form e.g. "1.º", "1.ª", "1.er" (or Roman numerals e.g. Siglo XX)            
            @"primer(?:o|a)?",      // 1st
            @"segund(?:o|a)",       // 2nd
            @"tercer(?:o|a)?",      // 3rd
            @"cuart(?:o|a)",        // 4th
            @"quint(?:o|a)",        // 5th
            @"sext(?:o|a)",         // 6th
            @"séptim(?:o|a)",       // 7th
            @"octav(?:o|a)",        // 8th
            @"noven(?:o|a)",        // 9th
            @"décim(?:o|a)",        // 10th
            @"undécim(?:o|a)",      // 11th       
            @"duodécim(?:o|a)",     // 12th
            @"decimotercer(?:o|a)", // 13th  
            @"decimocuart(?:o|a)",  // 14th
            @"decimoquint(?:o|a)",  // 15th
            @"decimosext(?:o|a)",   // 16th
            @"decimoséptim(?:o|a)", // 17th
            @"decimoctav(?:o|a)",   // 18th
            @"decimonoven(?:o|a)",  // 19th
            @"vigésim(?:o|a)",      // 20th
            @"vigésimoprimer(?:o|a)",    // 21st
            @"vigésimosegund(?:o|a)",    // 22nd
            @"vigésimotercer(?:o|a)",    // 23rd
            @"vigésimocuart(?:o|a)",     // 24th
            @"vigésimoquint(?:o|a)",     // 25th
            @"vigésimosext(?:o|a)",      // 26th
            @"vigésimoséptim(?:o|a)",    // 27th
            @"vigésimooctav(?:o|a)",     // 28th
            @"vigésimonoven(?:o|a)",     // 29th
            @"trigésim(?:o|a)",          // 30th
            @"trigésimoprimer(?:o|a)"    // 31st
        };
        private static string[] patterns_fr = 
        {
            @"premiere?",               // 1st
            @"(?:deuxième|seconde?)",   // 2nd
            @"troisième",               // 3rd
            @"quatrième",               // 4th
            @"cinquième",               // 5th
            @"sixième",                 // 6th
            @"septième",                // 7th
            @"huitième",                // 8th
            @"neuvième",                // 9th
            @"dixième",                 // 10th
            @"onzième",                 // 11th
            @"douzième",                // 12th
            @"treizième",               // 13th
            @"quatorzième",             // 14th
            @"quinzième",               // 15th
            @"seizième",                // 16th
            @"dix\-septième",           // 17th
            @"dix\-huitième",           // 18th
            @"dix\-neuvième",           // 19th
            @"vingtième",               // 20th
            @"vingt\-et\-unième",       // 21st
            @"vingt\-deuxième",         // 22nd
            @"vingt\-troisième",        // 23rd
            @"vingt\-quatrième",        // 24th
            @"vingt\-cinquième",        // 25th
            @"vingt\-sixième",          // 26th
            @"vingt\-septième",         // 27th
            @"vingt\-huitième",         // 28th
            @"vingt\-neuvième",         // 29th 
            @"trentième",               // 30th 
            @"trent\-et-unième"         // 31st
        };
        private static string[] patterns_it = 
        {
            @"primo",           // 1st
            @"secondo",         // 2nd
            @"terzo",           // 3rd
            @"quarto",          // 4th
            @"quinto",          // 5th
            @"sesto",           // 6th
            @"settimo",         // 7th
            @"ottavo",          // 8th
            @"nono",            // 9th
            @"decimo",          // 10th
            @"undicesimo",      // 11th
            @"dodicesimo",      // 12th
            @"tredicesimo",     // 13th
            @"quattordicesimo", // 14th
            @"quindicesimo",    // 15th
            @"sedicesimo",      // 16th
            @"diciassettesimo", // 17th
            @"diciottesimo",    // 18th
            @"diciannovesimo",  // 19th
            @"ventesimo",       // 20th
            @"ventunesimo",     // 21st  
            @"ventiduesima",    // 22nd
            @"ventitreesimo",   // 23rd
            @"ventiquattresimo", // 24th
            @"venticinquesimo", // 25th
            @"ventiseiesimo",   // 26th
            @"ventisettesimo",  // 27th
            @"ventotto",        // 28th
            @"ventinovesimo",   // 29th
            @"trentesimo",      // 30th
            @"trentunesima"     // 31st                  
        };
        private static string[] patterns_nl = 
        {
            @"eerste",          // 1st
            @"tweede",          // 2nd
            @"derde",           // 3rd
            @"vierde",          // 4th
            @"vijfe",           // 5th
            @"zesde",           // 6th
            @"zevende",         // 7th
            @"achtse",          // 8th
            @"negende",         // 9th
            @"tiende",          // 10th
            @"elfde",           // 11th
            @"twaalfde",        // 12th
            @"dertiende",       // 13th
            @"veertiende",      // 14th
            @"vijftiende",      // 15th
            @"zestiende",       // 16th
            @"zeventiende",     // 17th
            @"achttiende",      // 18th
            @"negentiende",     // 19th
            @"twintigste",      // 20th
            @"eenentwintigste", // 21st
            @"tweeëntwintigste", // 22nd
            @"drieëntwintig",   // 23rd
            @"vierentwintig",   // 24th
            @"vijfentwintig",   // 25th
            @"zesentwintig",    // 26th
            @"zevenentwintig",  // 27th
            @"achtentwintig",   // 28th
            @"negenentwintig",  // 29th
            @"dertigste",       // 30th
            @"eenendertigste"   // 31st
            
        };
        private static string[] patterns_sv = 
        {
            //@"\d+\:[ae]", // short form e.g. "24:e"
            @"första",          // 1st
            @"andra",           // 2nd
            @"tredje",          // 3rd
            @"fjärde",          // 4th
            @"femte",           // 5th
            @"sjätte",          // 6th
            @"sjunde",          // 7th
            @"åttonde",         // 8th
            @"nionde",          // 9th
            @"tionde",          // 10th
            @"elfte",           // 11th
            @"tolfte",          // 12th
            @"trettonde",       // 13th
            @"fjortonde",       // 14th
            @"femtonde",        // 15th
            @"sextonde",        // 16th
            @"sjuttonde",       // 17th
            @"artonde",         // 18th
            @"nittonde",        // 19th
            @"tjugonde",        // 20th
            @"tjugoförsta",     // 21st
            @"tjugoandra",      // 22nd
            @"tjugotredje",     // 23rd
            @"tjugofjärde",     // 24th
            @"tjugofemte",      // 25th
            @"tjugosjätte",     // 26th
            @"tjugosjunde",     // 27th
            @"tjugoåttonde",    // 28th
            @"tjugonionde",     // 29th
            @"trettionde",      // 30th
            @"trettioförsta"    // 31st
        };

        // get array of regex patterns for the specified language
        private static string[] Patterns(EnumLanguage language)
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
            return oneof(Patterns(language), groupname); // regex named capture group: (?<groupname>first|second|third|etc)
        }

        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            return (Regex.IsMatch(input.Trim(), Pattern(language), options));
        }

        public static int Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            input = input.Trim();
            string[] patterns = Patterns(language);

            for(int i = 0; i < patterns.Length; i++)
            {
                if (Regex.IsMatch(input, patterns[i], options))
                    return i + 1;
            }
            return -1; // not matched            
        }    
    }
}