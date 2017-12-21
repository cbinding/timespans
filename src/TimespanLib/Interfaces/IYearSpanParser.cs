using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timespans
{    
    public interface IYearSpanParser
    {
        IYearSpan Parse(string s, EnumLanguage language);
        Boolean IsMatch(string s, EnumLanguage language);
    }
}
