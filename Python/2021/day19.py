import math
import unittest
import sys
import itertools

#input = [x.rstrip() for x in fileinput.input(encoding="utf-16")]
input = [x.rstrip() for x in open(sys.argv[1])] if sys.argv[0].endswith('day19.py') and len(sys.argv) > 1 else []
print(input)
currentScanner = None
scanners = []
for line in input:
    if line == "":
        scanners.append(currentScanner)
        continue
    if line.startswith('--'):
        currentScanner = []
        continue
    left = int(line.split(',')[0])
    right = int(line.split(',')[1])
    currentScanner.append((left,right))
scanners.append(currentScanner)

print(scanners)
#print('\n'.join([''.join(['{:5}'.format(item) for item in row]) for row in scanners]))

# 1. Place two points in the same location.
# 2. Shift all points for one of the scanner into the coordinates of the first scanner.
# 3. Check how many matching points we get.
# 4. If not enough matching points, then overlay two other points and try again.
# 5. If still not enough, then start rotating one of the scanners points.
# 6. If no match found, move on to the next scanner.
for l, r in itertools.combinations(range(len(scanners)), 2):
    print(scanners[l])
    print(scanners[r])
    for point1, point2 in itertools.product(scanners[l], scanners[r]):
        print(point1)
        print(point2)
        # 1. Place two points in the same location.
        deltaX = point1[0] - point2[0]
        deltaY = point1[1] - point2[1]
        print(deltaX, deltaY)
        newSet = set([(tuple[0]+deltaX, tuple[1]+deltaY) for tuple in scanners[r]])
        print(newSet)
        overlap = 0
        for item in scanners[l]:
            #print(item)
            if item in newSet:
                overlap += 1
        print("Overlap:", overlap)
        if overlap == 3:
            break