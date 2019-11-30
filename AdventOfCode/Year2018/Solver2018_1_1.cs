using AdventOfCode.Utility;
using System.Linq;

namespace AdventOfCode.Year2018
{
    class Solver2018_1_1
    {
        public string Solve(IInputResolver input) => 
            input.AsEnumerable().Sum(s => int.Parse(s)).ToString();

    }
}
