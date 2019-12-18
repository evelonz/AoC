using AdventOfCode.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_17
    {
        private enum Map15Type
        {
            None = -1,
            Wall = 0,
            Path = 1,
            O2 = 2,
        }

        public static string SolveFirst(IInputResolver input, bool print = false)
        {
            var res = GetMap(input);
            return res.ToString();
        }

        //public static string SolveSecond(IInputResolver input, bool print = false)
        //{
        //    var (map, _, end) = GetMap(input, print);
        //    var steps = Bfs(map, end);
        //    return steps.ToString();
        //}

        private static string GetMap(IInputResolver input)
        {
            var instructions = new long[10_000];
            var inData = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            inData.CopyTo(instructions, 0);

            var sut = new IntcodeComputer(instructions);

            long? pInput = null;
            long response = 0; // 0 = wall, 1 moved, 2 found oxygen system.
            ProgramState state = ProgramState.None;

            var sb = new System.Text.StringBuilder(1000);

            int y = 0;
            int x = 0;

            var map = new int[61][];
            var row = new int[100];
            while (state != ProgramState.Halted)
            {
                state = sut.RunProgram(pInput);

                // Save the display values based on output.
                if (state == ProgramState.ProvidedOutput)
                {
                    response = sut.LastOutput;

                    if (response == 35)
                    {
                        sb.Append("#");
                        row[x++] = (int)response;
                    }
                    else if (response == 46)
                    {
                        sb.Append(".");
                        row[x++] = (int)response;
                    }
                    else if (response == 10)
                    {
                        if(y < map.Length)
                            map[y++] = row;
                        row = new int[100];
                        sb.Append(Environment.NewLine);
                        x = 0;
                    }

                }


            }

            int sum = 0;
            for (int yy = 1; yy < map.Length - 1; yy++)
            {
                var roww = map[yy];
                for (int xx = 1; xx < roww.Length - 1; xx++)
                {
                    var v = roww[xx];
                    var left = roww[xx - 1];
                    var right = roww[xx + 1];
                    var up = map[yy - 1][xx];
                    var down = map[yy + 1][xx];

                    if(v == 35 && left == 35 && right == 35 && up == 35 && down == 35)
                    {
                        var val = xx * yy;
                        sum += val;
                        Console.WriteLine($"({xx},{yy}) : {val} . {sum}");
                    }
                }
            }

            Console.WriteLine(sb.ToString());

            return sum.ToString();
        }

    }
}
