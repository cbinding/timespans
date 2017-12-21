using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace TimespanLib.Rx
{
    // type specific matchers (can then implement EnumDay matcher, EnumMonth matcher, int matcher etc) 
    /*public interface IMatcher<T>
    {
        //string[] Patterns(EnumLanguage language);
        string Pattern(EnumLanguage language);
        string Pattern(EnumLanguage language, string groupname);
        T Match(string input, EnumLanguage language);
        bool IsMatch(string input, EnumLanguage language);
    }*/

    public abstract class Matcher<T> //: IMatcher<T> // was AbstractYearSpanParser - now all parse/match classes are RxMatcher<T>
    {
        // characters
        protected internal const string START = @"^";
        protected internal const string SPACE = @"\s";
        protected internal const string NONSPACE = @"\S";
        protected internal const string DIGIT = @"\d";
        protected internal const string NONDIGIT = @"\D";
        protected internal const string WORD = @"\w";
        protected internal const string NONWORD = @"\W";
        protected internal const string ANY = @".";
        protected internal const string END = @"$";
        protected internal const string ROMAN = @"[MDCLXVI]+";
        protected internal const int PRESENT = 1950; // benchmark point for calculation of BP dates. See https://en.wikipedia.org/wiki/Before_Present
        // However have seen 'present' also refer to 2000 - maybe this would need to be in config file or passed in option to override default??
        
        // repeaters
        // exactly(2) => "{2}"
        protected internal static string exactly(int n) 
        { 
            return String.Concat("{", n.ToString(), "}"); 
        }

        // minimum(2) => "{2,}"
        protected internal static string minimum(int n) 
        {
            return range(n); 
        }

        // range(2, 5) => "{2,5}"
        protected internal static string range(int min, int max = -1) 
        {

            return String.Concat("{", min.ToString(), ",", (max > -1 ? max.ToString() : ""), "}"); 
        }

        // groups
        // syntactic sugar: maybe("myinput", "myname") => "(?<myname>myinput)?"
        protected internal static string maybe(string input, string name = "")
        {
            return group(input, name, "?");
        }
        protected internal static string maybe(string[] input, string name = "")
        {
            return oneof(input, name, "?");
        }

        // oneof(new string[]{ "Tom", "Dick", "Harry" }, "name") => "(?<name>Tom|Dick|Harry)"
        protected internal static string oneof(string[] input, string name = "", string repeat = "")
        {
            return group(String.Join("|", input), name, repeat);
        }

        // group("myinput", "myname", "+") => "(?<myname>myinput)+"
        protected internal static string group(string input, string name = "", string repeat = "")
        {
            input = input.Trim();
            name = name.Trim();
            repeat = repeat.Trim();

            return String.Concat(
               "(?",
               (name != "" ? "<" + name + ">" : ":"),
               input,
               ")",
               repeat
            );
        }
        
        // default options for regex matching (can be ignored if required)
        protected internal static RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;

        //public virtual string[] Patterns(EnumLanguage language) { throw new NotImplementedException(); }
        //public static string Pattern(EnumLanguage language) { throw new NotImplementedException(); }
        // public static string Pattern(EnumLanguage language, string groupname) { throw new NotImplementedException(); }
        // public static T Match(string input, EnumLanguage language) { throw new NotImplementedException(); }
        //public static bool IsMatch(string input, EnumLanguage language) { throw new NotImplementedException(); }
                
        // get span representing numbered century e.g. century 5 = { min: 401, max: 500 } 
        // code shared by both OrdinalNamedCentury and OrdinalNumberedCentury 
        protected internal static IYearSpan getCenturyYearSpan(int centuryNo, EnumDatePrefix prefix = EnumDatePrefix.NONE, EnumDateSuffix suffix = EnumDateSuffix.NONE)
        {
            // data cleansing of input parameters						
            int yearMin = 0, yearMax = 0;

            // adjust boundaries if E/M/L qualifier is present using
            // (invented) boundaries: EARLY=1-40, MID=30-70, LATE=60-100
            switch (suffix)
            {
                case EnumDateSuffix.BC:
                case EnumDateSuffix.BCE:
                    {
                        yearMin = (centuryNo * -100);
                        switch (prefix)
                        {
                            case EnumDatePrefix.HALF1:
                                yearMax = yearMin + 50;
                                break;
                            case EnumDatePrefix.HALF2:
                                yearMin += 50;
                                yearMax = yearMin + 49;
                                break;
                            case EnumDatePrefix.EARLY:
                                yearMax = yearMin + 40;
                                break;
                            case EnumDatePrefix.MID:
                                yearMin += 30;
                                yearMax = yearMin + 40;
                                break;
                            case EnumDatePrefix.LATE:
                                yearMin += 60;
                                yearMax = yearMin + 39;
                                break;
                            case EnumDatePrefix.QUARTER1:
                                yearMax = yearMin + 25;
                                break;
                            case EnumDatePrefix.QUARTER2:
                                yearMin += 25;
                                yearMax = yearMin + 25;
                                break;
                            case EnumDatePrefix.QUARTER3:
                                yearMin += 50;
                                yearMax = yearMin + 25;
                                break;
                            case EnumDatePrefix.QUARTER4:
                                yearMin += 75;
                                yearMax = yearMin + 24;
                                break;
                            default:
                                // There is no year zero...
                                yearMax = yearMin + 99;
                                break;
                        }
                        break;
                    }
                case EnumDateSuffix.BP:
                    {
                        yearMin = PRESENT - (centuryNo * 100) + 1;
                        switch (prefix)
                        {
                            case EnumDatePrefix.HALF1:
                                yearMax = yearMin + 50;
                                break;
                            case EnumDatePrefix.HALF2:
                                yearMin += 50;
                                yearMax = yearMin + 49;
                                break;
                            case EnumDatePrefix.EARLY:
                                yearMax = yearMin + 40;
                                break;
                            case EnumDatePrefix.MID:
                                yearMin += 30;
                                yearMax = yearMin + 40;
                                break;
                            case EnumDatePrefix.LATE:
                                yearMin += 60;
                                yearMax = yearMin + 39;
                                break;
                            case EnumDatePrefix.QUARTER1:
                                yearMax = yearMin + 25;
                                break;
                            case EnumDatePrefix.QUARTER2:
                                yearMin += 25;
                                yearMax = yearMin + 25;
                                break;
                            case EnumDatePrefix.QUARTER3:
                                yearMin += 50;
                                yearMax = yearMin + 25;
                                break;
                            case EnumDatePrefix.QUARTER4:
                                yearMin += 75;
                                yearMax = yearMin + 24;
                                break;
                            default:
                                // There is no year zero...
                                yearMax = yearMin + 99;
                                break;
                        }
                        break;
                    }
                default: // AD, CE or NONE
                    {
                        yearMin = (centuryNo * 100) - 99;
                        switch (prefix)
                        {
                            case EnumDatePrefix.HALF1:
                                yearMax = yearMin + 49;
                                break;
                            case EnumDatePrefix.HALF2:
                                yearMin += 49;
                                yearMax = yearMin + 50;
                                break;
                            case EnumDatePrefix.EARLY:
                                yearMax = yearMin + 39;
                                break;
                            case EnumDatePrefix.MID:
                                yearMin += 29;
                                yearMax = yearMin + 40;
                                break;
                            case EnumDatePrefix.LATE:
                                yearMin += 59;
                                yearMax = yearMin + 40;
                                break;
                            case EnumDatePrefix.QUARTER1:
                                yearMax = yearMin + 24;
                                break;
                            case EnumDatePrefix.QUARTER2:
                                yearMin += 24;
                                yearMax = yearMin + 25;
                                break;
                            case EnumDatePrefix.QUARTER3:
                                yearMin += 49;
                                yearMax = yearMin + 25;
                                break;
                            case EnumDatePrefix.QUARTER4:
                                yearMin += 74;
                                yearMax = yearMin + 25;
                                break;
                            default:
                                yearMax = yearMin + 99;
                                break;
                        }
                        break;
                    }
            }
            // TODO: not currently accounting for earlymid, or midlate
            return new YearSpan(yearMin, yearMax);
        }

        protected internal static IYearSpan getMillenniumYearSpan(int millenniumNo, EnumDatePrefix prefix = EnumDatePrefix.NONE, EnumDateSuffix suffix = EnumDateSuffix.NONE)
        {
            // data cleansing of input parameters						
            int yearMin = 0, yearMax = 0;

            // adjust boundaries if E/M/L qualifier is present using
            // (invented) boundaries: EARLY=1-40, MID=30-70, LATE=60-100
            switch (suffix)
            {
                case EnumDateSuffix.BC:
                case EnumDateSuffix.BCE:
                    {
                        yearMin = (millenniumNo * -1000);
                        switch (prefix)
                        {
                            case EnumDatePrefix.HALF1:
                                yearMax = yearMin + 500;
                                break;
                            case EnumDatePrefix.HALF2:
                                yearMin += 500;
                                yearMax = yearMin + 499;
                                break;
                            case EnumDatePrefix.EARLY:
                                yearMax = yearMin + 400;
                                break;
                            case EnumDatePrefix.MID:
                                yearMin += 300;
                                yearMax = yearMin + 400;
                                break;
                            case EnumDatePrefix.LATE:
                                yearMin += 600;
                                yearMax = yearMin + 399;
                                break;
                            case EnumDatePrefix.QUARTER1:
                                yearMax = yearMin + 250;
                                break;
                            case EnumDatePrefix.QUARTER2:
                                yearMin += 250;
                                yearMax = yearMin + 250;
                                break;
                            case EnumDatePrefix.QUARTER3:
                                yearMin += 500;
                                yearMax = yearMin + 250;
                                break;
                            case EnumDatePrefix.QUARTER4:
                                yearMin += 750;
                                yearMax = yearMin + 249;
                                break;
                            default:
                                // There is no year zero...
                                yearMax = yearMin + 999;
                                break;
                        }
                        break;
                    }
                case EnumDateSuffix.BP:
                    {
                        yearMin = PRESENT - (millenniumNo * 1000) + 1;
                        switch (prefix)
                        {
                            case EnumDatePrefix.HALF1:
                                yearMax = yearMin + 500;
                                break;
                            case EnumDatePrefix.HALF2:
                                yearMin += 500;
                                yearMax = yearMin + 499;
                                break;
                            case EnumDatePrefix.EARLY:
                                yearMax = yearMin + 400;
                                break;
                            case EnumDatePrefix.MID:
                                yearMin += 300;
                                yearMax = yearMin + 400;
                                break;
                            case EnumDatePrefix.LATE:
                                yearMin += 600;
                                yearMax = yearMin + 399;
                                break;
                            case EnumDatePrefix.QUARTER1:
                                yearMax = yearMin + 250;
                                break;
                            case EnumDatePrefix.QUARTER2:
                                yearMin += 250;
                                yearMax = yearMin + 250;
                                break;
                            case EnumDatePrefix.QUARTER3:
                                yearMin += 500;
                                yearMax = yearMin + 250;
                                break;
                            case EnumDatePrefix.QUARTER4:
                                yearMin += 750;
                                yearMax = yearMin + 249;
                                break;
                            default:
                                // There is no year zero...
                                yearMax = yearMin + 999;
                                break;
                        }
                        break;
                    }
                default: // AD, CE or NONE
                    {
                        yearMin = (millenniumNo * 1000) - 999;
                        switch (prefix)
                        {
                            case EnumDatePrefix.HALF1:
                                yearMax = yearMin + 499;
                                break;
                            case EnumDatePrefix.HALF2:
                                yearMin += 499;
                                yearMax = yearMin + 500;
                                break;
                            case EnumDatePrefix.EARLY:
                                yearMax = yearMin + 399;
                                break;
                            case EnumDatePrefix.MID:
                                yearMin += 299;
                                yearMax = yearMin + 400;
                                break;
                            case EnumDatePrefix.LATE:
                                yearMin += 599;
                                yearMax = yearMin + 400;
                                break;
                            case EnumDatePrefix.QUARTER1:
                                yearMax = yearMin + 249;
                                break;
                            case EnumDatePrefix.QUARTER2:
                                yearMin += 249;
                                yearMax = yearMin + 250;
                                break;
                            case EnumDatePrefix.QUARTER3:
                                yearMin += 499;
                                yearMax = yearMin + 250;
                                break;
                            case EnumDatePrefix.QUARTER4:
                                yearMin += 749;
                                yearMax = yearMin + 250;
                                break;
                            default:
                                yearMax = yearMin + 999;
                                break;
                        }
                        break;
                    }
            }
            // TODO: not currently accounting for earlymid, or midlate
            return new YearSpan(yearMin, yearMax);
        }
    }
}