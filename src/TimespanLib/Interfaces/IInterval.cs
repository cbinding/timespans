using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimespanLib
{
    public interface IInterval<T> : IEquatable<IInterval<T>>
    {
        T min { get; }
        T max { get; }
        T size();
    }
}
