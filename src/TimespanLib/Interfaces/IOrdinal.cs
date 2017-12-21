using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timespans
{
    public interface IOrdinal
    {
        EnumLanguage language { get; }
        string lang { set; }
        string label { get; }
        int value { get; }
    }  
}
