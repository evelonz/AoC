using AdventOfCode.Utility;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_7_1
    {

        public static int IntCodeCompute(
            int[] instructions,
            int phaseSetting,
            int secondInput)
        {
            int pointer = 0;
            var opcodeAndParams = instructions[pointer];
            var opcode = opcodeAndParams % 100;

            var programInput = new[] { phaseSetting, secondInput };
            int inputPointer = 0;

            while (opcode != 99)
            {
                int inp1;
                int inp2;
                int outp;
                int inv1;
                int inv2;
                int opcodeParamCount = 0;
                int val;
                // Get param types.
                var param1Type = opcodeAndParams / 100 % 10;
                var param2Type = opcodeAndParams / 1000 % 10;
                var param3Type = opcodeAndParams / 10000 % 10;

                switch (opcode)
                {
                    case 1:
                        inp1 = instructions[pointer + 1];
                        inp2 = instructions[pointer + 2];
                        inv1 = param1Type == 0 ? instructions[inp1] : inp1;
                        inv2 = param2Type == 0 ? instructions[inp2] : inp2;

                        val = inv1 + inv2;
                        if (param3Type == 1) throw new System.Exception("Unexpected out type 1");

                        outp = instructions[pointer + 3];
                        instructions[outp] = val;
                        opcodeParamCount = 4;
                        break;
                    case 2:
                        inp1 = instructions[pointer + 1];
                        inp2 = instructions[pointer + 2];
                        inv1 = param1Type == 0 ? instructions[inp1] : inp1;
                        inv2 = param2Type == 0 ? instructions[inp2] : inp2;

                        val = inv1 * inv2;
                        if (param3Type == 1) throw new System.Exception("Unexpected out type 1");

                        outp = instructions[pointer + 3];
                        instructions[outp] = val;
                        opcodeParamCount = 4;
                        break;
                    case 3:
                        val = programInput[inputPointer++];
                        outp = instructions[pointer + 1];
                        instructions[outp] = val;

                        opcodeParamCount = 2;
                        break;
                    case 4:
                        inp1 = instructions[pointer + 1];
                        inv1 = instructions[inp1];

                        return inv1;
                    case 5:
                        inp1 = instructions[pointer + 1];
                        inp2 = instructions[pointer + 2];
                        inv1 = param1Type == 0 ? instructions[inp1] : inp1;
                        inv2 = param2Type == 0 ? instructions[inp2] : inp2;
                        if (inv1 != 0)
                            pointer = inv2;
                        else
                            opcodeParamCount = 3;
                        break;
                    case 6:
                        inp1 = instructions[pointer + 1];
                        inp2 = instructions[pointer + 2];
                        inv1 = param1Type == 0 ? instructions[inp1] : inp1;
                        inv2 = param2Type == 0 ? instructions[inp2] : inp2;
                        if (inv1 == 0)
                            pointer = inv2;
                        else
                            opcodeParamCount = 3;
                        break;
                    case 7:
                        inp1 = instructions[pointer + 1];
                        inp2 = instructions[pointer + 2];
                        inv1 = param1Type == 0 ? instructions[inp1] : inp1;
                        inv2 = param2Type == 0 ? instructions[inp2] : inp2;

                        outp = instructions[pointer + 3];
                        instructions[outp] = (inv1 < inv2) ? 1 : 0;
                        opcodeParamCount = 4;
                        break;
                    case 8:
                        inp1 = instructions[pointer + 1];
                        inp2 = instructions[pointer + 2];
                        inv1 = param1Type == 0 ? instructions[inp1] : inp1;
                        inv2 = param2Type == 0 ? instructions[inp2] : inp2;

                        outp = instructions[pointer + 3];
                        instructions[outp] = (inv1 == inv2) ? 1 : 0;
                        opcodeParamCount = 4;
                        break;
                    default: throw new System.InvalidOperationException("unknown opcode");
                }

                pointer += opcodeParamCount;
                opcodeAndParams = instructions[pointer];
                opcode = opcodeAndParams % 100;
            }

            throw new System.Exception("No output provided");
        }

        public static string Solve(IInputResolver input)
        {
            var instructions = input.AsEnumerable().First().Split(',').Select(s => int.Parse(s)).ToArray();

            var phases = new[] { 4, 3, 2, 1, 0 };
            var permutations = SetHelper.GetPermutations(phases, phases.Length);
            
            int max = 0;
            foreach (var phase in permutations)
            {
                int secondInput = 0;
                foreach (var firstInput in phase)
                {
                    var instruction = new int[instructions.Length];
                    instructions.CopyTo(instruction, 0);
                    secondInput = IntCodeCompute(instruction, firstInput, secondInput);
                }

                max = (secondInput > max) ? secondInput : max;
            }

            return max.ToString();
        }
    }

}
