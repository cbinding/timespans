using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

// loading regex patterns from external JSON file (need to escape backslashes though)
// Generically this is a regex lookup to return a controlled value
namespace TimespanLib.Rx
{    
    public class LookupItem<T> 
    {
        // the language of the text to match (language-specific patterns)
        [JsonProperty("language")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public EnumLanguage language { get; set; }

        // The regex pattern for matching
        [JsonProperty("pattern")]
        public string pattern { get; set; }

        // the value the match represents
        [JsonProperty("value")]
        //[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public T value { get; set; }
    }
    
    public class Lookup<T> : Matcher<T> 
    {
        private static Lazy<IList<LookupItem<T>>> _items = new Lazy<IList<LookupItem<T>>>(() =>
        {  
            //string thisClassName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name;            
            
            string path = String.Concat("Lookups\\RxLookup-", typeof(T).Name, ".json"); // fileName;

           // if (System.Web.HttpContext.Current != null)
               // path = System.Web.HttpContext.Current.Request.MapPath("~\\" + path);

            string text = System.IO.File.ReadAllText(path); // needs some error checking here...
            return Newtonsoft.Json.JsonConvert.DeserializeObject<IList<LookupItem<T>>>(text);
        });  

        private static IList<LookupItem<T>> items
        {
            get { return _items.Value; }
        }

        public static string[] Patterns(EnumLanguage language = EnumLanguage.NONE)
        {
            return items.Where(i => i.language == language).Select(i => i.pattern).ToArray();
        }

        public static string Pattern(EnumLanguage language = EnumLanguage.NONE, string groupname = "")
        {
            string[] patterns = Patterns(language); 
            return oneof(patterns, groupname); // returns (?<groupname>item1|item2|item3|etc)
        }

        public static bool IsMatch(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            //string[] patterns = Patterns(language); // items.Where(i => i.language == language).Select(i => i.pattern).ToArray();
            
            //return (Regex.IsMatch(input.Trim(), Pattern(language), options));

            IEnumerable<LookupItem<T>> candidates = items.Where(i => i.language == language);
            foreach (LookupItem<T> candidate in candidates)
            {
                if (Regex.IsMatch(input, @"\b" + candidate.pattern + @"\b", options))
                    return true;
            }
            return false;
        }

        public static T Match(string input, EnumLanguage language = EnumLanguage.NONE)
        {
            input = input.Trim();
            IEnumerable<LookupItem<T>> candidates = items.Where(i => i.language == language);
            foreach (LookupItem<T> candidate in candidates)
            {
                if (Regex.IsMatch(input, @"\b" + candidate.pattern + @"\b", options))
                    return candidate.value;
            }
            
            return default(T); // no match = default (enum with value 0) - so will be "NONE" in this app
        }
    }

    //public class LookupEnumDay : Lookup<EnumDay> { } // not really required now...
       
}