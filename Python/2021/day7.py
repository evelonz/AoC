import fileinput
from collections import deque
from itertools import repeat

crabs = [int(x) for x in next(fileinput.input(encoding="utf-16")).split(',')]
minPos = min(crabs)
maxPos = max(crabs)
minFuelUsed = float('inf')
minFuelUsed2 = float('inf')

for currentPos in range(minPos, maxPos):
    fuelUsed = 0
    fuelUsed2 = 0
    for crab in crabs:
        delta = abs(crab-currentPos)
        fuelUsed += delta
        fuelUsed2 += (delta*(delta+1))/2
    if(fuelUsed < minFuelUsed):
        minFuelUsed = fuelUsed
    if(fuelUsed2 < minFuelUsed2):
        minFuelUsed2 = fuelUsed2

print(minFuelUsed)
print(minFuelUsed2)