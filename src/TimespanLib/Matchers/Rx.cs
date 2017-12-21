using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Timespans
{    
    public class Rx
    {
        public const string START = @"^";
        public const string SPACE = @"\s";
        public const string DIGIT = @"\d";
        public const string ANY = @".";
        public const string END = @"$";

        // subexpression grouping
        // oneof(new char[]{'A','E','I','O','U'},"+") => [AEIOU]+ note doesn't escape problematic characters...
        public static string oneof(char[] input)
        {
            return String.Concat("[", String.Join("", input), "]");
        }
        // example built expression
        public static string ROMAN = oneormore(oneof(new char[] { 'M', 'C', 'D', 'X', 'V', 'I' })); // [MCDXVI]+ 

        // oneof(new string[]{ "Tom", "Dick", "Harry"}) => (?:Tom|Dick|Harry)
        public static string oneof(string[] input, string name = "")
        {
            return group(String.Join("|", input), name);
        }

        // group("myinput", "myname", "+") => (?<myname>myinput)+
        public static string group(string input, string name = "")
        {
            return String.Concat("(?", (name != "" ? "<" + name + ">" : ":"), input, ")");
        }

        // repeaters
        public static string maybe(string input) { return repeat(input, "?"); }
        public static string zeroormore(string input) { return repeat(input, "*"); }
        public static string oneormore(string input) { return repeat(input, "+"); }
        public static string exactly(string input, int n) { return repeat(input, "{" + n.ToString() + "}"); }   // (?:input){n}
        public static string minimum(string input, int n) { return repeat(input, "{" + n.ToString() + ",}"); }  // (?:input){n,}
        public static string range(string input, int n, int m) { return repeat(input, "{" + n.ToString() + "," + m.ToString() + "}"); } // (?:value){n,m}
        private static string repeat(string input, string repeater)
        {
            input = input.Trim();
            if ((input.StartsWith("(") && input.EndsWith(")")) ||
                (input.StartsWith("[") && input.EndsWith("]")))
                return input + repeater;
            else
                return group(input) + repeater;
        }

        // TODO: call language specific versions of each function:
        // EnumDatePrefix parseDatePrefix(string input, EnumLanguage language)
        // EnumDateSuffix parseDateSuffix(string input, EnumLanguage language)
        // int parseNamedOrdinal(string input, EnumLanguage language)
        // EnumMonth parseMonthName(string input, EnumLanguage language)
        // EnumDay parseDayName(string input, EnumLanguage language)
    }
}