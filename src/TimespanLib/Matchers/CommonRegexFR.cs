using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Timespans.CommonRegex
{
    public class fr
    {
        // Month names (French)
        private static string[] monthnames = new string[] {     
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

        // Ordinals (French)
        private static string[] named_ordinals = new string[] { 
            @"premiere?",   // premier | premiere
            @"deuxième",
            @"seconde?",     // second | seconde
            @"troisième",
            @"quatrième",
            @"cinquième",
            @"sixième",
            @"septième",
            @"huitième",
            @"neuvième",
            @"dixième",
            @"onzième",
            @"douzième",
            @"treizième",
            @"quatorzième",
            @"quinzième",
            @"seizième",
            @"dix\-septième",
            @"dix\-huitième",
            @"dix\-neuvième",
            @"vingtième",
            @"vingt et unième"
        };

        public static readonly string[] seasonnamepatterns = new string[] {
            @"Printemps",     
            @"Été",   
            @"L\'automne",       
            @"Hiver"            
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