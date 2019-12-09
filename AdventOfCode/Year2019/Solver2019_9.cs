using AdventOfCode.Utility;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_9
    {
        public static long IntCodeCompute(
            long[] instructions,
            long input,
            ref int pointer,
            out bool halted,
            ref int relativeBase)
        {
            var opcodeAndParams = instructions[pointer];
            var opcode = opcodeAndParams % 100;
            var programInput = input;
            bool needNewInput = false;

            while (opcode != 99)
            {
                long inp1;
                long inp2;
                long outp;
                long inv1;
                long inv2;
                int opcodeParamCount = 0;
                long val;
                // Get param types.
                var param1Type = (int)opcodeAndParams / 100 % 10;
                var param2Type = (int)opcodeAndParams / 1000 % 10;
                var param3Type = (int)opcodeAndParams / 10000 % 10;

                switch (opcode)
                {
                    case 1:
                        inp1 = instructions[pointer + 1];
                        inp2 = instructions[pointer + 2];
                        inv1 = GetInput(param1Type, inp1, instructions, relativeBase); //param1Type == 0 ? instructions[inp1] : inp1;
                        inv2 = GetInput(param2Type, inp2, instructions, relativeBase); //param2Type == 0 ? instructions[inp2] : inp2;

                        val = inv1 + inv2;
                        outp = GetOutP(param3Type, pointer + 3, instructions, relativeBase);
                        instructions[outp] = val;
                        opcodeParamCount = 4;
                        break;
                    case 2:
                        inp1 = instructions[pointer + 1];
                        inp2 = instructions[pointer + 2];
                        inv1 = GetInput(param1Type, inp1, instructions, relativeBase); //param1Type == 0 ? instructions[inp1] : inp1;
                        inv2 = GetInput(param2Type, inp2, instructions, relativeBase); //param2Type == 0 ? instructions[inp2] : inp2;

                        val = inv1 * inv2;
                        outp = GetOutP(param3Type, pointer + 3, instructions, relativeBase);
                        instructions[outp] = val;
                        opcodeParamCount = 4;
                        break;
                    case 3:
                        if (needNewInput)
                        {
                            halted = false;
                            return -1;
                        }
                        val = programInput;
                        outp = GetOutP(param1Type, pointer + 1, instructions, relativeBase);
                        instructions[outp] = val;
                        needNewInput = true;
                        
                        opcodeParamCount = 2;
                        break;
                    case 4:
                        inp1 = instructions[pointer + 1];
                        inv1 = GetInput(param1Type, inp1, instructions, relativeBase); // instructions[inp1];

                        opcodeParamCount = 2;
                        pointer += opcodeParamCount;
                        halted = false;
                        return inv1;
                    case 5:
                        inp1 = instructions[pointer + 1];
                        inp2 = instructions[pointer + 2];
                        inv1 = GetInput(param1Type, inp1, instructions, relativeBase); //param1Type == 0 ? instructions[inp1] : inp1;
                        inv2 = GetInput(param2Type, inp2, instructions, relativeBase); //param2Type == 0 ? instructions[inp2] : inp2;
                        if (inv1 != 0)
                            pointer = (int)inv2;
                        else
                            opcodeParamCount = 3;
                        break;
                    case 6:
                        inp1 = instructions[pointer + 1];
                        inp2 = instructions[pointer + 2];
                        inv1 = GetInput(param1Type, inp1, instructions, relativeBase); //param1Type == 0 ? instructions[inp1] : inp1;
                        inv2 = GetInput(param2Type, inp2, instructions, relativeBase); //param2Type == 0 ? instructions[inp2] : inp2;
                        if (inv1 == 0)
                            pointer = (int)inv2;
                        else
                            opcodeParamCount = 3;
                        break;
                    case 7:
                        inp1 = instructions[pointer + 1];
                        inp2 = instructions[pointer + 2];
                        inv1 = GetInput(param1Type, inp1, instructions, relativeBase); //param1Type == 0 ? instructions[inp1] : inp1;
                        inv2 = GetInput(param2Type, inp2, instructions, relativeBase); //param2Type == 0 ? instructions[inp2] : inp2;

                        outp = GetOutP(param3Type, pointer + 3, instructions, relativeBase);
                        instructions[outp] = (inv1 < inv2) ? 1 : 0;

                        opcodeParamCount = 4;
                        break;
                    case 8:
                        inp1 = instructions[pointer + 1];
                        inp2 = instructions[pointer + 2];
                        inv1 = GetInput(param1Type, inp1, instructions, relativeBase);// == 0 ? instructions[inp1] : inp1;
                        inv2 = GetInput(param2Type, inp2, instructions, relativeBase); //param2Type == 0 ? instructions[inp2] : inp2;

                        outp = GetOutP(param3Type, pointer + 3, instructions, relativeBase);
                        instructions[outp] = (inv1 == inv2) ? 1 : 0;

                        opcodeParamCount = 4;
                        break;
                    case 9:
                        inp1 = instructions[pointer + 1];
                        inv1 = GetInput(param1Type, inp1, instructions, relativeBase);

                        relativeBase += (int)inv1;
                        opcodeParamCount = 2;
                        break;
                    default: throw new System.InvalidOperationException("unknown opcode");
                }

                pointer += opcodeParamCount;
                opcodeAndParams = instructions[pointer];
                opcode = opcodeAndParams % 100;
            }

            halted = true;
            return 0;
        }

        private static long GetInput(int type, long paramValue, long[] instructions, long relativeBase)
        {
            return type == 0 ? instructions[paramValue]
                            : type == 1 ? paramValue
                            : instructions[paramValue+relativeBase];
        }

        private static long GetOutP(int type, int pointer, long[] instructions, int relativeBase)
        {
            if (type == 0)
                return instructions[pointer];
            else if (type == 2)
            {
                var t = instructions[pointer];
                return t + relativeBase;
            }
            else
            {
                throw new System.Exception("Unexpected out type");
            }
        }

        public static string SolveFirst(IInputResolver input)
            => Solve(input, 1);

        public static string SolveSecond(IInputResolver input)
            => Solve(input, 2);

        private static string Solve(IInputResolver input, int startValue)
        {
            var instructions = new long[10_000];
            var inData = input.AsEnumerable().First().Split(',').Select(s => long.Parse(s)).ToArray();
            inData.CopyTo(instructions, 0);

            int pointer = 0;
            bool halted = false;
            int relateiveBase = 0;
            var result = new List<string>(100);
            while(!halted)
            {
                var output = IntCodeCompute(instructions, startValue, ref pointer, out halted, ref relateiveBase);
                if (!halted)
                    result.Add(output.ToString());
                System.Console.WriteLine(output);
            }

            return string.Join(',', result);
        }
    }

}
