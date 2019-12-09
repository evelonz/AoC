using AdventOfCode.Utility;
using AdventOfCode.Year2019;
using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var b = Solver2019_9.SolveSecond(new FileInputResolver(2019, 9));
            Console.WriteLine(b);
        }
    }
}
