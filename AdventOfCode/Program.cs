using AdventOfCode.Utility;
using AdventOfCode.Year2019;
using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Solver2019_10.SolveSecond(new FileInputResolver(2019, 10), (25, 31));
            Console.WriteLine(result);
        }
    }
}
