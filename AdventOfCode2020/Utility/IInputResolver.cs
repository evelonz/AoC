using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2020.Utility
{
    internal interface IInputResolver
    {
        IEnumerable<string> AsEnumerable();
    }
}
