import math
from typing import overload
import unittest
import sys
import itertools
from collections import namedtuple
import time
from collections import defaultdict

#input = [x.rstrip() for x in fileinput.input(encoding="utf-16")]
input = [x.rstrip() for x in open(sys.argv[1])] if sys.argv[0].endswith('day24.py') and len(sys.argv) > 1 else []
# print(input)

class ALU:
    z = 0
    def __init__(self, instr, input, z = 0):
        self.w = 0 
        self.x = 0
        self.y = 0
        self.z = z
        self.instr = instr
        self.pointer = 0
        self.input = input
        self.inputPointer = 0

    def __str__(self):
        return f'w: {self.w} x: {self.x} y: {self.y} z: {self.z}'

    def getFirstVariable(self, str):
        match str:
            case 'w':
                return self.w
            case 'x':
                return self.x
            case 'y':
                return self.y
            case 'z':
                return self.z
            case _:
                raise NameError("accessing variable: " + str)

    def getSecondVariable(self, str):
        match str:
            case 'w':
                return self.w
            case 'x':
                return self.x
            case 'y':
                return self.y
            case 'z':
                return self.z
            case _:
                return int(str)

    def setVariable(self, str, value):
        match str:
            case 'w':
                self.w = value
            case 'x':
                self.x = value
            case 'y':
                self.y = value
            case 'z':
                self.z = value
            case _:
                raise NameError("setting variable: " + str)

    def validMONAD(self):
        return self.z == 0

    def run(self):
        while self.pointer < len(self.instr):
            nxt = self.instr[self.pointer]
            self.pointer += 1
            # print(nxt[:3])
            match nxt[:3]:
                case 'inp':
                    self.setVariable(nxt[4:], self.input[self.inputPointer])
                    self.inputPointer += 1
                case 'add':
                    var = self.getFirstVariable(nxt[4:5])
                    new = var + self.getSecondVariable(nxt[6:])
                    self.setVariable(nxt[4:5], new)
                case 'mul':
                    var = self.getFirstVariable(nxt[4:5])
                    new = var * self.getSecondVariable(nxt[6:])
                    self.setVariable(nxt[4:5], new)
                case 'div':
                    var = self.getFirstVariable(nxt[4:5])
                    new = int(var / self.getSecondVariable(nxt[6:]))
                    self.setVariable(nxt[4:5], new)
                case 'mod':
                    var = self.getFirstVariable(nxt[4:5])
                    new = var % self.getSecondVariable(nxt[6:])
                    self.setVariable(nxt[4:5], new)
                case 'eql':
                    var = self.getFirstVariable(nxt[4:5])
                    new = 1 if var == self.getSecondVariable(nxt[6:]) else 0
                    self.setVariable(nxt[4:5], new)
                case _:
                    raise NameError("missing instruction: " + nxt)

# For full input, it seems the same instructions, with different variables repeat 
# for each input.
# We want to run them one by one, possibly backwards, to find input that gives valid z.

subroutines = []
tmp = []
for line in input:
    if line.startswith('inp'):
        if len(tmp) > 0:
            subroutines.append(tmp)
        tmp = []
    tmp.append(line)
subroutines.append(tmp)

possibleZ = []
possibleZ.append([0])
for index in range(4):
    instr = subroutines[0]
    tmp = []
    for i in range(1,10):
        for z in possibleZ[0]:
            alu = ALU(instr, [i], z)
            alu.run()
            #if alu.z == 0:
            # print(alu, "start z:", z)
            tmp.append(alu.z)
    possibleZ.append(tmp)
# print(possibleZ)

subroutines = subroutines[::-1]
# Can we assume that z%26 operation should always decrease the z value?
# If so then we only need to check `(z % 26) - constant == input`
targets = []
targets.append(set([0]))
result = []
for index, inst in enumerate(subroutines):
    print(inst)
    target = targets.pop()
    validInZ = set()
    inputForValidOutZ = defaultdict(list)
    wzToZOut = defaultdict(int)

    print(len(possibleZ), len(subroutines), index, len(subroutines)-index-1)
    if len(possibleZ) > len(subroutines)-index-1:
        print("picked from list")
        zeds = possibleZ[len(subroutines)-index-1]
        if len(subroutines)-index-1 < 2:
            print(zeds)
    else:
        zeds = range(0,1000)

    for i in range(1,10):
        for z in zeds:
            alu = ALU(inst, [i], z)
            alu.run()
            if alu.z in target:
                validInZ.add(z)
                inputForValidOutZ[z].append(i)
                wzToZOut[(i, z)] = alu.z
                # print(alu, "start z:", z)
    targets.append(validInZ)
    result.append((validInZ, inputForValidOutZ, wzToZOut))

# for index, row in enumerate(result):
#     print(index, row)

exit()
startZ = 0
# reverse results
result = result[::-1]
# make loop that find the max number
output = []
for validInZ, inputForValidOutZ, outMap in result:
    # _, inputForValidOutZ, outMap = result[0]
    if startZ not in inputForValidOutZ:
        print(output)
    maxIn = max(inputForValidOutZ[startZ])
    givesOutZ = outMap[(maxIn, startZ)]
    print(maxIn, startZ, givesOutZ)
    startZ = givesOutZ
    output.append(maxIn)
# print(maxIn)
# validZs, inputForValidOutZ = result[-2]
# print(validZs)
# maxIn = max(inputForValidOutZ[maxIn])
# print()
print(output)

# for i in output:
#     alu = ALU(subroutines[-1], [i], z)
#     alu.run()
#     print(alu)

# w   11151
# z i 0
# z o 3+w
