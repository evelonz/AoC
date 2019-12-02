using AdventOfCode.Utility;
using System.Linq;

namespace AdventOfCode.Year2019
{
    class Solver2019_2_1
    {
        public string Solve(IInputResolver input)
        {
            var instructions = input.AsEnumerable().First().Split(',').Select(s => int.Parse(s)).ToArray();
            int pointer = 0;
            var opcode = instructions[0];
            // Manipulate the input according to the task.
            instructions[1] = 12;
            instructions[2] = 2;

            while(opcode != 99)
            {
                var inp1 = instructions[pointer + 1];
                var inp2 = instructions[pointer + 2];
                var outp = instructions[pointer + 3];
                var inv1 = instructions[inp1];
                var inv2 = instructions[inp2];
                int val;
                switch (opcode)
                {
                    case 1:
                        val = inv1 + inv2;
                        break;
                    case 2:
                        val = inv1 * inv2;
                        break;
                    default: throw new System.InvalidOperationException("unknown opcode");
                }
                instructions[outp] = val;
                pointer += 4;
                opcode = instructions[pointer];
            }
            return instructions[0].ToString();
        }
    }
}
