using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimespanLib.Rx
{
    // Reason for presence of this class is that Rx.Lookup filename is based on datatype 
    // so we need a 'special' NamedPeriod datatype - but in reality it's just a YearSpan
    public class NamedPeriod : YearSpan {} 
}