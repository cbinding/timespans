using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Timespans
{
    public class Label
    {
        public string value { get; set; }
        public string lang { get; set; }
    }

    public class Lookup
    {
        IDictionary<String, Label> _lookupItems;

        // constructor
        public Decade() {
            _labels = new List<Label>();
        }

        // destructor
        ~Decade()
        {
            _labels = null;
        }

        public Uri uri { get; set; }
        public IEnumerable<Label> labels { get { return _labels; } }        
    }
}