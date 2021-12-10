import fileinput
from collections import deque
from itertools import repeat

increasingDepth = 0
increasingDepth2 = 0
lastDepth = float('inf')
firstWindow = list(repeat(float('inf'), 3))
secondWindow = list(repeat(float('inf'), 3))

# for line in fileinput.input(encoding="utf-16"):
with open("2021/inputs/day1.txt") as f:
    for line in f:
        currentDepth = int(line)
        if currentDepth > lastDepth:
            increasingDepth += 1

        firstWindow.pop(0)
        firstWindow.append(secondWindow[-1])
        secondWindow.pop(0)
        secondWindow.append(currentDepth)
        s1 = sum(secondWindow)
        s2 = sum(firstWindow)
        if sum(secondWindow) > sum(firstWindow):
            increasingDepth2 += 1

        lastDepth = currentDepth

print(increasingDepth)
print(increasingDepth2)

if __name__ == '__main__':
    print("ran from cli")