using AdventOfCode.Utility;
using AdventOfCode.Year2019;
using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var b = Solver2019_6_2.Solve(new FileInputResolver(2019, 6));
            Console.WriteLine(b);
        }
    }
}
