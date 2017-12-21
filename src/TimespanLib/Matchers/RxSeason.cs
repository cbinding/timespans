using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Timespans.Rx
{  
    public class Season : Matcher<EnumSeason>
    {
        private static string[] patterns_cy = 
        { 
            @"(?:g|ng)?wanwyn",         // Spring (with mutations)
            @"haf",                     // Summer
            @"hydref",                  // Autumn
            @"(?:c|g|ng|ch|ngh)?aeaf"   // Winter 
        };
        private static string[] patterns_de = 
        {
            @"Frühling",   // Spring     
            @"Sommer",     // Summer 
            @"Herbst",     // Autumn  
            @"Winter"      // Winter           
        };
        private static string[] patterns_en = 
        {             
            @"Spring",      // Spring
            @"Summer",      // Summer
            @"Autumn",      // Autumn  
            @"(?:Winter|Fall)" // Winter | Fall       
        };
        private static string[] patterns_es = 
        {
            @"Primavera",   // Spring
            @"Verano",      // Summer
            @"Otoño",       // Autumn
            @"Invierno"     // Winter           
        };
        private static string[] patterns_fr = 
        {
            @"Printemps",   // Spring    
            @"Été",         // Summer
            @"L\'automne",  // Autumn      
            @"Hiver"        // Winter      
        };
        private static string[] patterns_it = 
        {
            @"Primavera",   // Spring     
            @"Estate",      // Summer 
            @"Autunno",     // Autumn     
            @"Inverno"      // Winter                  
        };
        private static string[] patterns_nl = 
        {
            @"De lente",    // Spring
            @"Zomer",       // Summer
            @"Herfst",      // Autumn
            @"Winter"       // Winter      
        };
        private static string[] patterns_sv = 
        {
            @"Vår",         // Spring
            @"Sommar",      // Summer
            @"Höst",        // Autumn
            @"Vinter"       // Winter
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
            return oneof(Patterns(language), groupname); // (?<groupname>Spring|Summer|Autumn|(?:Winter|Fall))
        }

        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE) 
        {
            return (Regex.IsMatch(input.Trim(), Pattern(language), options));
        }

        public static EnumSeason Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            input = input.Trim();
            string[] patterns = Patterns(language);

            for (int i = 0; i < patterns.Length; i++)
            {
                if (Regex.IsMatch(input, patterns[i], options))
                {
                    switch (i)
                    {
                        case 0: return EnumSeason.SPRING;
                        case 1: return EnumSeason.SUMMER;
                        case 2: return EnumSeason.AUTUMN;
                        case 3: return EnumSeason.WINTER;
                    }
                }
            }
            return EnumSeason.NONE; // no match
        }
    }
}