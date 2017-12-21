using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Timespans.Rx
{
    // see http://www.omniglot.com/language/numbers/
    public class Cardinal : Matcher<int>
    {        
        private static string[] patterns_cy = 
        { 
            @"un",                  // 1
            @"(?:dau|dwy)",         // 2
            @"(?:tri|tair)",        // 3
            @"ped(?:wa|ai)r",       // 4
            @"pump?",               // 5
            @"chwe(?:ch)?",         // 6
            @"saith",               // 7
            @"wyth",                // 8
            @"naw",                 // 9
            @"deg",                 // 10
            @"un (?:ar ddeg|deg un)",   // 11
            @"(?:deuddeg|un deg dau)",  // 12
            @"(?:tri ar ddeg|un deg tri)",  // 13
            @"(?:pedwar ar ddeg|un deg pedwar)", // 14
            @"(?:pymtheg|un deg pump)", // 15
            @"(?:un ar bymtheg|un deg chwech)", // 16
            @"(?:dau ar bymtheg|un deg saith)",  // 17
            @"(?:deunaw|un deg wyth)",  // 18
            @"(?:pedwar ar bymtheg|un deg nau)", // 19
            @"(?:ugain|dau ddeg)",      // 20
            @"(?:un ar hugain|dau ddeg un)",   // 21
            @"(?:dau ar hugain|dau ddeg dau)", // 22
            @"(?:tri ar hugain|dau ddeg tri)", // 23
            @"(?:pedwar ar hugain|dau ddeg pedwar)", // 24
            @"(?:pump ar hugain|dau ddeg pump)", // 25
            @"(?:chwech ar hugain|dau ddeg chwech)", // 26
            @"(?:saith ar hugain|dau ddeg saith)",  // 27
            @"(?:wyth ar hugain|dau ddeg wyth)", // 28
            @"(?:naw ar hugain|dau ddeg naw)", // 29
            @"(?:deg ar hugain|tri deg)",  // 30
            @"(?:un ar ddeg ar hugain|tri deg un)"  // 31
        };
        private static string[] patterns_de = 
        {
            @"eins",                // 1
            @"zwei",                // 2
            @"drei",                // 3
            @"vier",                // 4
            @"fünf",                // 5
            @"sechs",               // 6
            @"sieben",              // 7
            @"acht",                // 8
            @"neun",                // 9
            @"zehn",                // 10
            @"elf",                 // 11
            @"zwölf",               // 12
            @"dreizehn",            // 13
            @"vierzehn",            // 14
            @"fünfzehn",            // 15
            @"sechzehn",            // 16
            @"siebzehn",            // 17
            @"achtzehn",            // 18
            @"neunzehn",            // 19
            @"zwanzig",             // 20
            @"einundzwanzig",       // 21
            @"zweiundzwanzig",      // 22
            @"dreiundzwanzig",      // 23
            @"vierundzwanzig",      // 24
            @"fünfundzwanzig",      // 25
            @"sechsundzwanzig",     // 26
            @"siebenundzwanzig",    // 27
            @"achtundzwanzig",      // 28
            @"neunundzwanzig",      // 29
            @"dreißig",             // 30
            @"einunddreißig",       // 31
        };
        private static string[] patterns_en = 
        {   
            @"one",        
            @"two",    
            @"three", 
            @"four", 
            @"five", 
            @"six",
            @"seven",
            @"eight",
            @"nine",
            @"ten",
            @"eleven",
            @"twelve",
            @"thirteen",
            @"fourteen",
            @"fifteen",
            @"sixteen",
            @"seventeen",
            @"eighteen",
            @"nineteen",
            @"twenty",
            @"twenty[\-\s]one",
            @"twenty[\-\s]two",
            @"twenty[\-\s]three",
            @"twenty[\-\s]four",
            @"twenty[\-\s]five",
            @"twenty[\-\s]six",
            @"twenty[\-\s]seven",
            @"twenty[\-\s]eight",
            @"twenty[\-\s]nine",
            @"thirty",
            @"thirty[\-\s]one"              
        };
        private static string[] patterns_es = 
        {
            @"un(?:o|a)?",      // 1
            @"dos",             // 2
            @"tres",            // 3
            @"cuatro",          // 4
            @"cinco",           // 5
            @"seis",            // 6
            @"siete",           // 7
            @"ocho",            // 8
            @"nueve",           // 9
            @"diez",            // 10
            @"once",            // 11       
            @"doce",            // 12
            @"trece",           // 13  
            @"catorce",         // 14
            @"quince",          // 15
            @"dieciséis",       // 16
            @"diecisiete",      // 17
            @"dieciocho",       // 18
            @"diecinueve",      // 19
            @"veinte",          // 20
            @"veintiuno",       // 21
            @"veintidós",       // 22
            @"veintitres",      // 23
            @"veinticuatro",    // 24
            @"veinticinco",     // 25
            @"veintiseis",      // 26
            @"veintisiete",     // 27
            @"veintiocho",      // 28
            @"veintinueve",     // 29
            @"treinta",         // 30
            @"treinta y uno"    // 31
        };
        private static string[] patterns_fr = 
        {
            @"une?",            // 1
            @"deux",            // 2
            @"trois",           // 3
            @"quatre",          // 4
            @"cinq",            // 5
            @"six",             // 6
            @"sept",            // 7
            @"huit",            // 8
            @"neuf",            // 9
            @"dix",             // 10
            @"onze",            // 11
            @"douze",           // 12
            @"treize",          // 13
            @"quatorze",        // 14
            @"quinze",          // 15
            @"seize",           // 16
            @"dix\-sept",       // 17
            @"dix\-huit",       // 18
            @"dix\-neuf",       // 19
            @"vingt",           // 20
            @"vingt\-et\-un",   // 21
            @"vingt\-deux",     // 22
            @"vingt\-trois",    // 23
            @"vingt\-quatre",   // 24
            @"vingt\-cinq",     // 25
            @"vingt\-six",      // 26
            @"vingt\-sept",     // 27
            @"vingt\-huit",     // 28
            @"vingt\-neuf",     // 29 
            @"trente",          // 30 
            @"trent\-et\-un"    // 31
        };
        private static string[] patterns_it = 
        {
            @"un[oa]",          // 1
            @"due",             // 2
            @"tre",             // 3
            @"quattro",         // 4
            @"cinque",          // 5
            @"sei",             // 6
            @"sette",           // 7
            @"otto",            // 8
            @"nove",            // 9
            @"dieci",           // 10
            @"undici",          // 11
            @"dodici",          // 12
            @"tredici",         // 13
            @"quattordici",     // 14
            @"quindici",        // 15
            @"sedici",          // 16
            @"diciassette",     // 17
            @"diciotto",        // 18
            @"diciannove",      // 19
            @"venti",           // 20
            @"ventuno",         // 21  
            @"ventidue",        // 22
            @"ventitré",        // 23
            @"ventiquattro",    // 24
            @"venticinque",     // 25
            @"ventisei",        // 26
            @"ventisette",      // 27
            @"ventotto",        // 28
            @"ventinove",       // 29
            @"trenta",          // 30
            @"quaranta"         // 31                  
        };
        private static string[] patterns_nl = 
        {
            @"één",             // 1
            @"twee",            // 2
            @"drie",            // 3
            @"vier",            // 4
            @"vijf",            // 5
            @"zes",             // 6
            @"zeven",           // 7
            @"acht",            // 8
            @"negen",           // 9
            @"tien",            // 10
            @"elf",             // 11
            @"twaalf",          // 12
            @"dertien",         // 13
            @"veertien",        // 14
            @"vijftien",        // 15
            @"zestien",         // 16
            @"zeventien",       // 17
            @"achttien",        // 18
            @"negentien",       // 19
            @"twintig",         // 20
            @"eenentwintig",    // 21
            @"tweeëntwintig",   // 22
            @"drieëntwintig",   // 23
            @"vierentwintig",   // 24
            @"vijfentwintig",   // 25
            @"zesentwintig",    // 26
            @"zevenentwintig",  // 27
            @"achtentwintig",   // 28
            @"negenentwintig",  // 29
            @"dertig",          // 30
            @"eenendertig"      // 31
            
        };
        private static string[] patterns_sv = 
        {
            @"enn",             // 1
            @"två",             // 2
            @"tre",             // 3
            @"fyra",            // 4
            @"fem",             // 5
            @"sex",             // 6
            @"sju",             // 7
            @"åtta",            // 8
            @"nio",             // 9
            @"tio",             // 10
            @"elva",            // 11
            @"tolv",            // 12
            @"tretton",         // 13
            @"fjorton",         // 14
            @"femton",          // 15
            @"sexton",          // 16
            @"sjutton",         // 17
            @"arton",           // 18
            @"nitton",          // 19
            @"tjugo",           // 20
            @"tjugoett",        // 21
            @"tjugotvå",        // 22
            @"tjugotre",        // 23
            @"tjugofyra",       // 24
            @"tjugofem",        // 25
            @"tjugosex",        // 26
            @"tjugosju",        // 27
            @"tjugoåtta",       // 28
            @"tjugonio",        // 29
            @"trettio",         // 30
            @"trettioett"       // 31
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
            // regex named capture group: (?<groupname>one|two|three|etc)
            return oneof(Patterns(language), groupname); 
        }

        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            return (Regex.IsMatch(input, Pattern(language), options));
        }

        public static int Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            input = input.Trim();
            string[] patterns = Patterns(language);

            for (int i = 0; i < patterns.Length; i++)
            {
                if (Regex.IsMatch(input, patterns[i], options))
                    return i + 1;
            }
            return -1; // not matched            
        }
    }
}