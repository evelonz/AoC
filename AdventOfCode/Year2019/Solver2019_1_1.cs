using AdventOfCode.Utility;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_1_1
    {
        public static string Solve(IInputResolver input)
            => input.AsEnumerable().Sum(s => int.Parse(s)/3 - 2).ToString();
    }
}
