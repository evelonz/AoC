using AdventOfCode.Utility;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    class computer
    {
        private readonly int phase;
        private readonly int[] instructions;
        public computer(int phaseIn, int[] instructionsIn)
        {
            phase = phaseIn;
            instructions = instructionsIn;
        }

        public int Compute(int input)
        {
            return 0;
        }

    }
    static class Solver2019_7_2
    {

        public static int IntCodeCompute(
            int[] instructions,
            int input,
            ref int pointer,
            out bool halted)
        {
            var opcodeAndParams = instructions[pointer];
            var opcode = opcodeAndParams % 100;
            var programInput = input;
            bool needNewInput = false;

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
                        if(needNewInput)
                        {
                            halted = false;
                            return -1;
                        }
                        val = programInput;
                        outp = instructions[pointer + 1];
                        instructions[outp] = val;
                        needNewInput = true;

                        opcodeParamCount = 2;
                        break;
                    case 4:
                        inp1 = instructions[pointer + 1];
                        inv1 = instructions[inp1];

                        opcodeParamCount = 2;
                        pointer += opcodeParamCount;
                        halted = false;
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

            halted = true;
            return 0;
        }

        public static string Solve(IInputResolver input)
        {
            var instructions = input.AsEnumerable().First().Split(',').Select(s => int.Parse(s)).ToArray();

            var phases = new[] { 9, 8, 7, 6, 5 };
            var permutations = GetPermutations(phases, phases.Length);

            int max = 0;
            foreach (var phase in permutations)
            {
                var instructionSets = new List<(int[], int)>(5);
                for (int i = 0; i < 5; i++)
                {
                    var inst = new int[instructions.Length];
                    instructions.CopyTo(inst, 0);
                    instructionSets.Add((inst, 0));
                }

                // Set start phase.
                int machine = 0;
                foreach (var item in phase)
                {
                    var (instruction, pointer) = instructionSets[machine];
                    var output = IntCodeCompute(instruction, item, ref pointer, out var halted);
                    if(output != -1)
                    {
                        throw new System.Exception("Unexpected output from phase set");
                    }
                    instructionSets[machine++] = (instruction, pointer);
                }

                // Feedback loop
                int loopedInput = 0;
                int thrusterSignal = 0;
                bool allHalted = false;
                while (!allHalted)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        allHalted = false;
                        var (instruction, pointer) = instructionSets[i];
                        loopedInput = IntCodeCompute(instruction, loopedInput, ref pointer, out var halted);
                        if (halted)
                        {
                            // Assume all machines halt at the same time, so we do not track this.
                            allHalted = true;
                            continue;
                        }
                        thrusterSignal = loopedInput;
                        instructionSets[i] = (instruction, pointer);
                    }
                }

                max = (thrusterSignal > max) ? thrusterSignal : max;
            }

            return max.ToString();
        }

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }
    }

}
