using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace TimespanLib.Rx
{
    public class DateTimeMatch : Matcher<IYearSpan> //AbstractYearSpanParser
    {
        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            DateTime dt;
            return DateTime.TryParse(input.Trim(), out dt);
        } 
        // parse year from a valid string date expression
        // input: "1571-05-01", "01/05/1571"
        // output: { min: 1571, max: 1571, label: "01/05/1571" } 
        public static IYearSpan Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
            {
                return new YearSpan(dt.Year, input, "RxDateTimeMatch");
            }
            else return null;
        }
    }
}