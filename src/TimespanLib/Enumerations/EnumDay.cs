using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimespanLib
{
    public enum EnumDay
    {
        NONE = 0, MON, TUE, WED, THU, FRI, SAT, SUN
    }

    
    // static class + const approach:
    public static class Days
    {      
        // http://vocab.getty.edu/aat/300410303 = days of week
        public const string Monday = "http://vocab.getty.edu/aat/300410304";
        public const string Tuesday = "http://vocab.getty.edu/aat/300410305";
        public const string Wednesday = "http://vocab.getty.edu/aat/300410306";
        public const string Thursday = "http://vocab.getty.edu/aat/300410307";
        public const string Friday = "http://vocab.getty.edu/aat/300410308";
        public const string Saturday = "http://vocab.getty.edu/aat/300410309";
        public const string Sunday = "http://vocab.getty.edu/aat/300410310";       
        // but how to get const from uri value?
        

    }

    public class tmp
    {
        public static void Write()
        {
            Console.Write(Days.Sunday);
        }
    }
}