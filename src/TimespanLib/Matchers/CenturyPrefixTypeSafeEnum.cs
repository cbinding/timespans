using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Timespans
{
    /// <summary>
    /// This class is used to fake an enum. You'll be able to use it as an enum.
    /// Note: type save enum, found on stackoverflow: http://stackoverflow.com/a/424414/603309
    /// </summary>
    public sealed class CenturyPrefixTypeSafeEnum
    {   
        #region Private Members
        private readonly String pattern;
        private readonly LanguageEnum language;
        #endregion Private Members

        public static CenturyPrefixTypeSafeEnum Parse(string input, LanguageEnum language = LanguageEnum.NONE)
        {
            CenturyPrefixTypeSafeEnum result = CenturyPrefixTypeSafeEnum.NONE;
            RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase;
            switch (language)
            {
                case LanguageEnum.IT:
                    {
                        if (Regex.IsMatch(input, @"^Prima Età", options))
                            result = CenturyPrefixTypeSafeEnum.EARLY;
                        else if (Regex.IsMatch(input, @"^Tarda Età", options))
                            result = CenturyPrefixTypeSafeEnum.LATE;
                        else if (Regex.IsMatch(input, @"^(?:1\s?°|prima) metà del", options))
                            result = CenturyPrefixTypeSafeEnum.HALF1;
                        else if (Regex.IsMatch(input, @"^(?:2\s?°|seconda) metà del", options))
                            result = CenturyPrefixTypeSafeEnum.HALF2;
                        else if (Regex.IsMatch(input, @"^(?:1\s?°|primo) quarto del", options))
                            result = CenturyPrefixTypeSafeEnum.QUARTER1;
                        else if (Regex.IsMatch(input, @"^(?:2\s?°|secondo) quarto del", options))
                            result = CenturyPrefixTypeSafeEnum.QUARTER2;
                        else if (Regex.IsMatch(input, @"^(?:3\s?°|terzo) quarto del", options))
                            result = CenturyPrefixTypeSafeEnum.QUARTER3;
                        else if (Regex.IsMatch(input, @"^(?:4\s?°|quarto|ultimo) quarto del", options))
                            result = CenturyPrefixTypeSafeEnum.QUARTER4;
                        break;
                    }
                default:
                    {
                        if (Regex.IsMatch(input, @"^Early", options))
                            result = CenturyPrefixTypeSafeEnum.EARLY;
                        if (Regex.IsMatch(input, @"^Mid", options))
                            result = CenturyPrefixTypeSafeEnum.MID;
                        if (Regex.IsMatch(input, @"^Late", options))
                            result = CenturyPrefixTypeSafeEnum.LATE;
                        else if (Regex.IsMatch(input, @"^(?:1st|first) half", options))
                            result = CenturyPrefixTypeSafeEnum.HALF1;
                        else if (Regex.IsMatch(input, @"^(?:2nd|second) half", options))
                            result = CenturyPrefixTypeSafeEnum.HALF2;
                        else if (Regex.IsMatch(input, @"^(?:1st|first) quarter", options))
                            result = CenturyPrefixTypeSafeEnum.QUARTER1;
                        else if (Regex.IsMatch(input, @"^(?:2nd|second) quarter", options))
                            result = CenturyPrefixTypeSafeEnum.QUARTER2;
                        else if (Regex.IsMatch(input, @"^(?:3rd|third) quarter", options))
                            result = CenturyPrefixTypeSafeEnum.QUARTER3;
                        else if (Regex.IsMatch(input, @"^(?:4th|fourth|last) quarter", options))
                            result = CenturyPrefixTypeSafeEnum.QUARTER4;
                        break;
                    }
            }
            return result;
        }
        public static bool Parse(string value, out CenturyPrefixTypeSafeEnum output)
        {

        }
                       
        public string Pattern
        {
            get
            {
                return pattern;
            }
        }
        public LanguageEnum Language
        {
            get
            {
                return language;
            }
        }
                
        #region Statics
        public static readonly CenturyPrefixTypeSafeEnum EARLY = new CenturyPrefixTypeSafeEnum(@"EARLY", LanguageEnum.EN);
        public static readonly CenturyPrefixTypeSafeEnum MID = new CenturyPrefixTypeSafeEnum(@"MID", LanguageEnum.EN);
        public static readonly CenturyPrefixTypeSafeEnum LATE = new CenturyPrefixTypeSafeEnum(@"LATE", LanguageEnum.EN);
        #endregion Statics

        #region Constructors
        private CenturyPrefixTypeSafeEnum(string pattern, LanguageEnum language)
        {
            this.pattern = pattern;
            this.language = language;
        }
        static CenturyPrefixTypeSafeEnum()
        {
        }
        #endregion Constructors
    }
}
