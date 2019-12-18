using AdventOfCode.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_17_2
    {
        public static string SolveFirst(IInputResolver input, bool print = false)
        {
            var r = SolveMutations();
            //var res = GetMap(input);
            //return res.ToString();
            return "";
        }

        private static string SolveMutations()
        {
            //var map = new string[] {
            //    "L", "10", "R", "10", "L", "10", "L", "10", "R", "10", "R", "12", "L", "12", "L", "10", "R", "10", "R", "12", "L", "12", "L", "10", "R", "12", "L", "12", "R", "12", "L", "12", "R", "6", "R", "12", "L", "12", "R", "6", "R", "10", "R", "12", "L", "12", "L", "10", "R", "10", "L", "10", "L", "10", "R", "10", "R", "12", "L", "12", "R", "12", "L", "12", "R", "6"
            //};

            var map = new string[] {
                "R", "8", "R", "8", "R", "4", "R", "4", "R", "8", "L", "6", "L", "2", "R", "4", "R", "4", "R", "8", "R", "8", "R", "8", "L", "6", "L", "2"
            };

            var goal = string.Join(",", map);

            string A = "";
            string B = "";
            string C = "";

            for (int a = 3; a < 10; a++)
            {
                var aa = map.Take(a + 1);
                A = string.Join(",", aa);

                // Test repeating A, before moving on B and C.
                // But we must then also backtrack from B, and try A again.
                // perhaps just try all permutations?

                for (int b = 2; b < 10; b++)
                {
                    var bb = map.Skip(a + 1).Take(b + 1);
                    B = string.Join(",", bb);

                    for (int c = 3; c < 10; c++)
                    {
                        var cc = map.Skip(a + 1 + b + 1).Take(c + 1);
                        C = string.Join(",", cc);
                    }
                }
            }


            return "";
        }

        private static string GetMap(IInputResolver input)
        {
            var instructions = new long[10_000];
            var inData = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            inData.CopyTo(instructions, 0);
            instructions[0] = 1;

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

                    if (response == 10)
                    {
                        if (y < map.Length)
                            map[y++] = row;
                        row = new int[100];
                        sb.Append(Environment.NewLine);
                        x = 0;
                    }
                    else
                    {
                        sb.Append((char)(int)response);
                        row[x++] = (int)response;
                    }
                }


            }

            for (int yy = 0; yy < map.Length; yy++)
            {
                var roww = map[yy];
                for (int xx = 0; xx < roww.Length; xx++)
                {
                    Console.Write((char)roww[xx]);
                }
                Console.WriteLine("");
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
