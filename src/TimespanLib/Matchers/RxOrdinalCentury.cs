using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TimespanLib.Rx
{
    public class OrdinalCentury : Matcher<IYearSpan> // AbstractYearSpanParser
    {        
        // done like this so they are only read once
       /* private static Lazy<IList<Ordinal>> _ordinals = new Lazy<IList<Ordinal>>(() => getOrdinals(), true);
        private static IList<Ordinal> ordinals
        {
            get
            {
                return _ordinals.Value;
            }
        }
        private static IList<Ordinal> getOrdinals()
        {
            IList<Ordinal> ordinals = null;
            string path = "";
            if (System.Web.HttpContext.Current != null)
                path = System.Web.HttpContext.Current.Request.MapPath("~\\ordinals.json");
            else
                path = "ordinals.json"; // tests don't have HttpContext, so were failing
            ordinals = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Ordinal>>(System.IO.File.ReadAllText(path));
            return ordinals;
        }
        */
        // attempt to get a match on ordinal
        /*protected int getOrdinalNoFromName(string name, EnumLanguage language)
        {
            IOrdinal match = null;
            match = ordinals.FirstOrDefault(o => o.label == name.Trim().ToLower() && (language != EnumLanguage.NONE ? o.language == language : true));
            return (match == null ? 0 : match.value);
        }      */

        private static string GetPattern(EnumLanguage language = EnumLanguage.NONE)
        {
            string pattern = "";
            switch (language)
            {
                case EnumLanguage.CY:
                    pattern = String.Concat(
                        START,
                        maybe(DateCirca.Pattern(language) + SPACE),
                        maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE),  
                        oneof(Lookup<EnumOrdinal>.Patterns(language), "ordinal"),
                        SPACE,
                        @"[cg]anrif",
                        maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),
                        END
                    );
                    break;
                case EnumLanguage.IT:
                    pattern = String.Concat(                       
                       START,                                               // ^
                       maybe(DateCirca.Pattern(language) + SPACE), // (?:(?:C(?:\.|irca)|Intorno al)\s)?
                       maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE),
                       oneof(new string[]{
                            @"sec(?:\.|olo)" + SPACE + oneof(Lookup<EnumOrdinal>.Patterns(language), "ordinal"), 
                            oneof(Lookup<EnumOrdinal>.Patterns(language), "ordinal") + SPACE +  @"sec(?:\.|olo)"
                       }),
                       maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")), // (?:\s(?<suffix>a\.?C\.?|d\.?C\.?))?
                      END                                                  // $
                   );
                    break;
                default:
                    pattern = String.Concat(
                        START,                                               // ^
                        maybe(DateCirca.Pattern(language) + SPACE), 
                        maybe(oneof(Lookup<EnumDatePrefix>.Patterns(language), "prefix") + SPACE),   // (?:
                        oneof(Lookup<EnumOrdinal>.Patterns(language), "ordinal"),
                        SPACE,
                        @"centur(?:y|ies)",
                        maybe(SPACE + oneof(Lookup<EnumDateSuffix>.Patterns(language), "suffix")),
                        END
                    );
                    break;
            }
            return pattern;
        }
       
        public static string Pattern(EnumLanguage language = EnumLanguage.NONE, string groupname = "")
        {
            return group(GetPattern(language), groupname);
        }

        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            return (Regex.IsMatch(input.Trim(), Pattern(language), options));
        }


        // "THIRD CENTURY AD"
        public static IYearSpan Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            // using vanilla Regex:
            // string pattern = @"^(?<circa>C(?:irca|.)?)?\s*(?<ordinal>FIRST|SECOND|THIRD|FOURTH|FIFTH|SIXTH|SEVENTH|EIGHTH|NINTH|TENTH|ELEVENTH|TWELFTH|THIRTEENTH|FOURTEENTH|FIFTEENTH|SIXTEENTH|SEVENTEENTH|EIGHTEENTH|NINETEENTH|TWENTIETH|TWENTY FIRST)\s+(?:CENTURY|CENTURIES)\s*(?<qualifier>AD|BC|BP|na Chr)?$";
            // Match m = Regex.Match(input, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            // using VerbalExpressions:
            /*VerbalExpressions ve = new VerbalExpressions()
                .StartOfLine()
                .Maybe(oneof(en.datecircapatterns), false)                
                .Then(@"\s*", false)
                .BeginCapture("prefix")
                .Maybe(oneof(en.dateprefixpatterns), false)
                .EndCapture()
                .Then(@"\s*", false)
                .BeginCapture("ordinal")
                .Then(oneof(en.named_ordinals), false)
                .EndCapture()
                .Then(@"\s+century\s*", false)
                .BeginCapture("qualifier")
                .Maybe(oneof(en.datesuffixpatterns), false)
                .EndCapture()
                .EndOfLine()
                .WithOptions(options);
            Match m = ve.ToRegex().Match(input.Trim());

            
            if (!m.Success) return null;*/

            string pattern = GetPattern(language);
            
            Match m = Regex.Match(input.Trim(), pattern, options);
            if (!m.Success) return null;
                        
            int centuryNo = m.Groups["ordinal"] != null ? (int)Lookup<EnumOrdinal>.Match(m.Groups["ordinal"].Value, language) : 0;
            EnumDatePrefix prefix = m.Groups["prefix"] != null ? Lookup<EnumDatePrefix>.Match(m.Groups["prefix"].Value, language) : EnumDatePrefix.NONE;
            EnumDateSuffix suffix = m.Groups["suffix"] != null ? Lookup<EnumDateSuffix>.Match(m.Groups["suffix"].Value, language) : EnumDateSuffix.NONE;      
            
            IYearSpan span = getCenturyYearSpan(centuryNo, prefix, suffix);
            span.label = input.Trim();
            span.note = "RxOrdinalCentury";
            return span;                        
            
        }                
    }
}
