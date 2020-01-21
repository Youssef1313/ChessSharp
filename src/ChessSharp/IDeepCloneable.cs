using System;
using System.Collections.Generic;
using System.Text;

namespace ChessSharp
{
    interface IDeepCloneable<T>
    {
        T DeepClone();
    }
}
