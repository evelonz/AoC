using AdventOfCode.Utility;
using System.Linq;

namespace AdventOfCode.Year2019
{
    class Solver2019_2_2
    {
        public string Solve(IInputResolver input)
        {
            var noun = 0;
            var verb = 0;
            var result = 0;
            const int target = 19690720;
            const int maxTestValue = 99;

            var steps2 = input.AsEnumerable().First().Split(',').Select(s => int.Parse(s)).ToArray();

            while (true)
            {
                var instructions = new int[steps2.Length];
                steps2.CopyTo(instructions, 0);
                int pointer = 0;
                var opcode = instructions[0];
                // Set the noun and verb to test.
                instructions[1] = noun;
                instructions[2] = verb;

                while (opcode != 99)
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

                result = instructions[0];
                if(result == target)
                {
                    break;
                }

                // Increatment test case.
                noun++;
                if(noun > maxTestValue)
                {
                    verb++;
                    noun = 0;
                }
                if(verb > maxTestValue)
                {
                    throw new System.Exception($"Verb is over {maxTestValue}");
                }
            }
            
            return (100 * noun + verb).ToString();
        }
    }
}
