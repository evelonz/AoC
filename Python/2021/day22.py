import math
from typing import overload
import unittest
import sys
import itertools
from collections import namedtuple

#input = [x.rstrip() for x in fileinput.input(encoding="utf-16")]
input = [x.rstrip() for x in open(sys.argv[1])] if sys.argv[0].endswith('day22.py') and len(sys.argv) > 1 else []
#print(input)
Instr = namedtuple('Instr', 'on x1 x2 y1 y2 z1 z2')

instructions = []
for line in input:
    onOff = line.split(' ')[0] == "on"
    axis = line.split(' ')[1].split(',')
    xs = axis[0][2:].split('..')
    x1 = int(xs[0])
    x2 = int(xs[1])
    ys = axis[1][2:].split('..')
    y1 = int(ys[0])
    y2 = int(ys[1])
    zs = axis[2][2:].split('..')
    z1 = int(zs[0])
    z2 = int(zs[1])
    instr = Instr(onOff, x1, x2, y1, y2, z1, z2)
    # print(instr)
    instructions.append(instr)
    

# Part one has a size complexity of 100^3=1 000 000
# While part two is estimated to be 250 000^3 = 1.56 × 10^16
# We can no longer loop the axis.
# Instead just count the number of cubes in the volume.
# If intersecting with others, we create a new cube to 
# - turn cubes off
# - check if cubes are already on
# - turn cubes that are off, in an volume of an active cubes, back on.
# The third part will be the most tricky to solve.

class Cuboid:
    x1
    x2
    y1
    y2
    z1
    z2
    instr

    def __init__(self, instr):
        self.x1 = instr.x1
        self.x2 = instr.x2
        self.y1 = instr.y1
        self.y2 = instr.y2
        self.z1 = instr.z1
        self.z2 = instr.z2
        self.instr = instr

    def __str__(self):
        return str(instr)

    def isOverlaping(self, other):
        if (self.x1 > other.x2): return False
        if (self.x2 < other.x1): return False
        if (self.y1 > other.y2): return False
        if (self.y2 < other.y1): return False
        if (self.z1 > other.z2): return False
        if (self.z2 < other.z1): return False
        return True

    def getOverlapVolume(self, other):
        # assumes overlap was checked with isOverlaping, else may return negative numbers.
        xDelta = min(self.x2, other.x2) - max(self.x1, other.x1) + 1
        yDelta = min(self.y2, other.y2) - max(self.y1, other.y1) + 1
        zDelta = min(self.z2, other.z2) - max(self.z1, other.z1) + 1
        return xDelta * yDelta * zDelta


activeCubes2 = 0
cuboids = []
darkAreas = []
for instr in instructions:
    c = Cuboid(instr)
    if instr.on:
        subtract = 0
        for otherC in cuboids:
            if c.isOverlaping(otherC):
                subtract += c.getOverlapVolume(otherC)
        activeCubes2 += (abs(instr.x2-instr.x1)+1) * (abs(instr.y2-instr.y1)+1) * (abs(instr.z2-instr.z1)+1)
        activeCubes2 -= subtract
        cuboids.append(c)
    else:
        subtract = 0
        for otherC in cuboids:
            if c.isOverlaping(otherC):
                subtract += c.getOverlapVolume(otherC)
        activeCubes2 -= subtract
        darkAreas.append(c)

print(activeCubes2)
exit()

# Part one
activeCubes = set()
for instr in instructions:
    print(len(activeCubes), instr)
    xx = (instr.x1 > 50 and instr.x2 > 50) or (instr.x1 < -50 and instr.x2 < -50)
    yy = (instr.y1 > 50 and instr.y2 > 50) or (instr.y1 < -50 and instr.y2 < -50)
    zz = (instr.z1 > 50 and instr.z2 > 50) or (instr.z1 < -50 and instr.z2 < -50)
    if xx or yy or zz:
        print(xx, yy, zz, len(activeCubes))
        continue
    for x in range(instr.x1, instr.x2+1):
        for y in range(instr.y1, instr.y2+1):
            for z in range(instr.z1, instr.z2+1):
                if instr.on:
                    activeCubes.add((x, y, z))
                else:
                    activeCubes.discard((x, y, z))
print(len(activeCubes))
