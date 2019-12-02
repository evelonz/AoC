using AdventOfCode.Utility;
using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new Year2019.Solver2019_2_2();
            var b = a.Solve(new FileInputResolver(2019, 2, 1));
            Console.WriteLine(b);
        }
    }
}
