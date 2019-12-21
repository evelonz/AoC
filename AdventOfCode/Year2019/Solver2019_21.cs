using AdventOfCode.Utility;
using System.Linq;
using System;
namespace AdventOfCode.Year2019
{
    static class Solver2019_21
    {
        public static string SolveFirst(IInputResolver input)
        {
            // Then give it input
            var prog = new[]
            {
                // Hole anywhere, but D = Jump?
                "NOT A J", // Hole at A.
                "NOT B T", // Hole at B.
                "OR T J",  // Hole at A or B.
                "NOT C T", // Hole at C.
                "OR T J",  // Hole at A or B or C. At J.
                "AND D J", // Ground at D and hole at A or B or C.

                "WALK"
            };

            return Solve(input, prog);
        }

        public static string SolveSecond(IInputResolver input)
        {
            // Then give it input
            var prog = new[]
            {
                // Hole anywhere, but D = Jump?
                "NOT A J", // Hole at A.
                "NOT B T", // Hole at B.
                "OR T J",  // Hole at A or B.
                "NOT C T", // Hole at C.
                "OR T J",  // Hole at A or B or C. At J.
                "AND D J", // Ground at D and hole at A or B or C.
                // We can now see 5 more slots.
                // We need to make sure we have at least one option after
                // a jump. So either E or H must have ground.

                // Think we need to set T to false, to be sure about this...
                "NOT D T", // Should only be interesting if D is true. If so set T to false.
                "OR E T", // Land at E
                "OR H T", // Or at H

                "AND T J",  // We want to jump over something.
                            // We can land at D.
                            // There is land at E (walk) or H (jump).

                "RUN" // RUN triggers the second version of the program.
            };

            return Solve(input, prog);
        }

        public static string Solve(IInputResolver input, string[] prog)
        {
            var instructions = new long[10_000];
            var inData = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            inData.CopyTo(instructions, 0);
            var sut = new IntcodeComputer(instructions);

            // First some output
            var state = ProgramState.None;
            while(state != ProgramState.NeedInput)
            {
                state = sut.RunProgram(null);
                PrintAscii(sut);
            }

            
            foreach (var inst in prog)
            {
                SetInstruction(sut, inst);
            }

            while (state != ProgramState.Halted)
            {
                state = sut.RunProgram(null);
                if (sut.LastOutput > 256)
                    return sut.LastOutput.ToString();
                PrintAscii(sut);
            }
            return "";
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
    }
}
