import fileinput
from collections import deque
from itertools import repeat

fishes = [0] * 9
fishesTemp = [0] * 9
partOneCount = 0
for number in next(fileinput.input(encoding="utf-16")).split(','):
# with open("2021/inputs/day1.txt") as f:
#     for line in f:
    fishes[int(number)] += 1

for day in range(0, 256):
    for index, fish in enumerate(fishes):
        if index == 0:
            fishesTemp[8] = fish # one new fish for each 0 day fish.
            fishesTemp[6] += fish
        else:
            fishesTemp[index-1] += fish
    fishes = fishesTemp
    fishesTemp = [0] * 9
    if(day == 0):
        partOneCount = sum(fishes)

partTwoCount = sum(fishes)
if __name__ == '__main__':
    print(partOneCount)
    print(partTwoCount)