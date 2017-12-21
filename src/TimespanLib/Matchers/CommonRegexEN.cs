using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Timespans.CommonRegex
{    
    public class en
    {        
        public static readonly string[] datecircapatterns = { //new string[] { 
            @"C(?:\.|irca)",            // C. | Circa
            @"around",                  // around
            @"approx(?:\.|imately)?"    // approx | approx. | approximately
        };

        public static readonly string[] dateprefixpatterns = { //new string[] {
            "EARLY", 
            "MID(?:DLE)", 
            "LATE",
            "(?:1|FIR)ST HALF",
            "(?:2|SECO)ND HALF",
            "(?:1|FIR)ST QUARTER",
            "(?:2|SECO)ND QUARTER",
            "(?:3|THI)RD QUARTER",
            "(?:(?:4|FOUR)TH|LAST) QUARTER"
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
            @"A\.?D\.?",            // AD|A.D|A.D.|AD.
            @"B\.?C\.?",            // BC|B.C|B.C.|BC.
            @"B\.?P\.?",            // BP|B.P|B.P.|BP.
            @"C\.?E\.?"             // CE|C.E|C.E.|CE.
        };
        public static EnumDateSuffix parseDateSuffix(string input)
        {
            RegexOptions options = RegexOptions.IgnoreCase;
            input = input.Trim();

            if (Regex.IsMatch(input, datesuffixpatterns[0], options))
                return EnumDateSuffix.AD;
            else if (Regex.IsMatch(input, datesuffixpatterns[1], options))
                return EnumDateSuffix.BC;
            else if (Regex.IsMatch(input, datesuffixpatterns[2], options))
                return EnumDateSuffix.BP;
            else if (Regex.IsMatch(input, datesuffixpatterns[3], options))
                return EnumDateSuffix.CE;
            else
                return EnumDateSuffix.NONE;
        }
        
        public static readonly string[] named_ordinals = new string[] {
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
            @"twenty\-?first",
            @"twenty\-?second",
            @"twenty\-?third",
            @"twenty\-?fourth",
            @"twenty\-?fifth",
            @"twenty\-?sixth",
            @"twenty\-?seventh",
            @"twenty\-?eighth",
            @"twenty\-?ninth",
            @"thirtieth",
            @"thirty\-?first"
        };
        public static int parseNamedOrdinal(string input)
        {
            RegexOptions options = RegexOptions.IgnoreCase;
            input = input.Trim();
            
            for (int i = 0; i < named_ordinals.Length; i++) {
                if (Regex.IsMatch(input, named_ordinals[i], options))
                    return i + 1;  
            }
            return 0; // no match   
        }
        
        public static readonly string[] daynamepatterns = new string[] {        
            @"Mon(?:\.|day)?",           // Mon | Mon. | Monday
            @"Tue(?:\.|s\.?|sday)?",     // Tue | Tue. | Tues | Tues. | Tuesday
            @"Wed(?:\.|nesday)?",        // Wed | Wed. | Wednesday
            @"Thu(?:\.|r\.?|sday)?",     // Thu | Thu. | Thur | Thur. | Thursday
            @"Fri(?:\.|day)?",           // Fri | Fri. | Friday
            @"Sat(?:\.|urday)?",         // Sat | Sat. | Saturday
            @"Sun(?:\.|day)?"            // Sun | Sun. | Sunday            
        };
        EnumDay parseDayName(string input)
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

        // for abbreviations (various languages) see http://web.library.yale.edu/cataloging/months.htm  
        public static readonly string[] monthnamepatterns = new string[] {
            @"Jan(?:\.|uary)?",     // Jan | Jan. | January
            @"Feb(?:\.|ruary)?",    // Feb | Feb. | February
            @"Mar(?:\.|ch)?",       // Mar | Mar. | March
            @"Apr(?:\.|il)?",       // Apr | Apr. | April
            @"May",                 // May
            @"Jun(?:\.|e)?",        // Jun | Jun. | June
            @"Jul(?:\.|y)?",        // Jul | Jul. | July
            @"Aug(?:\.|ust)?",      // Aug | Aug. | August
            @"Sep(?:\.|t\.?|tember)?", // Sep | Sep. | Sept | Sept. | September
            @"Oct(?:\.|ober)?",     // Oct | Oct. | October
            @"Nov(?:\.|ember)?",    // Nov | Nov. | November
            @"Dec(?:\.|ember)?"     // Dec | Dec. | December            
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

        public static readonly string[] seasonnamepatterns = new string[] {
            @"Spring",     
            @"Summer",   
            @"Autumn",       
            @"(?:Winter|Fall)"            
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

        // Example: Parsing Enumerated value from language specific string
        //public static EnumMonth m = EnumMonth.Parse("Jan.", EnumLanguage.EN);

        // ^(?:(?:C(?:\.|irca)|C.|Circa)\s)?(?<month>January|February|March|etc)\s+(?<year>\d+)(?:\s(?<suffix>AD|BC|BP|CE))?$
        // e.g. January 1850 AD, Jan 1850, Jan. 1850        
        public static readonly string monthyear = String.Concat(
            Rx.START,                               // ^            
            Rx.maybe(String.Concat(Rx.oneof(datecircapatterns), Rx.SPACE)), //  (?:(?:C(?:\.|irca)|C.|Circa)\s)?
            Rx.oneof(en.monthnamepatterns, "month"),       // (?<month>January|February|March|etc)
            Rx.SPACE,                               // \s
            Rx.group(@"\d+", "year"),               // (?<year>\d+)
            Rx.maybe(String.Concat(Rx.SPACE, Rx.oneof(datesuffixpatterns, "suffix"))),  // (?:\s(?<suffix>AD|BC|BP|CE))?
            Rx.END                                  // $
        );

        // ^(?<dayname>Monday|Tuesday|Wednesday|etc)\s(?<ordinal>\d+)(?:st|nd|rd|th)\s(?<monthname>January|February|March|etc)\s(?<year>\d+)$
        // e.g. "Monday 2nd March 1786" => [dayname="Monday", ordinal="2nd", monthname="March", year="1786"]
        public static readonly string longdate = String.Concat(
            Rx.START,                               // ^
            Rx.oneof(daynamepatterns, "dayname"),          // (?<dayname>Monday|Tuesday|Wednesday|etc)
            Rx.SPACE,                               // \s
            Rx.group(@"\d+", "ordinal"),              // (?<ordinal>\d+)       
            Rx.oneof(new string[] { "st", "nd", "rd", "th" }),  // (?:st|nd|rd|th)
            Rx.SPACE,                               // \s
            Rx.oneof(monthnamepatterns, "monthname"),      // (?<monthname>January|February|March|etc)   
            Rx.SPACE,                               // \s
            Rx.group(@"\d+", "year"),              // (?<year>\d+)
            Rx.END                                  // $
        );

        // ^(?:(?:C(?:\.|irca)|C.|Circa)\s)?(?<centuryNo>first|second|third)\scentury(?:\s(?<suffix>AD|BC|BP|CE))?$
        // e.g. "circa eighteenth century AD" => [centuryNo="eighteenth", suffix="AD"]
        public static string named_ordinal_century = String.Concat(
            Rx.START,                                           // ^
            Rx.maybe(String.Concat(Rx.oneof(datecircapatterns), Rx.SPACE)), // (?:(?:C(?:\.|irca)|C.|Circa)\s)?  
            Rx.oneof(named_ordinals, "centuryNo"),                    // (?<centuryNo>first|second|third|etc)
            Rx.SPACE,                                           // \s
            "century",                                          // century
            Rx.maybe(String.Concat(Rx.SPACE, Rx.oneof(datesuffixpatterns, "suffix"))), // (?:\s(?<suffix>AD|BC|BP|CE))?
            Rx.END                                              // $
        );

        // ^(?:(?:C(?:\.|irca)|C.|Circa)\s)?(?<centuryNo>\d+)(?:st|nd|rd|th)\scentury(?:\s(?<suffix>AD|BC|BP|CE))?
        // e.g. "circa 18th century AD" => [centuryNo="18", suffix="AD"]
        public static string numeric_ordinal_century = String.Concat(
           Rx.START,                                            // "^"
           Rx.maybe(String.Concat(Rx.oneof(datecircapatterns), Rx.SPACE)),  // "(?:(?:C(?:\.|irca)|C.|Circa)\s)?"                         
           Rx.group(@"\d+", "centuryNo"),                       // (?<centuryNo>\d+)
           Rx.oneof(new string[] { "st", "nd", "rd", "th" }),   // (?:st|nd|rd|th)
           Rx.SPACE,                                            // "\s"
           "century",                                           // "century"
           Rx.maybe(String.Concat(Rx.SPACE, Rx.oneof(datesuffixpatterns, "suffix"))), // (?:\s(?<suffix>AD|BC|BP|CE))?
           Rx.END                                               // "$"
        );
              
    }
}