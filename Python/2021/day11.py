

from collections import namedtuple
import fileinput

octopi = []
input = [x.rstrip() for x in fileinput.input(encoding="utf-16")]
for row in input:
    tempArray = []
    for energy in row:
        tempArray.append(int(energy))
    octopi.append(tempArray)

rows = len(octopi)
cols = len(octopi[0])
Point = namedtuple('Point', 'row col')
flashes = 0
firstSimultaneousFlash = 0
octopiCount = rows*cols

def getNeighbour(matrix, point):
    (row, col) = point
    result = []
    # Cross
    if col != 0 and matrix[row][col-1] > 0:
        result.append(Point(row, col-1))
    if col != cols-1 and matrix[row][col+1] > 0:
        result.append(Point(row, col+1))
    if row != 0 and matrix[row-1][col] > 0:
        result.append(Point(row-1, col))
    if row != rows-1 and matrix[row+1][col] > 0:
        result.append(Point(row+1, col))
    # Corners
    if row != 0 and col != 0 and matrix[row-1][col-1] > 0:
        result.append(Point(row-1, col-1))
    if row != 0 and col != cols-1 and matrix[row-1][col+1] > 0:
        result.append(Point(row-1, col+1))
    if row != rows-1 and col != cols-1 and matrix[row+1][col+1] > 0:
        result.append(Point(row+1, col+1))
    if row != rows-1 and col != 0 and matrix[row+1][col-1] > 0:
        result.append(Point(row+1, col-1))
    return result

for day in range(1, 2000): # 2000 is just an arbitrary limit.
    partialCount = 0
    neighbors = []
    for row in range(0, rows):
        for col in range(0, cols):
            if octopi[row][col] + 1 > 9:
                neighbors.append(Point(row, col))
                octopi[row][col] = 0
                partialCount += 1
            else:
                octopi[row][col] += 1
    # Handling of flashes.
    # Each flash (energy > 9) increases energy of neighbour's.
    # After a flash, that octopus is set to 0 and cannot increase or flash again.
    # Instead of looping the matrix multiple times, we should do a first loop, create a set, and 
    # then just append and pop from it.
    while neighbors:
        point = neighbors.pop()
        for neighbour in getNeighbour(octopi, point):
            value = octopi[neighbour.row][neighbour.col]
            if value == 0:
                continue
            elif value + 1 > 9:
                neighbors.append(Point(neighbour.row, neighbour.col))
                octopi[neighbour.row][neighbour.col] = 0
                partialCount += 1
            else:
                octopi[neighbour.row][neighbour.col] += 1
    # Part one
    if day < 101:
        flashes += partialCount
    elif(day == 101): 
        print(flashes)
    # part two
    if(partialCount == octopiCount):
        print(day)
        break

#print('\n'.join([''.join(['{:1}'.format(item) for item in row]) for row in octopi]))
