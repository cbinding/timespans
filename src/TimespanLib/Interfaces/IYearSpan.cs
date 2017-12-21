using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimespanLib
{
    public interface IYearSpan : IInterval<int> 
    {
        string label { get; set; }
        string note { get; set; }
        EnumLanguage language { get; set; }
    }    
}
