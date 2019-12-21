﻿using AdventOfCode.Utility;
using AdventOfCode.Year2019;
using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            var result = Solver2019_21.SolveSecond(new FileInputResolver(2019, 21));
            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine(sw.ElapsedMilliseconds + " ms");
        }
    }
}
