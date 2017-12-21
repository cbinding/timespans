using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Timespans.Rx
{
    public class Day : Matcher<EnumDay>
    {
        private static string[] patterns_cy = 
        { 
            @"(?:(?:Dd?|n)ydd\s)?Llun",           // Monday
            @"(?:(?:Dd?|n)ydd\s)?[MF]awrth",      // Tuesday
            @"(?:(?:Dd?|n)ydd\s)?[MF]ercher",     // Wednesday
            @"(?:(?:Dd?|n)ydd\s)?Iau",            // Thursday   
            @"(?:(?:Dd?|n)ydd\s)?(?:G|Ng)?wener", // Friday
            @"(?:(?:Dd?|n)ydd\s)?Sadwrn",         // Saturday
            @"(?:(?:Dd?|n)ydd\s)?Sul"             // Sunday
        };
        private static string[] patterns_de = 
        {
            @"Mo(?:\.|ntag)?",      // Monday (Mo | Mo. | Montag)
            @"Di(?:\.|enstag)?",    // Tuesday (Di | Di. | Dienstag)
            @"Mi(?:\.|ttwoch)?",    // Wednesday (Mi | Mi. | Mittwoch)
            @"Do(?:\.|nnerstag)?",  // Thursday (Do | Do. | Donnerstag)
            @"Fr(?:\.|eitag)?",     // Friday (Fr | Fr. | Freitag)
            @"Sa(?:\.|mstag)?",     // Saturday (Sa | Sa. | Samstag)
            @"So(?:\.|nntag)"       // Sunday (So | So. | Sonntag)
        };
        private static string[] patterns_en = 
        {             
            @"Mon(?:\.|day)?",           // (Mon | Mon. | Monday)
            @"Tue(?:\.|s\.?|sday)?",     // (Tue | Tue. | Tues | Tues. | Tuesday)
            @"Wed(?:\.|nesday)?",        // (Wed | Wed. | Wednesday)
            @"Thu(?:\.|r\.?|sday)?",     // (Thu | Thu. | Thur | Thur. | Thursday)
            @"Fri(?:\.|day)?",           // (Fri | Fri. | Friday)
            @"Sat(?:\.|urday)?",         // (Sat | Sat. | Saturday)
            @"Sun(?:\.|day)?"            // (Sun | Sun. | Sunday)
        };
        private static string[] patterns_es = 
        {
            @"(?:L|Lun|lunes)",     // Monday (L | Lun | lunes)
            @"(?:M|Mar|martes)",    // Tuesday (M | Mar | martes)
            @"(?:X|Mie|miércoles)", // Wednesday (X | Mie | miércoles)
            @"(?:J|Jue|jueves)",    // Thursday (J | Jue | jueves)
            @"(?:V|Vie|viernes)",   // Friday (V | Vie | viernes)
            @"(?:S|Sáb|sábado)",    // Saturday (S | Sáb | sábado)
            @"(?:D|Dom|domingo)"    // Sunday (D | Dom | domingo)
        };
        private static string[] patterns_fr = 
        {
            @"lun(?:\.|di)?",       // Monday (lun | lun. | lundi)
            @"mar(?:\.|di)?",       // Tuesday (mar | mar. | mardi)
            @"mer(?:\.|credi)?",    // Wednesday (mer | mer. | mercredi)
            @"jeu(?:\.|di)?",       // Thursday (jeu | jeu. | jeudi)
            @"ven(?:\.|dredi)?",    // Friday (ven | ven. | vendredi)
            @"sam(?:\.|edi)?",      // Saturday (sam | sam. | samedi)
            @"dim(?:\.|anche)"      // Sunday (dim | dim. | dimanche)
        };
        private static string[] patterns_it = 
        {
            @"lun(?:\.|edì)?",      // Monday (lun | lun. | lunedì)
            @"mar(?:\.|tedì)?",     // Tuesday (mar | mar. | martedì)
            @"mer(?:\.|coledì)?",   // Wednesday (mer | mer. | mercoledì)
            @"gio(?:\.|vedì)?",     // Thursday (gio | gio. | giovedì)
            @"ven(?:\.|erdì)?",     // Friday (ver | ven. | venerdì)
            @"sab(?:\.|ato)?",      // Saturday (sab | sab. | sabato)
            @"do(?:\.|menica)?"     // Sunday (do | do. | domenica)                     
        };
        private static string[] patterns_nl = 
        {
            @"ma(?:\.|andag)?",     // Monday (ma | ma. | maandag)
            @"di(?:\.|nsdag)?",     // Tuesday (di | di. | dinsdag)
            @"woe(?:\.|nsdag)?",    // Wednesday (woe | woe. | woensdag)
            @"don(?:\.|derdag)?",   // Thursday (do | don. | donderdag)
            @"vrij(?:\.|dag)?",     // Friday (vrij | vrij. | vrijdag)
            @"zat(?:\.|erdag)?",    // Saturday (zat | zat. | zaterdag)
            @"zon(?:\.|dag)?"       // Sunday (zon | zon. | zondag)
        };
        private static string[] patterns_sv = 
        {
            @"Mån(?:\.|dag)?",      // Monday (Mån | Mån. | Måndag)
            @"Tis(?:\.|dag)?",      // Tuesday (Tis | Tis. | Tisdag)
            @"Ons(?:\.|dag)?",      // Wednesday (Ons | Ons. | Onsdag)
            @"Tors(?:\.|dag)?",     // Thursday (Tors | Tors. | Torsdag)
            @"Fre(?:\.|dag)?",      // Friday (Fre | Fre. | Fredag)
            @"Lör(?:\.|dag)?",      // Saturday (Lör | Lör. | Lördag)
            @"Sön(?:\.|dag)?"       // Sunday (Sön | Sön. | Söndag)
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

        public static EnumDay Match(string input, EnumLanguage language = EnumLanguage.NONE)
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
                        case 0: return EnumDay.MON;
                        case 1: return EnumDay.TUE;
                        case 2: return EnumDay.WED;
                        case 3: return EnumDay.THU;
                        case 4: return EnumDay.FRI;
                        case 5: return EnumDay.SAT;
                        case 6: return EnumDay.SUN;
                    }
                }
            }
            return EnumDay.NONE; // no match
        }
    }
}
