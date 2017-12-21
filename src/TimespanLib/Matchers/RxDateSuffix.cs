using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Timespans.Rx
{
    public class DateSuffix : Matcher<EnumDateSuffix>
        {
            private static string[] patterns_cy = 
            { 
                @"(?:O\.?C\.?|A\.?D\.?)",   // (OC|O.C|O.C.|OC.|AD|A.D|A.D.|AD.)  (ar ôl Crist) 
                @"(?:C\.?C\.?|B\.?C\.?)",   // (CC|C.C|C.C.|CC.|BC|B.C|B.C.|BC.)  (cyn Crist)
                @"B\.?P\.?",                // BP|B.P|B.P.|BP. (maybe CP - cyn y presennol? check)
                @"C\.?E\.?"                 // CE|C.E|C.E.|CE. 
            };
            private static string[] patterns_de = 
            {
                @"(?:n\.?\sChr|A\.?D\.?)",  // AD|A.D|A.D.|AD. 
                @"(?:v\.?\sChr|B\.?C\.?)",  // BC|B.C|B.C.|BC.                
                @"B\.?P\.?",            // BP|B.P|B.P.|BP.
                @"C\.?E\.?"             // CE|C.E|C.E.|CE.
            };
            private static string[] patterns_en = 
            {   
                @"A\.?D\.?",            // AD|A.D|A.D.|AD.  
                @"B\.?C\.?",            // BC|B.C|B.C.|BC.
                @"B\.?P\.?",            // BP|B.P|B.P.|BP.
                @"C\.?E\.?"             // CE|C.E|C.E.|CE.              
            };
            private static string[] patterns_es = 
            {
                @"A\.?D\.?",            // AD|A.D|A.D.|AD.  
                @"a\.?C\.?",            // aC|a.C|a.C.|aC.                
                @"B\.?P\.?",            // BP|B.P|B.P.|BP.
                @"C\.?E\.?"             // CE|C.E|C.E.|CE. 
            };
            private static string[] patterns_fr = 
            {
                @"(?:J\.?C\.?|A\.?D\.?)",   // AD|A.D|A.D.|AD.   
                @"(?:J\.?\-C\.?|B\.?C\.?)", // BC|B.C|B.C.|BC.                
                @"B\.?P\.?",                // BP|B.P|B.P.|BP.
                @"C\.?E\.?"                 // CE|C.E|C.E.|CE.   
            };
            private static string[] patterns_it = 
            {
                @"a\.?C\.?",        // aC | aC. | a.C | a.C.  (BC)
                @"d\.?C\.?",        // dC | dC. | d.C | d.C.  (AD)
                @"B\.?P\.?",        // BP|B.P|B.P.|BP.
                @"C\.?E\.?"         // CE|C.E|C.E.|CE.

            };
            private static string[] patterns_nl = 
            {
                @"A\.?D\.?",            // AD|A.D|A.D.|AD.   
                @"(?:v\.Chr\.|voor Christus)", // voor Christus (BC)
                @"B\.?P\.?",            // BP|B.P|B.P.|BP.
                @"C\.?E\.?"             // CE|C.E|C.E.|CE.
            };
            private static string[] patterns_sv = 
            {
                @"(?:e\.?Kr\.?|A\.?D\.?)",  // AD|A.D|A.D.|AD.  
                @"(?:f\.?Kr\.?|B\.?C\.?)",  // BC|B.C|B.C.|BC.
                @"B\.?P\.?",                // BP|B.P|B.P.|BP.
                @"C\.?E\.?"                 // CE|C.E|C.E.|CE.    
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

            public static EnumDateSuffix Match(string input, EnumLanguage language = EnumLanguage.NONE)
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
                            case 0: return EnumDateSuffix.BC;
                            case 1: return EnumDateSuffix.AD;
                            case 2: return EnumDateSuffix.BP;
                            case 3: return EnumDateSuffix.CE; 
                        }
                    }
                }
                return EnumDateSuffix.NONE; // no match
            }
        }
    }
