using AdventOfCode.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_13_2
    {
        public static string Solve(IInputResolver input)
        {
            var instructions = new long[10_000];
            var inData = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            inData.CopyTo(instructions, 0);

            var sut = new IntcodeComputer(instructions);

            long? pInput = null;

            int outType = 0; // 0 = x, 1 = y, 3 = type, then back to 0.
            long x = 0; long y = 0;
            ProgramState state = ProgramState.None;
            var map = new Dictionary<(long x, long y), long>(600);
            var score = 0L;
            var ballPos = (x: 0L, y: 0L);
            var paddlePos = (x: 0L, y: 0L);
            while (state != ProgramState.Halted)
            {
                // Input promt is in start. Might have to check when halt is given as well.
                state = sut.RunProgram(pInput);
                pInput = null;

                // Save the display values based on output.
                if (state == ProgramState.ProvidedOutput)
                {
                    var val = sut.LastOutput;
                    if (outType == 2)
                    {
                        // Special position gives the score.
                        if(x == -1 && y == 0)
                        {
                            score = val;
                        }
                        else
                        {
                            if(map.ContainsKey((x, y)))
                            {
                                map[(x, y)] = val;
                            }
                            else 
                                map.Add((x, y), val);
                            if(val == 3)
                            {
                                paddlePos = (x, y);
                            }
                            if (val == 4)
                                ballPos = (x, y);
                        }
                    }
                    if (outType == 0)
                        x = val;
                    if (outType == 1)
                        y = val;

                    outType++;
                    outType %= 3;
                }

                // Inverted the opcode in the incode computer to ask for input,
                // instead of first using the provided one and then promt for more.
                // Else we get one frames input lag to the game.
                if (state == ProgramState.NeedInput)
                {
                    // Draw the map.
                    //Console.WriteLine("");
                    //Console.WriteLine($"--------- {score} -----------");
                    //var xmax = map.Max(m => m.Key.x);
                    //var xmin = map.Min(m => m.Key.x);
                    //var ymax = map.Max(m => m.Key.y);
                    //var ymin = map.Min(m => m.Key.y);
                    //for (int yy = 0; yy < ymax; yy++)
                    //{
                    //    for (int xx = 0; xx < xmax; xx++)
                    //    {
                    //        if(map.TryGetValue((xx, yy), out var vall))
                    //        {
                    //            var t = (vall == 0) ? " "
                    //                : vall == 1 ? "#"
                    //                : vall == 2 ? "¤"
                    //                : vall == 3 ? "_"
                    //                : vall == 4 ? "."
                    //                : "?";
                    //            Console.Write(t);
                    //        }
                    //        else
                    //        {
                    //            Console.Write(" ");
                    //        }
                    //    }
                    //    Console.WriteLine("");
                    //}

                    // Provide input.
                    // Automatically
                    long joystickMove = 0L;
                    if (ballPos.x > paddlePos.x)
                        joystickMove = 1;
                    if (ballPos.x < paddlePos.x)
                        joystickMove = -1;
                    pInput = joystickMove;

                    // Manually play the game
                    //var userInput = Console.ReadLine();
                    //pInput = (long)Convert.ToInt32(userInput);
                }

            }

            return score.ToString();
        }
    }
}
