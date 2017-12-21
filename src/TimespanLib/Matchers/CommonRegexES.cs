using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Timespans.CommonRegex
{
    public class es
    {
        // Month names (Spanish)
        private static string[] monthnames = new string[] {              
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

        // Ordinals (Spanish)
        private static string[] named_ordinals = new string[] { 
            @"primero",         // first
            @"segund(?:o|a)",   // second
            @"tercero",         // third
            @"cuarto",          // fourth
            @"quinto",          // fifth
            @"sexto",           // sixth
            @"séptimo",         // seventh
            @"octavo",          // eighth
            @"noveno",          // ninth
            @"décimo",          // tenth
            @"undécimo",        // eleventh        
            @"(?:duodécimo|doceavo|dozavo)", // twelfth
            @"decimoterc(?:io|ero)",  // thirteenth  
            @"decimocuarto",    // fourteenth
            @"decimoquinto",    // fifteenth
            @"decimosexto",     // sixteenth
            @"dieciséis",       // sixteenth
            @"decimoséptimo",   // seventeenth
            @"decimoctavo",     // eighteenth
            @"decimonoveno",    // nineteenth
            @"vigésimo",        // twentieth
            @"vigésimo primero" // twenty first
        };

        public static readonly string[] seasonnamepatterns = new string[] {
            @"Primavera",       // Spring
            @"Verano",          // Summer
            @"Otoño",           // Autumn
            @"Invierno"         // Winter           
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
    }
}