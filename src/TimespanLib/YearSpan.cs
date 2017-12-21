using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimespanLib.Rx;

namespace TimespanLib
{
    public class YearSpan : IYearSpan
    {
        // member property getters and setters
        public int min { get; set; }
        public int max { get; set; }
        public string label { get; set; }
        public string note { get; set; } // used for displaying parser name later
        public EnumLanguage language { get; set; }

        #region constructors
        public YearSpan()
        {
            this.min = 0;
            this.max = 0;
            this.label = "";
            this.note = "";
            this.language = EnumLanguage.NONE;
        }
        public YearSpan(int min, int max, string label = "", string note = "", string lang = "en")
            : this()
        {
            // ensure values go to correct fields 
            this.min = Math.Min(min, max);
            this.max = Math.Max(min, max);
            this.label = label.Trim();
            this.note = note.Trim();
            EnumLanguage language = EnumLanguage.NONE;
            Enum.TryParse<EnumLanguage>(lang.Trim(), true, out language);
            this.language = language;
        }

        public YearSpan(int value, string label = "", string note = "", string lang = "en") : this(value, value, label, note, lang) { }
        #endregion

        #region IEquatable interface
        public bool Equals(IInterval<int> span)
        {
            return (span == null ? false : (span.min == this.min && span.max == this.max));
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj as IInterval<int>);
        }
        public override int GetHashCode()
        {
            return this.min.GetHashCode() ^ this.max.GetHashCode(); // using XOR.. is this correct?? 
        }
        #endregion

        // todo: ToJSON (?)
        public override string ToString()
        {
            return String.Format("{0}", this.label);
        }        

        // member functions
        public int size()
        {
            // +1 for years e.g. Size of { 1521, 1522 } is 2
            return Math.Abs(this.max - this.min) + 1;
            // there is no year zero - goes from 1 BC to 1 AD - so size of 1 BC -> 1 AD is 2 years? 
        }


        public static IYearSpan Parse(string input, string language)
        {
            EnumLanguage lang = EnumLanguage.EN;
            if (!Enum.TryParse(language.Trim().ToUpper(), true, out lang))
                lang = EnumLanguage.EN;
            return Parse(input, lang);
        }
                
        // NEW: 09/05/2017 bringing parser operations into the YearSpan class itself
        // TODO: throw exceptions:
        // ArgumentNullException -> s is null
        // ?FormatException -> s is not in the correct format (how can we know that?)
        // ?OverflowException -> a year represents a number less than Int.MinValue or greater than Int.MaxValue

        /// <summary>Parse a <c>YearSpan</c> from a <c>String</c> input value</summary>
        /// <param name="s">Input <c>String</c> to be parsed</param>
        /// <returns><c>YearSpan</c></returns>
        /// <exception cref="System.ArgumentNullException">Thrown when s is null</exception>
        public static IYearSpan Parse(string s, EnumLanguage language)
        {
            if (s == null) throw new System.ArgumentNullException("s", "null input value");

            // IEnumerable<IYearSpanParser> yearSpanParsers = getParserInstances();
            IYearSpan span;

            // try each matcher until first match (done this way rather than reflection so we control the order of matching)
            if (Rx.FromYearToYear.IsMatch(s, language)) span = Rx.FromYearToYear.Match(s, language);
            else if (Rx.FromCenturyToCentury.IsMatch(s, language)) span = Rx.FromCenturyToCentury.Match(s, language);
            else if (Rx.FromCenturyToCenturyOrdinal.IsMatch(s, language)) span = Rx.FromCenturyToCenturyOrdinal.Match(s, language);
            else if (Rx.Decade.IsMatch(s, language)) span = Rx.Decade.Match(s, language);
            else if (Rx.MonthYear.IsMatch(s, language)) span = Rx.MonthYear.Match(s, language);
            else if (Rx.CardinalCentury.IsMatch(s, language)) span = Rx.CardinalCentury.Match(s, language);
            else if (Rx.OrdinalCentury.IsMatch(s, language)) span = Rx.OrdinalCentury.Match(s, language);
            else if (Rx.OrdinalMillennium.IsMatch(s, language)) span = Rx.OrdinalMillennium.Match(s, language);
            else if (Rx.CardinalMillennium.IsMatch(s, language)) span = Rx.CardinalMillennium.Match(s, language);
            else if (Rx.SeasonYear.IsMatch(s, language)) span = Rx.SeasonYear.Match(s, language);
            else if (Rx.SingleYear.IsMatch(s, language)) span = Rx.SingleYear.Match(s, language);
            else if (Rx.YearWithTolerance.IsMatch(s, language)) span = Rx.YearWithTolerance.Match(s, language);
            else if (Rx.DateTimeMatch.IsMatch(s, language)) span = Rx.DateTimeMatch.Match(s, language);
            //else if (Rx.NamedPeriod.IsMatch(s, language)) span = Rx.NamedPeriod.Match(s, language);
            else if (Rx.Lookup<NamedPeriod>.IsMatch(s, language))
            {
                span = (IYearSpan)Rx.Lookup<NamedPeriod>.Match(s, language);
                span.label = s;
                span.language = language;
                span.note = "RxLookup"; // these properties are not populated by (generic) Rx.Lookup
            }

            //else if (NamedPeriod.IsMatch(s, language)) span = (IYearSpan)NamedPeriod.Match(s, language);
            //else if (Rx.LastChance.IsMatch(s, language)) span = Rx.LastChance.Match(s, language);
            else span = new YearSpan(0, s); // no match? zero year span returned
            
            
            // try each available parser until first match
            /*foreach (IMatcher<IYearSpan> p in parserInstances)
            {
                IYearSpan span = null;
                try
                {
                    span = p.Match(s, language);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
                if (span != null) return span;
            }*/

           /* IList<Type> types = getMatcherTypes();
            foreach (Type t in types)
            {                
                IYearSpan span = null;
                try
                {
                    IMatcher<IYearSpan> m = (IMatcher<IYearSpan>)t;
                    span = m.Match(s, language);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
                if (span != null) return span;
            } */

            /*foreach (IYearSpanParser p in parserInstances)
            {
                IYearSpan span = p.Parse(s, language);
                if (span != null) return span;
            }*/
            
            return span;

            //return new YearSpan(5,5); //temp
        }

        /// <summary>Attempt to Parse a <c cref="YearSpan">YearSpan</c> from a <c cref="System.String">String</c> input value</summary>
        /// <param name="s">Input <c cref="System.String">String</c> to be parsed</param>
        /// <param name="result">Output <c cref="YearSpan">YearSpan</c> if successful, <c>null</c> otherwise</param>
        /// <returns>Returns <c>true</c> if parse was successful, <c>false</c> otherwise</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when s is null</exception>
        public static bool TryParse(string s, EnumLanguage language, out IYearSpan result)
        {
            if (s == null) throw new System.ArgumentNullException("s", "null input value");
            result = Parse(s, language);
            return (result == null ? false : true);
        }

        // overload to allow language code to be passed in as a string
        public static IYearSpan[] Parse(string[] input, string language)
        {
            EnumLanguage lang = EnumLanguage.EN;
            if (!Enum.TryParse(language.Trim().ToUpper(), true, out lang))
                lang = EnumLanguage.EN;
            return Parse(input, lang);
        }

        public static IYearSpan[] Parse(string[] input, EnumLanguage language)
        {
            // get list of parser instances
            //IEnumerable<IYearSpanParser> yearSpanParsers = getParserInstances();            
            //IEnumerable<IMatcher<IYearSpan>> yearSpanParsers = getParserInstances();
            System.Collections.Generic.IList<IYearSpan> results = new List<IYearSpan>();

            foreach (string s in input)
            {
                IYearSpan span = Parse(s, language);
                results.Add(span != null ? span : new YearSpan(0, s));
                  

                // try each available parser until first match
                //foreach (IYearSpanParser p in yearSpanParsers)
                /*foreach (IMatcher<IYearSpan> p in yearSpanParsers)
                {
                    if (p != null)
                    {
                        if (p.IsMatch(s, language))
                        {
                            IYearSpan result = p.Match(s, language);
                            if (result != null)
                            {
                                span = result;
                                break;
                            }
                        }
                    }
                }*/

               /* IList<Type> matchers = getMatcherTypes();
                foreach (IMatcher<IYearSpan> m in matchers)
                {
                    if (m.IsMatch(s, language))
                    {
                        IYearSpan result = m.Match(s, language);
                        if (result != null)
                        {
                            span = result;
                            break;
                        }
                    }
                } 
                results.Add(span);*/
            }
            return results.ToArray();
        }


        /*public override IYearSpan parse(string input)
        {
            // cache a list of parser instances so we only create it once
            if (yearSpanParsers == null) yearSpanParsers = getParserInstances();

            // try each available parser until first match
            foreach (IYearSpanParser p in yearSpanParsers)
            {
                IYearSpan span = p.parse(input);
                if (span != null) return span;
            }
            // nothing matched
            return null;
        }*/

        // create a list containing one instance of each YearSpan matcher
        /*
        private static IList<IYearSpanParser> getParserInstancesOLD()
        {
            IList<Type> parserTypes = getParserTypes();
            IList<IYearSpanParser> lst = new List<IYearSpanParser>();
            foreach (Type t in parserTypes)
            {
                lst.Add((IYearSpanParser)Activator.CreateInstance(t));
            }
            return lst.OrderBy(x => x.ToString()).ToList();
        }*/
        /*
        private static IList<IMatcher<IYearSpan>> getParserInstances()
        {
            IList<Type> parserTypes = getParserTypes();
            IList<IMatcher<IYearSpan>> lst = new List<IMatcher<IYearSpan>>();
            foreach (Type t in parserTypes)
            {
                lst.Add((IMatcher<IYearSpan>)Activator.CreateInstance(t));
            }
            return lst.OrderBy(x => x.ToString()).ToList();
        }*/

        /*private static IList<Type> getParserTypesOLD()
        {
            // Using reflection to get list of all valid parserTypes
            System.Collections.Generic.IList<Type> lst = new System.Collections.Generic.List<Type>();
            Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type t in types)
            {
                if (t.IsClass && t.BaseType == typeof(AbstractYearSpanParser))
                    lst.Add(t);
            }
            return lst;
        }*/

        private static IList<Type> getMatcherTypes()
        {
            // Using reflection to get list of all valid parserTypes
            System.Collections.Generic.IList<Type> lst = new System.Collections.Generic.List<Type>();
            Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type t in types)
            {
                if (t.IsClass && t.BaseType == typeof(Matcher<IYearSpan>))
                    lst.Add(t);
            }
            return lst;
        }

        /*
        // create a list containing one instance of each YearSpanParser subclass
        private static IEnumerable<IYearSpanParser> getParserInstances1()
        {
            //  instances of parsers 
           
            IList<IYearSpanParser> parserInstances = new List<IYearSpanParser>();
            //AppDomain.CurrentDomain.GetAssemblies();
            Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            //Type[] types = System.Reflection.Assembly.GetCallingAssembly().GetTypes();
            foreach (Type t in types)
            {
                if(!t.IsInterface && 
                    !t.IsAbstract && 
                    t.GetConstructor(Type.EmptyTypes) != null && 
                    t.GetInterfaces().Contains(typeof(IYearSpanParser)))
                    parserInstances.Add((IYearSpanParser)Activator.CreateInstance(t));
            }
        */
        /* IEnumerable<IYearSpanParser> parserInstances = from t in System.Reflection.Assembly.GetCallingAssembly().GetTypes()
                                         where !t.IsInterface 
                                         && !t.IsAbstract
                                         && t.IsClass 
                                         && t.BaseType == typeof(AbstractYearSpanParser) 
                                         && t.IsNotPublic
                                         && t.GetConstructor(Type.EmptyTypes) != null
                                         && t.GetInterfaces().Contains(typeof(IYearSpanParser))
                                         orderby t.Name // order allows controlling order parsers are tried, by prefixing their names 
                                         select Activator.CreateInstance(t) as IYearSpanParser;*/
        //return parserInstances;            

        //foreach (Type t in types)
        //{
        // if it's a subclass of AbstractYearSpanParser, create an instance to add to the list
        // if (t.IsClass && t.BaseType == typeof(AbstractYearSpanParser) && t.IsNotPublic)
        //parserInstances.Add((IYearSpanParser)Activator.CreateInstance(t));




        /*var type = typeof(IYearSpanParser);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes()).Where(p => type.IsAssignableFrom(p) && !p.IsAbstract && !p.IsInterface);

        //IEnumerable<IYearSpanParser> parserInstances = types.SelectMany(t =>  (IYearSpanParser)Activator.CreateInstance(t));

        IEnumerable<IYearSpanParser> parserInstances = from t in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                                                       where t.GetInterfaces().Contains(IYearSpanParser) &&
                                                                t.IsClass  && t.GetConstructor(Type.EmptyTypes) != null
                                                       select (IYearSpanParser)Activator.CreateInstance(t);
        return parserInstances;*/
    }
}