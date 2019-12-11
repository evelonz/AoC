using AdventOfCode.Utility;
using AdventOfCode.Year2019;
using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Solver2019_11_2.Solve(new FileInputResolver(2019, 11));
            Console.WriteLine(result);
        }
    }
}
