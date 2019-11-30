using AdventOfCode.Utility;
using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new Year2018.Solver2018_1_1();
            var b = a.Solve(new FileInputResolver(2018, 1, 1));
            Console.WriteLine(b);
            Console.WriteLine("Hello World!");
        }
    }
}
