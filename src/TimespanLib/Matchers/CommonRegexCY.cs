using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Timespans.CommonRegex
{
    public class cy
    {
        private static string[] seasons = new string[] { 
            @"gwanwyn",             // Spring
            @"haf",                 // Summer
            @"hydref",              // Autumn
            @"gaeaf"                // Winter
        };

        // Month name (Welsh)
        private static string[] monthnames = { 
            @"Ion(?:\.|awr)?",      // Ion | Ion. | Ionawr
            @"Chwef(?:\.|ror)?",    // Chwef | Chwef. | Chwefror
            @"Maw(?:\.|rth)?",      // Maw | Maw. | Mawrth
            @"Ebr(?:\.|ill)?",      // Ebr | Ebr. | Ebrill
            @"Mai",                 // Mai
            @"Meh(?:\.|efin)?",     // Meh | Meh. | Mehefin
            @"Gorff(?:\.|ennaf)?",  // Gorff | Gorff. | Gorffennaf
            @"Awst",                // Awst
            @"Medi",                // Medi
            @"Hyd(?:\.|ref)?",      // Hyd | Hyd. | Hydref
            @"Tach(?:\.|wedd)?",    // Tach | Tach. | Tachwedd
            @"Rhag(?:\.|fyr)?"      // Rhag | Rhag. | Rhagfyr
        };

        // Day of week (Welsh)
        private static string[] daynames = {
            @"Dydd Llun",           // Monday
            @"Dydd Mawrth",         // Tuesday
            @"Dydd Mercher",        // Wednesday
            @"Dydd Iau",            // Thursday
            @"Dydd Gwener",         // Friday
            @"Dydd Sadwrn",         // Saturday
            @"Dydd Sul"             // Sunday
        };

        // Ordinals (Welsh)
        private static string[] named_ordinals = { 
            @"cyntaf",                  // first
            @"ail",                     // second
            @"trydydd",                 // third
            @"pedwerydd",               // fourth
            @"pumed",                   // fifth
            @"chweched",                // sixth
            @"seithfed",                // seventh
            @"wythfed",                 // eighth
            @"nawfed",                  // ninth
            @"degfed",                  // tenth
            @"unfed ar ddeg",           // eleventh
            @"deuddegfed",              // twelfth
            @"trydydd ar ddeg",         // thirteenth
            @"pedwerydd ar ddeg",       // fourteenth
            @"pymthegfed",              // fifteenth
            @"unfed ar bymtheg",        // sixteenth
            @"ail ar bymtheg",          // seventeenth
            @"deunawfed",               // eighteenth
            @"pedwerydd ar bymtheg",    // nineteenth
            @"ugeinfed",                // twentieth
            @"unfed ar hugain",         // twenty first
            @"ail ar hugain",           // twenty second
            @"trydydd ar hugain",       // twenty third
            @"pedwerydd ar hugain",     // twenty fourth
            @"pumed ar hugain",         // twenty fifth
            @"chweched ar hugain",      // twenty sixth
            @"seithfed ar hugain",      // twenty seventh
            @"wythfed ar hugain",       // twenty eighth
            @"nawfed ar hugain",        // twenty ninth
            @"degfed ar hugain",        // thirtieth
            @"unfed ar ddeg ar hugain"  // thirty first
        };

        // (?<ordinal>\d+)(?:af|ail|ydd|fed|ed|eg)
        private static string numeric_ordinals = Rx.group(@"\d+", "ordinal") + Rx.oneof(new string[]{ "af", "ail", "ydd", "fed", "ed", "eg" });

    }
}