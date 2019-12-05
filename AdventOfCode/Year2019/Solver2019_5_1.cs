using AdventOfCode.Utility;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_5_1
    {
        public static string Solve(IInputResolver input)
        {
            var instructions = input.AsEnumerable().First().Split(',').Select(s => int.Parse(s)).ToArray();
            int pointer = 0;
            var opcodeAndParams = instructions[0];
            // paramters now vaiers.
            var opcode = opcodeAndParams % 100;

            var programInput = 1;

            while (opcode != 99)
            {
                int inp1;
                int inp2;
                int outp;
                int inv1;
                int inv2;
                int opcodeParamCount;
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
                        val = programInput;
                        outp = instructions[pointer + 1];
                        instructions[outp] = val;

                        opcodeParamCount = 2;
                        break;
                    case 4:
                        inp1 = instructions[pointer + 1];
                        inv1 = instructions[inp1];
                        System.Console.WriteLine("opc4 out: " + inv1);

                        opcodeParamCount = 2;
                        break;
                    default: throw new System.InvalidOperationException("unknown opcode");
                }

                pointer += opcodeParamCount;
                opcodeAndParams = instructions[pointer];
                opcode = opcodeAndParams % 100;
            }

            // Fix this
            return instructions[0].ToString();
        }
    }
}
