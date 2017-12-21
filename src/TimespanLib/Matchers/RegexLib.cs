using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Timespans
{    
     public class RegexLib
     {       
        public const string circa_en = @"c(?:irca|.)?";
        public const string circa_it = @"(?<circa_it>circa|c\.|intorno al)";
        public const string qualifier_en = @"(AD|BC|BP|CE)";
        public const string qualifier_it = @"(a\.C\.|d\.C\.)";
        public const string centuryprefix_en = @"(EARLY|MID|LATE)";
        public const string centuryprefix_nl = @"(VROEG|MIDDEN|LAAT)";
        public const string ordinals_en = @"(first|second|third|fourth|fifth|sixth|seventh|eighth|ninth|tenth|eleventh|twelfth|thirteenth|fourteenth|fifteenth|sixteenth|seventeenth|eighteenth|nineteenth|twentieth|twenty first)";
        public const string ordinals_nl = @"(eerste|tweede|derde|vierde|vijfe|zesde|zevende|achtse|negende|tiende|elfde|twaalfde|dertiende|veertiende|vijftiende|zestiende|zeventiende|achttiende|negentiende|twintigste)";
        public const string ordinals_de = @"(erste|zweite|dritte|vierte|fünfte|sechste|siebte|achte|neunte|zehnte|elfte|zwölfte|dreizehnte|vierzehnte|fünfzehnte|sechzehnte|siebzehnte|achtzehnte|neunzehnte|zwanzig|einundzwanzigster)";
        public const string ordinals_it = @"(primo|secondo|terzo|quarto|quinto|sesto|settimo|ottavo|nono|decimo|undicesimo|dodicesimo|tredicesimo|quattordicesimo|quindicesimo|sedicesimo|diciassettesimo|diciottesimo|diciannovesimo|ventesimo|ventunesimo)";
        public const string ordinals_sv = @"(första|andra|tredje|fjärde|femte|sjätte|sjunde|åttonde|nionde|tionde|elfte|tolfte|trettonde|fjortonde|femtonde|sextonde|sjuttonde|artonde|nittonde|tjugonde|tjugoförsta)";
        public const string ordinals_fr = @"(premier|première|deuxième|second|seconde|troisième|quatrième|cinquième|sixième|septième|huitième|neuvième|dixième|onzième|douzième|treizième|quatorzième|quinzième|seizième|dix-septième|dix-huitième|dix-neuvième|vingtième|vingt et unième)";
        public const string ordinals_es = @"(primero|segundo|tercero|cuarto|quinto|sexto|séptimo|octavo|noveno|décimo|undécimo|once|duodécimo|doce|decimotercero|trece|decimocuarto|catorce|decimoquinto|quince|decimosexto|dieciséis|decimoséptimo|diecisiete|decimoctavo|dieciocho|decimonoveno|diecinueve|vigésimo|veinte|vigésimo primero)";
         
        
       
        
        
        

        // ************************************************************************************
        // NEW 03/08/2017 - build larger regex patterns from small functions and reusable parts
        // ************************************************************************************
        private const string START = @"^";
        private const string SPACE = @"\s";
        private const string DIGIT = @"\d";
        private const string ANY = @".";
        private const string END = @"$";
                 
        // subexpression grouping
        public static string oneof(char[] input) { return String.Concat("[", String.Join("", input), "]"); } // [AEIOU]
        public static string oneof(string[] input, string name = "") { return group(String.Join("|", input), name); } // (?:tom|dick|harry)
        private static string group(string input, string name = "") { return "(?" + (name != "" ? "<" + name + ">" : ":") + input + ")"; }
                   
        // repeaters
        private static string optional(string input) { return group(input) + "?"; }
        private static string zeroormore(string value) { return group(value) + "*"; }
        private static string oneormore(string value) { return group(value) + "+"; }
        private static string exactly(string value, int n) { return group(value) + "{" + n.ToString() + "}"; }
        private static string minimum(string value, int n) { return group(value) + "{" + n.ToString() + ",}"; }
        private static string range(string value, int n, int m) { return group(value) + "{" + n.ToString() + "," + m.ToString() + "}"; }
                   
        // specialized prefix and suffix variants
        private static string[] circa_en1 = new string[] { "Circa", @"C\.", "around", @"approx\.?", "approximately" };
        
        
        private static string[] qualifier_en1 = new string[] { "AD", "BC", "BP", "CE" };
        private static string[] qualifier_it1 = new string[] { @"a\.C\.", @"d\.C\." };

         // decades:
        //@"^(?:c(?:irca|\.|)\s*)?(?<decade>\d+[1-9]0)\'?s$";


         //@"^(?:Intorno\sal|circa)?\s*decennio\s(?<decade>\d+[1-9]0)(?:esimo|mo)?$";



     } 
}
