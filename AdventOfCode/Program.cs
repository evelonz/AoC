using AdventOfCode.Utility;
using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new Year2019.Solver2019_3_2();
            var b = a.Solve(new FileInputResolver(2019, 3, 1));
            Console.WriteLine(b);
        }
    }
}
