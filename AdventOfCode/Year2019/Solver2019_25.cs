using AdventOfCode.Utility;
using System.Linq;
using System;
namespace AdventOfCode.Year2019
{
    static class Solver2019_25
    {
        public static string SolveFirst(IInputResolver input)
        {
            return Solve(input);
        }

        public static string Solve(IInputResolver input)
        {
            var instructions = new long[10_000];
            var inData = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            inData.CopyTo(instructions, 0);
            var sut = new IntcodeComputer(instructions);

            var state = ProgramState.None;
            int pointer = 0;
            while (state != ProgramState.Halted)
            {
                state = sut.RunProgram(null);
                if (sut.NeedNewInput && pointer < 34)
                {
                    var cmd = GetIntoPosition[pointer++];
                    Console.WriteLine(cmd);
                    SetInstruction(sut, cmd);
                }
                else if (sut.NeedNewInput)
                {
                    // To play the game manually:
                    //var cmd = ProvideInput();
                    //SetInstruction(sut, cmd);
                    var result = GetPastPressureSensor(sut);
                    return result;
                }
                PrintAscii(sut);
            }
            return "-1";
        }

        private static string GetPastPressureSensor(IntcodeComputer sut)
        {
            // Test each permutation of items.
            // We know when we are to light or heavy, so we could prune 
            // the search space.
            // We have 7 items (8th to heavy by itself, asuming items don't have
            // negative weight).
            // For ease, each iteration will pick all items, then drop all of them.

            for (int items = 1; items < 8; items++)
            {
                var set = SetHelper.GetPermutations(Items, items);
                foreach (var list in set)
                {
                    // Take items
                    foreach (var item in list)
                    {
                        SetInstruction(sut, "take " + item);
                        var rr = GetWholeAscii(sut);
                        //Console.WriteLine(rr);
                    }
                    // Try and move and check result
                    SetInstruction(sut, "west");
                    var res = GetWholeAscii(sut);
                    //Console.WriteLine(res);
                    if (!res.Contains("A loud, robotic voice says \"Alert!"))
                    {
                        return res;
                    }
                    // Else put stuff back.
                    foreach (var item in list)
                    {
                        SetInstruction(sut, "drop " + item);
                        var rr = GetWholeAscii(sut);
                        //Console.WriteLine(rr);
                    }
                }
            }
            return "-2";
            
        }

        private static string ProvideInput()
        {
            var input = Console.ReadLine();
            return input;
        }

        private static void SetInstruction(IntcodeComputer sut, string inst)
        {
            foreach (var c in inst)
            {
                sut.RunProgram((int)c);
                PrintAscii(sut);
            }
            sut.RunProgram(10);
            PrintAscii(sut);
        }

        private static void PrintAscii(IntcodeComputer sut)
        {
            if(sut.ProvidedOutput)
            {
                if (sut.LastOutput == 10)
                    Console.WriteLine("");
                else
                    Console.Write((char)sut.LastOutput);
            }
        }

        private static string GetWholeAscii(IntcodeComputer sut)
        {
            var sb = new System.Text.StringBuilder();
            while (sut.ProvidedOutput)
            {
                if (sut.LastOutput == 10)
                    sb.Append(Environment.NewLine);
                else
                    sb.Append((char)sut.LastOutput);
                sut.RunProgram(null);
            }
            return sb.ToString();
        }

        public static string[] Items => new string[]
        {
            "wreath",
            "mug",
            "astrolabe",
            "manifold",
            "mouse",
            "space law space brochure",
            //"monolith",
            "sand"
        };

        public static string[] GetIntoPosition => new string[]
        {
            "south",
            "take space law space brochure",
            "south",
            "take mouse",
            "south",
            "take astrolabe",
            "south",
            "take mug",
            "north",
            "north",
            "west",
            "north",
            "north",
            "take wreath",
            "south",
            "south",
            "east",
            "north",
            "west",
            "take sand",
            "north",
            "take manifold",
            "south",
            "west",
            "take monolith",
            "west",
            "drop space law space brochure",
            "drop mouse",
            "drop astrolabe",
            "drop mug",
            "drop wreath",
            "drop sand",
            "drop manifold",
            "drop monolith"
        };
    }
}
