using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Utility
{
    interface IInputResolver
    {
        IEnumerable<string> AsEnumerable();
    }
}
