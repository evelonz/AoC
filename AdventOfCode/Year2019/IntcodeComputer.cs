namespace AdventOfCode.Year2019
{
    class IntcodeComputer
    {
        private readonly long[] instructions;
        private readonly long[] _originalIns;

        public int Pointer { get; private set; }
        public bool Halted { get; private set; }
        public bool NeedNewInput { get; private set; }
        public bool ProvidedOutput { get; private set; }
        public long LastOutput { get; private set; }
        public int RelativeBase { get; private set; }
        public long LastSetValue { get; private set; }
        public bool UsedInput { get; set; }
        public IntcodeComputer(long[] instructions)
        {
            this.instructions = instructions;

            _originalIns = new long[instructions.Length];
            instructions.CopyTo(_originalIns, 0);
        }

        public void Reset()
        {
            _originalIns.CopyTo(instructions, 0);
            Pointer = 0;
            Halted = false;
            RelativeBase = 0;
            NeedNewInput = false;
            UsedInput = false;
            LastOutput = 0;
            LastSetValue = 0;
        }

        public ProgramState RunProgram(long? input)
        {
            while(!Halted)
            {
                Compute(input);
                if (NeedNewInput) return ProgramState.NeedInput;
                if (UsedInput) input = null;
                if (ProvidedOutput) return ProgramState.ProvidedOutput;
            }
            return ProgramState.Halted;
        }

        public void Compute(long? input2)
        {
            NeedNewInput = false;
            ProvidedOutput = false;
            UsedInput = false;
            long inp1, inp2, outp, inv1, inv2, temp;
            inp1 = inp2 = outp = inv1 = inv2 = temp = 0L;
            int param1Type = 0, param2Type = 0, param3Type = 0;

            var opcodeAndParams = instructions[Pointer];
            var opcode = (int)opcodeAndParams % 100;

            if (opcode == 99)
            {
                Halted = true;
                return;
            }
            if(opcode == 3 && !input2.HasValue)
            {
                NeedNewInput = true;
                return;
            }

            // op:123456789
            // Get parameter modes: 0 = position mode, 1 = immediate mode, 2 = relative mode.
            if ("_xxxxxxxxx"[opcode] == 'x') param1Type = (int)opcodeAndParams / 100 % 10;
            if ("_xx__xxxx_"[opcode] == 'x') param2Type = (int)opcodeAndParams / 1000 % 10;
            if ("_xx____xx_"[opcode] == 'x') param3Type = (int)opcodeAndParams / 10000 % 10;
            // Get parameter values. The actual value depends on the parameter mode.
            if ("_xx_xxxxxx"[opcode] == 'x') inp1 = instructions[Pointer + 1];
            if ("_xx__xxxx_"[opcode] == 'x') inp2 = instructions[Pointer + 2];
            if ("_xx____xx_"[opcode] == 'x') outp = instructions[Pointer + 3];
            // Get actual parameter values based on parameter mode.
            if ("_xx_xxxxxx"[opcode] == 'x') inv1 = GetInput(param1Type, inp1, instructions, RelativeBase);
            if ("_xx__xxxx_"[opcode] == 'x') inv2 = GetInput(param2Type, inp2, instructions, RelativeBase);
            // opcode specific operations.
            if ("_x________"[opcode] == 'x') temp = inv1 + inv2;
            if ("__x_______"[opcode] == 'x') temp = inv1 * inv2;
            if ("_______x__"[opcode] == 'x') temp = inv1 < inv2 ? 1 : 0;
            if ("________x_"[opcode] == 'x') temp = inv1 == inv2 ? 1 : 0;
            if ("___x______"[opcode] == 'x') { temp = (long)input2; UsedInput = true; }
            if ("____x_____"[opcode] == 'x') { LastOutput = inv1; ProvidedOutput = true; }
            if ("_________x"[opcode] == 'x') RelativeBase += (int)inv1;
            // Get location pointer for output.
            if ("___x______"[opcode] == 'x') outp = GetOutP(param1Type, Pointer + 1, instructions, RelativeBase);
            if ("_xx____xx_"[opcode] == 'x') outp = GetOutP(param3Type, Pointer + 3, instructions, RelativeBase);
            // Store value to memory location.
            if ("_xxx___xx_"[opcode] == 'x') instructions[outp] = temp;
            // Set program pointer.
            if ("_xx____xx_"[opcode] == 'x') Pointer += 4;
            if ("___xx____x"[opcode] == 'x') Pointer += 2;
            if ("_____x____"[opcode] == 'x') Pointer = (inv1 != 0) ? (int)inv2 : Pointer + 3;
            if ("______x___"[opcode] == 'x') Pointer = (inv1 == 0) ? (int)inv2 : Pointer + 3;

            LastSetValue = temp;
            return;
        }

        private static long GetInput(int type, long paramValue, long[] instructions, long relativeBase)
            => type == 0 ? instructions[paramValue]
                            : type == 1 ? paramValue
                            : instructions[paramValue + relativeBase];

        private static long GetOutP(int type, int pointer, long[] instructions, int relativeBase)
         => type == 0 ? instructions[pointer]
            : type == 2 ? instructions[pointer] + relativeBase
            : throw new System.Exception("Parameter mode 1 not supported for output");
    }

    public enum ProgramState
    {
        Halted = -1,
        None = 0,
        NeedInput = 1,
        ProvidedOutput = 2
    }
}
