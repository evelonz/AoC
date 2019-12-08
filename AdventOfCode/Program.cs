using AdventOfCode.Utility;
using AdventOfCode.Year2019;
using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var b = Solver2019_8_2.Solve(new FileInputResolver(2019, 8));
            Console.WriteLine(b);
        }
    }
}
