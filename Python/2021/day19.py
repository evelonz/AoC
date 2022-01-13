import math
import unittest
import sys
import itertools

#input = [x.rstrip() for x in fileinput.input(encoding="utf-16")]
input = [x.rstrip() for x in open(sys.argv[1])] if sys.argv[0].endswith('day19.py') and len(sys.argv) > 1 else []
# print(input)
currentScanner = None
scanners = []
for line in input:
    if line == "":
        scanners.append(currentScanner)
        continue
    if line.startswith('--'):
        currentScanner = []
        continue
    x = int(line.split(',')[0])
    y = int(line.split(',')[1])
    z = int(line.split(',')[2])
    currentScanner.append((x,y,z))
scanners.append(currentScanner)

# print(scanners)
#print('\n'.join([''.join(['{:5}'.format(item) for item in row]) for row in scanners]))

def rotatedPoints(points):
    yield [(p[0]   , p[1]   , p[2]   ) for p in points]
    yield [(p[0]   , p[1]*-1, p[2]   ) for p in points]
    yield [(p[0]   , p[1]   , p[2]*-1) for p in points]
    yield [(p[0]   , p[1]*-1, p[2]*-1) for p in points]

    yield [(p[0]*-1, p[1]   , p[2]   ) for p in points]
    yield [(p[0]*-1, p[1]*-1, p[2]   ) for p in points]
    yield [(p[0]*-1, p[1]   , p[2]*-1) for p in points]
    yield [(p[0]*-1, p[1]*-1, p[2]*-1) for p in points]
    # x - y
    yield [(p[1]   , p[0]   , p[2]   ) for p in points]
    yield [(p[1]   , p[0]*-1, p[2]   ) for p in points]
    yield [(p[1]   , p[0]   , p[2]*-1) for p in points]
    yield [(p[1]   , p[0]*-1, p[2]*-1) for p in points]

    yield [(p[1]*-1, p[0]   , p[2]   ) for p in points]
    yield [(p[1]*-1, p[0]*-1, p[2]   ) for p in points]
    yield [(p[1]*-1, p[0]   , p[2]*-1) for p in points]
    yield [(p[1]*-1, p[0]*-1, p[2]*-1) for p in points]
    # x - z
    yield [(p[2]   , p[1]   , p[0]   ) for p in points]
    yield [(p[2]   , p[1]*-1, p[0]   ) for p in points]
    yield [(p[2]   , p[1]   , p[0]*-1) for p in points]
    yield [(p[2]   , p[1]*-1, p[0]*-1) for p in points]

    yield [(p[2]*-1, p[1]   , p[0]   ) for p in points]
    yield [(p[2]*-1, p[1]*-1, p[0]   ) for p in points]
    yield [(p[2]*-1, p[1]   , p[0]*-1) for p in points]
    yield [(p[2]*-1, p[1]*-1, p[0]*-1) for p in points]
    # y - z
    yield [(p[0]   , p[2]   , p[1]   ) for p in points]
    yield [(p[0]   , p[2]*-1, p[1]   ) for p in points]
    yield [(p[0]   , p[2]   , p[1]*-1) for p in points]
    yield [(p[0]   , p[2]*-1, p[1]*-1) for p in points]

    yield [(p[0]*-1, p[2]   , p[1]   ) for p in points]
    yield [(p[0]*-1, p[2]*-1, p[1]   ) for p in points]
    yield [(p[0]*-1, p[2]   , p[1]*-1) for p in points]
    yield [(p[0]*-1, p[2]*-1, p[1]*-1) for p in points]

# for index, i in enumerate(rotatedPoints(scanners[0])):
#     print(index, "--------------------------")
#     for point in i:
#         print(str(point[0])+","+str(point[1])+","+str(point[2]))
# exit()

# 1. Place two points in the same location.
# 2. Shift all points for one of the scanner into the coordinates of the first scanner.
# 3. Check how many matching points we get.
# 4. If not enough matching points, then overlay two other points and try again.
# 5. If still not enough, then start rotating one of the scanners points.
# 6. If no match found, move on to the next scanner.

# if match found, update coordinates for one of the scanners and add to a set.
# for l, r in itertools.combinations(range(len(scanners)), 2):
matches = []
matches.append(0) # Set scanner 0 as root
while len(matches) < len(scanners):
    for l in matches:
        for r in range(len(scanners)):
            if r in matches:
                continue
            print(l, r)
            # print(scanners[l])
            # print(scanners[r])
            done = False
            for secondScannerPoints in rotatedPoints(scanners[r]):
                # for secondScannerPoints2 in rotatedPoints(scanners[l]):
                for point1, point2 in itertools.product(scanners[l], secondScannerPoints):
                    # print(point1)
                    # print(point2)
                    # 1. Place two points in the same location.
                    # if point2 == (-618,-824,-621): # and point2 == (686,422,578):
                    #     print("Found:", point1, point2)
                    deltaX = point1[0] - point2[0]
                    deltaY = point1[1] - point2[1]
                    deltaZ = point1[2] - point2[2]
                    # if deltaX != 68 or deltaY != -1246 or deltaZ != -43:
                    #     break
                    #print(deltaX, deltaY, deltaZ)
                    newSet = set([(tuple[0]+deltaX, tuple[1]+deltaY, tuple[2]+deltaZ) for tuple in secondScannerPoints])
                    #print(newSet)
                    overlap = 0
                    for item in scanners[l]:
                        #print(item)
                        if item in newSet:
                            overlap += 1
                    # if overlap > 0:

                    if overlap == 12:
                        print("Overlap:", overlap, r)  
                        done = True
                        matches.append(r)
                        # update coords
                        tmp = []
                        for cord in secondScannerPoints:
                            tmp.append((cord[0]+deltaX, cord[1]+deltaY, cord[2]+deltaZ))
                            #print((cord[0]+deltaX, cord[1]+deltaY, cord[2]+deltaZ))
                        scanners[r] = tmp
                        break
                if done:
                    break

beacons = set()
for s in scanners:
    for point in s:
        beacons.add(point)
print(len(beacons))