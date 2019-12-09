using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Year2019
{
    class IntcodeComputer
    {
        private int pointer = 0;
        private readonly long[] instructions;
        public bool Halted { get; private set; }
        public bool NeedNewInput { get; private set; }
        public long LastOutput { get; private set; }
        public long LastSetValue { get; private set; }

        public IntcodeComputer(long[] instructions)
        {
            this.instructions = instructions;
        }

        public long Compute(long input)
        {
            NeedNewInput = false;

            var opcodeAndParams = instructions[pointer];
            var opcode = (int)opcodeAndParams % 100;
            var programInput = input;
            long inp1 = 0;
            long inp2 = 0;
            long outp = 0;
            long inv1 = 0;
            long inv2 = 0;
            int relativeBase = 0;
            int opcodeParamCount = 0;
            var temp = 0L;
            int param1Type = 0, param2Type = 0, param3Type = 0;

            if(opcode == 99)
            {
                Halted = true;
                return -1;
            }

            if ("_xxxx"[opcode] == 'x') param1Type = (int)opcodeAndParams / 100 % 10;
            if ("_xx__"[opcode] == 'x') param2Type = (int)opcodeAndParams / 1000 % 10;
            if ("_xx__"[opcode] == 'x') param3Type = (int)opcodeAndParams / 10000 % 10;
            if ("_xx_x"[opcode] == 'x') inp1 = instructions[pointer + 1];
            if ("_xx__"[opcode] == 'x') inp2 = instructions[pointer + 2];
            if ("_xx__"[opcode] == 'x') outp = instructions[pointer + 3];
            if ("_xx_x"[opcode] == 'x') inv1 = GetInput(param1Type, inp1, instructions, relativeBase);
            if ("_xx__"[opcode] == 'x') inv2 = GetInput(param2Type, inp2, instructions, relativeBase);
            if ("_x___"[opcode] == 'x') temp = inv1 + inv2;
            if ("__x__"[opcode] == 'x') temp = inv1 * inv2;
            if ("____x"[opcode] == 'x') LastOutput = inv1;
            if ("___x_"[opcode] == 'x') temp = programInput;
            if ("_xx__"[opcode] == 'x') outp = GetOutP(param3Type, pointer + 3, instructions, relativeBase);
            if ("___x_"[opcode] == 'x') outp = GetOutP(param1Type, pointer + 1, instructions, relativeBase);
            if ("_xx__"[opcode] == 'x') instructions[outp] = temp;
            if ("_xx__"[opcode] == 'x') opcodeParamCount += 4;
            if ("___xx"[opcode] == 'x') opcodeParamCount += 2;
            if ("___x_"[opcode] == 'x') NeedNewInput = true;

            if ("_xxxx"[opcode] == 'x') pointer += opcodeParamCount;

            LastSetValue = temp;
            return temp;
        }

        private static long GetInput(int type, long paramValue, long[] instructions, long relativeBase)
        {
            return type == 0 ? instructions[paramValue]
                            : type == 1 ? paramValue
                            : instructions[paramValue + relativeBase];
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
                throw new Exception("Unexpected out type");
            }
        }
    }
}
