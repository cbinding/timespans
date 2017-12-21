using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Timespans.CommonRegex
{
    public class it
    {
        public static readonly string[] datecircapatterns = new string[] { 
            @"C(?:\.|irca)",    // c. | Circa
            "Intorno al"        // Intorno al
        };
                
        public static readonly string[] dateprefixpatterns = new string[] { 
            @"Prima Età",                                  // EARLY
            @"Metà del",                                   // MID
            @"Tarda Età",                                  // LATE
            @"(?:1\s?(?:°|º)|prima) metà del",             // HALF1
            @"(?:2\s?(?:°|º)|seconda) metà del",           // HALF2
            @"(?:1\s?(?:°|º)|primo) quarto del",           // QUARTER1
            @"(?:2\s?(?:°|º)|secondo) quarto del",         // QUARTER2
            @"(?:3\s?(?:°|º)|terzo) quarto del",           // QUARTER3
            @"(?:4\s?(?:°|º)|quarto|ultimo) quarto del"    // QUARTER4
        };
        public static EnumDatePrefix parseDatePrefix(string input)
        {
            RegexOptions options = RegexOptions.IgnoreCase;
            input = input.Trim();

            if (Regex.IsMatch(input, dateprefixpatterns[0], options))
                return EnumDatePrefix.EARLY;
            else if (Regex.IsMatch(input, dateprefixpatterns[1], options))
                return EnumDatePrefix.MID;
            else if (Regex.IsMatch(input, dateprefixpatterns[2], options))
                return EnumDatePrefix.LATE;
            else if (Regex.IsMatch(input, dateprefixpatterns[3], options))
                return EnumDatePrefix.HALF1;
            else if (Regex.IsMatch(input, dateprefixpatterns[4], options))
                return EnumDatePrefix.HALF2;
            else if (Regex.IsMatch(input, dateprefixpatterns[5], options))
                return EnumDatePrefix.QUARTER1;
            else if (Regex.IsMatch(input, dateprefixpatterns[6], options))
                return EnumDatePrefix.QUARTER2;
            else if (Regex.IsMatch(input, dateprefixpatterns[7], options))
                return EnumDatePrefix.QUARTER3;
            else if (Regex.IsMatch(input, dateprefixpatterns[8], options))
                return EnumDatePrefix.QUARTER4;
            else
                return EnumDatePrefix.NONE;
        }
        
        public static readonly string[] datesuffixpatterns = new string[] { 
            @"a\.?C\.?",        // aC | aC. | a.C | a.C.  (BC)
            @"d\.?C\.?"         // dC | dC. | d.C | d.C.  (AD)
        };
        public static EnumDateSuffix parseDateSuffix(string input)
        {
            RegexOptions options = RegexOptions.IgnoreCase;
            input = input.Trim();

            if (Regex.IsMatch(input, datesuffixpatterns[0], options))
                return EnumDateSuffix.BC;
            else if (Regex.IsMatch(input, datesuffixpatterns[1], options))
                return EnumDateSuffix.AD;
            else
                return EnumDateSuffix.NONE;
        }
        
        // Ordinals (Italian)
        public static string[] named_ordinals = new string[] {
            @"primo",           // first
            @"secondo",         // second
            @"terzo",           // third
            @"quarto",          // fourth
            @"quinto",          // fifth
            @"sesto",           // sixth
            @"settimo",         // seventh
            @"ottavo",          // eighth
            @"nono",            // ninth
            @"decimo",          // tenth
            @"undicesimo",      // eleventh
            @"dodicesimo",      // twelfth
            @"tredicesimo",     // thirteenth
            @"quattordicesimo", // fourteenth
            @"quindicesimo",    // fifteenth
            @"sedicesimo",      // sixteenth
            @"diciassettesimo", // seventeenth
            @"diciottesimo",    // eighteenth
            @"diciannovesimo",  // nineteenth
            @"ventesimo",       // twentieth
            @"ventunesimo",     // twenty first  
            @"ventiduesima",    // twenty second
            @"ventitreesimo",   // twenty third
            @"ventiquattresimo",    // twenty fourth
            @"venticinquesimo", // twenty fifth
            @"ventiseiesimo",   // twenty sixth
            @"ventisettesimo",  // twenty seventh
            @"ventotto",        // twenty eighth
            @"ventinovesimo",   // twenty ninth
            @"trentesimo",      // thirtieth
            @"trentunesima"     // thirty first
        };
        public static int parseNamedOrdinal(string input)
        {
            RegexOptions options = RegexOptions.IgnoreCase;
            input = input.Trim();

            for (int i = 0; i < named_ordinals.Length; i++)
            {
                if (Regex.IsMatch(input, named_ordinals[i], options))
                    return i + 1;
            }
            return 0; // no match   
        }

        private static string numeric_ordinals = @"(?<ordinal>\d+)\s?(?:°|º)"; // 15° | 15º | 15 ° | 15 º

        // Month names (Italian)
        public static string[] monthnamepatterns = new string[] {
            @"Genn(?:\.|aio)?",     // Genn | Genn. | Gennaio
            @"Febbr(?:\.|aio)?",    // Febbr | Febbr. | Febbraio
            @"Mar(?:\.|zo)?",       // Mar | Mar. | Marzo
            @"Apr(?:\.|ile)?",      // Apr | Apr. | Aprile
            @"Magg(?:\.|io)?",      // Magg | Magg. | Maggio
            @"Giu(?:\.|gno)?",      // Giu | Giu. | Giugno
            @"Lug(?:\.|lio)?",      // Lug | Lug. | Luglio
            @"Ag(?:\.|osto)?",      // Ag  | Ag.  | Agosto
            @"Sett(?:\.|embre)?",   // Sett | Sett. | Settembre
            @"Ott(?:\.|obre)?",     // Ott | Ott. | Ottobre
            @"Nov(?:\.|embre)?",    // Nov | Nov. | Novembre
            @"Dic(?:\.|embre)?"     // Dic | Dic. | Dicembre                   
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

        private static string[] daynamepatterns = new string[] {
            @"lun(?:\.|edì)",      // Monday
            @"mar(?:\.|tedì)",     // Tuesday
            @"mer(?:\.|coledì)",   // Wednesday
            @"gio(?:\.|vedì)",     // Thursday
            @"ven(?:\.|erdì)",     // Friday
            @"sab(?:\.|ato)",      // Saturday
            @"do(?:\.|menica)"     // Sunday                  
        };
        public static EnumDay parseDayName(string input)
        {
            RegexOptions options = RegexOptions.IgnoreCase;
            input = input.Trim();

            if (Regex.IsMatch(input, daynamepatterns[0], options))
                return EnumDay.MON;
            else if (Regex.IsMatch(input, daynamepatterns[1], options))
                return EnumDay.TUE;
            else if (Regex.IsMatch(input, daynamepatterns[2], options))
                return EnumDay.WED;
            else if (Regex.IsMatch(input, daynamepatterns[3], options))
                return EnumDay.THU;
            else if (Regex.IsMatch(input, daynamepatterns[4], options))
                return EnumDay.FRI;
            else if (Regex.IsMatch(input, daynamepatterns[5], options))
                return EnumDay.SAT;
            else if (Regex.IsMatch(input, daynamepatterns[6], options))
                return EnumDay.SUN;
            else
                return EnumDay.NONE;
        }

        public static readonly string[] seasonnamepatterns = new string[] {
            @"Primavera",   // Spring     
            @"Estate",      // Summer 
            @"Autunno",     // Autumn     
            @"Inverno"      // Winter           
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

        // Example century patterns:
        // 18th century (Italian expressions):
        // 18° secolo
        // Secolo XVIII
        // XVIII Secolo
        // Diciottesimo secolo
        // '700
        // 18º secolo 
        // Settecento

        // composite built patterns:

        // ^(?:C(?:\.|irca)|Intorno al)?(?:(?<prefix>Prima Età|Metà del|etc)\s)?\ssec(?:\.|olo)(?:\s(?<suffix>a\.?C\.?|d\.?C\.?))?$
        // e.g. "Intorno al Diciottesimo secolo d.C."
        public static string ordinal_century = String.Concat(
            Rx.START,                                                           // ^
            Rx.maybe(Rx.oneof(datecircapatterns) + Rx.SPACE),                   // (?:C(?:\.|irca)|Intorno al)?          
            Rx.maybe(Rx.oneof(dateprefixpatterns, "prefix") + Rx.SPACE),        // (?:(?<prefix>Prima Età|Metà del|etc)\s)?
            Rx.oneof(named_ordinals, "century"),                                // (?<centuryNo>primo|secondo|etc)
            Rx.SPACE,                                                           // \s
            @"sec(?:\.|olo)",                                                   // sec(?:\.|olo)
            Rx.maybe(Rx.SPACE + Rx.oneof(datesuffixpatterns, "suffix")),        // (?:\s(?<suffix>a\.?C\.?|d\.?C\.?))?
           Rx.END                                                               // $
        );

        // ^(?:C(?:\.|irca)|Intorno al)?(?<year>\d+)\s?(?:°|º)\ssec(?:\.|olo)(?:\s(?<suffix>a\.?C\.?|d\.?C\.?))?$
        // e.g. "Intorno al 18° secolo d.C."
        public static string numeric_century = String.Concat(
           Rx.START,                                                            // ^
           Rx.maybe(Rx.oneof(datecircapatterns) + Rx.SPACE),                    // (?:C(?:\.|irca)|Intorno al)? 
           Rx.group(@"\d+", "year"),                                             // (?<year>\d+)
           Rx.maybe(Rx.SPACE),                                                  // \s?
           Rx.oneof(new string[] { "°", "º" }),                                 // (?:°|º)
           Rx.SPACE,                                                            // \s
           @"sec(?:\.|olo)",                                                    // sec(?:\.|olo)
           Rx.maybe(Rx.SPACE + Rx.oneof(datesuffixpatterns, "suffix")),         // (?:\s(?<suffix>a\.?C\.?|d\.?C\.?))?
           Rx.END                                                               // $
        );

        // "Intorno al XVIII secolo d.C."
        public static string numeral_century = String.Concat(
            Rx.START,
            Rx.maybe(Rx.oneof(datecircapatterns)),
            Rx.oneormore(Rx.SPACE),
            Rx.group("[MCDXVI]+", "year"),
            Rx.oneormore(Rx.SPACE),
            Rx.oneof(new string[] { "secolo", @"sec\.?" }),
            Rx.maybe(Rx.SPACE + Rx.oneof(datesuffixpatterns, "qualifier")),
            Rx.END
        );

    }
}