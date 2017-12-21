using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Timespans.Rx
{
    public class NamedPeriodOld : Matcher<IYearSpan> 
    {
        // done like this so they are only read once
        private static Lazy<IList<YearSpan>> _namedPeriodsEN = new Lazy<IList<YearSpan>>(() => getNamedPeriods(EnumLanguage.EN), true);
        private static Lazy<IList<YearSpan>> _namedPeriodsNL = new Lazy<IList<YearSpan>>(() => getNamedPeriods(EnumLanguage.NL), true);
        private static Lazy<IList<YearSpan>> _namedPeriodsIT = new Lazy<IList<YearSpan>>(() => getNamedPeriods(EnumLanguage.IT), true);
        private static IList<YearSpan> namedPeriodsEN
        {
            get
            {
                return _namedPeriodsEN.Value;
            }
        }
        private static IList<YearSpan> namedPeriodsNL
        {
            get
            {
                return _namedPeriodsNL.Value;
            }
        }
        private static IList<YearSpan> namedPeriodsIT
        {
            get
            {
                return _namedPeriodsIT.Value;
            }
        }

        private static IList<YearSpan> getNamedPeriods(EnumLanguage language = EnumLanguage.NONE)
        {
            IList<YearSpan> namedPeriods = null;
            string path = "";

            switch (language)
            {
                case EnumLanguage.IT:
                    path = @"fasti-named-periods.json";
                    break;
                case EnumLanguage.NL:
                    path = @"dans-named-periods.json";
                    break;
                default:
                    path = @"eh-named-periods.json";
                    break;
            }

            // string path = System.Web.HttpContext.Current.Request.MapPath("~\\eh-named-periods.json");            
            if (System.Web.HttpContext.Current != null)
                path = System.Web.HttpContext.Current.Request.MapPath("~\\" + path);
            
            string text = System.IO.File.ReadAllText(path);
            namedPeriods = Newtonsoft.Json.JsonConvert.DeserializeObject<List<YearSpan>>(text);
            foreach (IYearSpan span in namedPeriods)
            {
                span.label = span.label.Trim().ToUpper(); // for comparison purposes in match...
            }
            return namedPeriods;
        }

        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            IYearSpan match = Match(input.Trim(), language);
            return (match != null && match.min != 0 && match.max != 0);
        }

        // input: "Georgian" (NOTE: this is just a lookup - different to Regex parsers...)
        // output: { min: 1714, max: 1837, label: "Georgian" } TODO: other languages (and give dates to periods in JSON files...)
        public static IYearSpan Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            // attempt to match on named period (returns null if not matched)        
            IYearSpan match = null;
            switch (language) 
            {
                case EnumLanguage.IT:
                    match = namedPeriodsIT.FirstOrDefault(o => o.label == input.Trim().ToUpper());
                    break;
                case EnumLanguage.NL:
                    match = namedPeriodsNL.FirstOrDefault(o => o.label == input.Trim().ToUpper());
                    break;
                default:
                    match = namedPeriodsEN.FirstOrDefault(o => o.label == input.Trim().ToUpper());
                    break;
            }
            if(match != null) match.note = "RxNamedPeriod";
            return match;            
        }
    }
}