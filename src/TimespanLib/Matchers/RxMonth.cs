using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Timespans.Rx
{
    public class Month: Matcher<EnumMonth>
    {
        private static string[] patterns_cy = 
        { 
            @"Ion(?:\.|awr)?",      // Ion | Ion. | Ionawr
            @"Chwef(?:\.|ror)?",    // Chwef | Chwef. | Chwefror
            @"[MF]aw(?:\.|rth)?",   // Faw | Faw. | Fawrth | Maw | Maw. | Mawrth
            @"H?Ebr(?:\.|ill)?",    // Ebr | Ebr. | Ebrill
            @"[MF]ai",                 // Mai
            @"[MF]eh(?:\.|efin)?",  // Feh | Feh. | Fehefin | Meh | Meh. | Mehefin
            @"G?orff(?:\.|ennaf)?",  // Gorff | Gorff. | Gorffennaf
            @"Awst",                // Awst
            @"[MF]edi",                // Medi
            @"Hyd(?:\.|ref)?",      // Hyd | Hyd. | Hydref
            @"[DT]ach(?:\.|wedd)?",    // Tach | Tach. | Tachwedd
            @"Rh?ag(?:\.|fyr)?"      // Rhag | Rhag. | Rhagfyr
        };
        private static string[] patterns_de = 
        {
            @"J(?:a|ä)n(?:\.|uar)?", // Jan | Jän | Jan. | Jän. | Januar | Jänuar
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
        private static string[] patterns_en = 
        {             
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
        private static string[] patterns_es = 
        {
            @"Enero",               // Enero
            @"Feb(?:\.|rero)?",     // Feb | Feb. | Febrero
            @"Marzo",               // Marzo
            @"Abr(?:\.|il)?",       // Abr | Abr. | Abril
            @"Mayo",                // Mayo
            @"Jun(?:\.|io)?",       // Jun | Jun. | Junio
            @"Jul(?:\.|io)?",       // Jul | Jul. | Julio
            @"Agosto",              // Agosto
            @"Sept(?:\.|iembre)?",  // Sept | Sept. | Septiembre
            @"Oct(?:\.|ubre)?",     // Oct | Oct. | Octubre
            @"Nov(?:\.|iembre)?",   // Nov | Nov. | Noviembre
            @"Dic(?:\.|iembre)?"    // Dic | Dic. | Diciembre           
        };
        private static string[] patterns_fr = 
        {
            @"Janv(?:\.|ier)?",     // Janv | Janv. | Janvier
            @"Févr(?:\.|ier)?",     // Févr | Févr. | Février
            @"Mars",                // Mars
            @"Avril",               // Avril        
            @"Mai",                 // Mai  
            @"Juin",                // Juin
            @"Juil(?:\.|let)?",     // Juil | Juil. | Juillet
            @"Août",                // Août
            @"Sept(?:\.|embre)?",   // Sept | Sept. | Septembre
            @"Oct(?:\.|obre)?",     // Oct | Oct. | Octobre 
            @"Nov(?:\.|embre)?",    // Nov | Nov. | Novembre
            @"Déc(?:\.|embre)?"     // Déc | Déc. | Décembre      
        };
        private static string[] patterns_it = 
        {
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
        private static string[] patterns_nl = 
        {
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
        private static string[] patterns_sv = 
        {
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
            return (Regex.IsMatch(input.Trim(), Pattern(language), options));
        }

        public static EnumMonth Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            input = input.Trim();
            string[] patterns = Patterns(language);

            for (int i = 0; i < patterns.Length; i++)
            {
                if (Regex.IsMatch(input, patterns[i], options))
                {
                    switch (i)
                    {
                        case 0: return EnumMonth.JAN;
                        case 1: return EnumMonth.FEB;
                        case 2: return EnumMonth.MAR;
                        case 3: return EnumMonth.APR;
                        case 4: return EnumMonth.MAY;
                        case 5: return EnumMonth.JUN;
                        case 6: return EnumMonth.JUL;
                        case 7: return EnumMonth.AUG;
                        case 8: return EnumMonth.SEP;
                        case 9: return EnumMonth.OCT;
                        case 10: return EnumMonth.NOV;
                        case 11: return EnumMonth.DEC;
                    }
                }
            }            
            return EnumMonth.NONE; // no match
        }
    }
}
