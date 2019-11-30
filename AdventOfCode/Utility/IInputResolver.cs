using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Utility
{
    interface IInputResolver
    {
        StreamReader AsStream();

        IEnumerable<string> AsEnumerable();
    }
}
