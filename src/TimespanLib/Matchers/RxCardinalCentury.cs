using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
//using CSharpVerbalExpressions; // modular regular expression builder

namespace TimespanLib.Rx
{    
    public class CardinalCentury : Matcher<IYearSpan> //AbstractYearSpanParser
    {
        private static string GetPattern(EnumLanguage language = EnumLanguage.NONE)
        {
            string pattern = "";
            switch (language)
            {
                case EnumLanguage.CY:
                    {
                         pattern = String.Concat(
                            START,
                            maybe(DateCirca.Pattern(language) + SPACE),
                            maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE), 
                            group(@"\d+", "century"),
                            oneof(new string[]{ "af", @"a?il", "ain", "ydd", @"f?ed", "eg" }),
                            SPACE,
                            "[cg]anrif",
                            maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),
                            END
                        );
                        break;
                    }
                case EnumLanguage.DE:
                    { 
                        pattern = String.Concat(
                            START,
                            maybe(DateCirca.Pattern(language) + SPACE),
                            maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE),
                            group(@"\d+", "century"),
                            @"\.",
                            SPACE,
                            "Jahrhundert",
                            END
                        );
                        break;
                    }
                case EnumLanguage.IT:
                    {
                        // ^(?:(?:C(?:\.|irca)|Intorno al)\s)?(?<millennium>[MCDLXVI]+)\s?(?:esimo|mo|°|º)?\smillennio(?:\s(?<suffix>a\.?C\.?|d\.?C\.?))?$
                        // eg XI secolo | secolo XI | XIesimo secolo | XImo secolo | 11 ° secolo                        
                        pattern = String.Concat(
                            START,                                               // ^
                            maybe(DateCirca.Pattern(language) + SPACE), // (?:(?:C(?:\.|irca)|Intorno al)\s)?
                            maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE),                            
                            oneof(new string[] {
                                String.Concat(
                                     @"sec(?:\.|olo)",
                                     SPACE,
                                     group(ROMAN, "century")         
                                ),
                                String.Concat(
                                     group(ROMAN, "century"),
                                     SPACE,
                                     @"sec(?:\.|olo)"                                     
                                ),
                                String.Concat(
                                     group(@"\d+", "century"),
                                     maybe(SPACE), 
                                     maybe(oneof(new string[] { "esimo", "mo", "°", "º" })), // (?:esimo|mo|°|º)?   
                                     SPACE,
                                     @"sec(?:\.|olo)"                                     
                                ),
                                group(ROMAN, "century")
                            }),
                            maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")), // (?:\s(?<suffix>a\.?C\.?|d\.?C\.?))?
                            END                                                  // $
                        );
                        break;
                    }
                case EnumLanguage.FR:
                    {
                        // 11e siècle | XIe siècle av. J-C. | apr. J-C.
                        // pattern = @"^(?<century>[MCDLXVI]+|\d+)e\s+siècle(?:\s+(?<suffix>av\.\sJ\-C\.|apr\.\sJ\-C\.))?$";
                        pattern = String.Concat(
                            START,
                            maybe(DateCirca.Pattern(language) + SPACE),
                            maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE),
                            oneof(new string[]{ROMAN, @"\d+"},"century"),
                            oneof(new string[] { "è?[mr]e", "er", "re", "[eè]", "n?de?", "o" }),
                            SPACE,
                            @"s(?:\.|iècle)",
                            maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),
                            END
                        );
                        break;
                    }
                case EnumLanguage.ES:
                    {
                        // XI siglo | siglo XI
                        //pattern = @"^(?:siglo\s+)?(?<century>[MCDLXVI]+|\d+)(?:\s+siglo)?$";
                        pattern = String.Concat(
                            START,
                            maybe(DateCirca.Pattern(language) + SPACE),
                            maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE),
                            maybe("Siglo" + SPACE),
                            oneof(new string[] { ROMAN, @"\d+" }, "century"),
                            maybe(SPACE + "Siglo"),
                            maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),
                            END
                        );
                        break;
                    }
                case EnumLanguage.NL:
                    {
                        pattern = String.Concat( 
                            START,                                     
                            maybe(DateCirca.Pattern(language) + SPACE),
                            maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE),                             
                            group(@"\d+", "century"),                             
                            oneof(new string[] { "e", "ste", "de" }),
                            SPACE,
                            "eeuw",                                
                            maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),
                            END
                        );
                        break;
                    }
                case EnumLanguage.SV:
                    {
                        // 1700-talet | 18:e århundradet | 18:e seklet | 1700-tal
                        pattern = String.Concat(
                            START,
                            maybe(DateCirca.Pattern(language) + SPACE),
                            maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE),
                            oneof(new string[] {
                                String.Concat(
                                    group(@"\d+", "century"),
                                    oneof(new string[] { ":a", ":e" }),
                                    SPACE,
                                    oneof(new string[] { "århundradet", "seklet" })
                                ),
                                String.Concat(
                                    group(@"\d+00", "century"),
                                    @"-tal",
                                    maybe("et")                                    
                                )
                            }),
                            maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),
                            END
                        );
                        break;
                    }
                default:
                    {
                        //pattern = @"^(?:c(?:irca|\.)?\s+)?(?<centuryNo>\d+)(?:st|nd|rd|th)\s+(?:century|centuries)(?:\s+(?<suffix>ad|bc|ce))?$";
                        pattern = String.Concat(
                            START,                                            // ^
                            maybe(DateCirca.Pattern(language) + SPACE),
                            maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE),   // (?:(?:C(?:\.|irca)|Intorno al)\s)?                            
                            group(@"\d+", "century"),                        // (?<century>\d+)
                            oneof(new string[] { "st", "nd", "rd", "th" }),
                            SPACE,
                            "century",                                         // century
                            maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")), // (?:\s(?<suffix>A\.?D\.?|B\.?C\.?))?
                            END                                             // $
                        );
                        break;
                    }
            }
            return pattern;
        }
         
        public static string Pattern(EnumLanguage language = EnumLanguage.NONE, string groupname = "")
        {
            return group(GetPattern(language), groupname);
        }
        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            return Regex.IsMatch(input.Trim(), Pattern(language), options);
        } 

        //public override IYearSpan Match(string input, EnumLanguage language)
        public static IYearSpan Match(string input, EnumLanguage language = EnumLanguage.NONE)        
        {
            string pattern = GetPattern(language);
            Match m = Regex.Match(input.Trim(), pattern, options);
            // no patterns matched?
            if (!m.Success) return null;

            // if we reach here we matched so get the represented year span 
            string centuryString = m.Groups["century"].Value.Trim().ToUpper();
            int centuryNo = Regex.IsMatch(centuryString, ROMAN) ? RomanToNumber.Parse(centuryString) : Int32.Parse(centuryString);
            EnumDatePrefix prefix = m.Groups["prefix"] != null ? Lookup<EnumDatePrefix>.Match(m.Groups["prefix"].Value, language) : EnumDatePrefix.NONE;
            EnumDateSuffix suffix = m.Groups["suffix"] != null ? Lookup<EnumDateSuffix>.Match(m.Groups["suffix"].Value, language) : EnumDateSuffix.NONE;

            IYearSpan span = getCenturyYearSpan(centuryNo, prefix, suffix);    
            span.label = input.Trim();
            span.note = "RxCardinalCentury"; //this.GetType().Name;
            return span;
        }

       /*
        // "3rd CENTURY AD"
        public  IYearSpan ParseOld(string input, EnumLanguage language)
        {
            // using vanilla Regex:       
            // string pattern = @"^(?:c(?:\.|irca)?)?\s*(?<centuryNo>\d+)(?:ST|ND|RD|TH|E)\s+(?:CENTURY|CENTURIES|EEUW)\s*(?<qualifier>AD|BC)?$";
            // Match m = Regex.Match(input, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            
            // using VerbalExpressions:
            VerbalExpressions ve = new VerbalExpressions()
                .StartOfLine()
                .Maybe(oneof(RxDateCirca.Patternsen.datecircapatterns), false)
                .Then(@"\s*", false)
                .BeginCapture("prefix").Maybe(oneof(en.dateprefixpatterns), false).EndCapture()
                .Then(@"\s*", false)
                .BeginCapture("centuryNo").Then(@"\d{1,2}", false).EndCapture()
                .Then(@"(?:st|nd|rd|th)\s+(?:century|eeuw)\s*", false)
                .BeginCapture("qualifier").Maybe(oneof(en.datesuffixpatterns), false).EndCapture()
                .EndOfLine()
                .WithOptions(RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Match m = ve.ToRegex().Match(input.Trim());
            if (!m.Success) return null;
            
            int centuryNo = 0;
            int.TryParse(m.Groups["centuryNo"].Value, out centuryNo);
            
            CenturyPrefixEnum prefix;
            if (!Enum.TryParse<CenturyPrefixEnum>(m.Groups["prefix"].Value.Trim(), true, out prefix))
                prefix = CenturyPrefixEnum.NONE;

            YearQualifierEnum qualifier;
            if (!Enum.TryParse<YearQualifierEnum>(m.Groups["qualifier"].Value.Trim(), true, out qualifier))
                qualifier = YearQualifierEnum.NONE;
            
            IYearSpan span = getCenturyYearSpan(centuryNo, qualifier, prefix); //TODO - use qualifier    
            span.label = input.Trim();
            span.note = this.GetType().Name;
            return span;
        }*/
    }
}
