using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timespans
{
    public class Ordinal: IOrdinal
    {
        
        public EnumLanguage language 
        { 
            get 
            {
                // match string to enum constant, non match returns EnumLanguage.NONE
                EnumLanguage result = EnumLanguage.NONE;
                if (!Enum.TryParse<EnumLanguage>(lang.Trim(), true, out result))
                    result = EnumLanguage.NONE;
                return result;
            } 
        }

        public string lang { set; private get;  }
        public string label { get; set; }
        public int value { get; set; }
    }
}
