import fileinput
from collections import namedtuple
import heapq
from functools import reduce

lowpoints = []
input = [x.rstrip() for x in fileinput.input(encoding="utf-16")]
matrix = []
rows = len(input)
cols = len(input[0])
Node = namedtuple('Node', 'value row col')
for row, line in enumerate(input):
    digits = []
    for col, digit in enumerate(line):
        digits.append(Node(int(digit), row, col))
    matrix.append(digits)
# print(matrix)
for row in range(0, rows):
    for col in range(0, cols):
        current = matrix[row][col].value
        if col != 0 and matrix[row][col-1].value <= current:
            continue
        if col != cols-1 and matrix[row][col+1].value <= current:
            continue
        if row != 0 and matrix[row-1][col].value <= current:
            continue
        if row != rows-1 and matrix[row+1][col].value <= current:
            continue
        lowpoints.append(matrix[row][col])

sumPartOne = sum([x[0]+1 for x in lowpoints])
print(sumPartOne)

def getNeighbour(matrix, node):
    (value, row, col) = node
    result = []
    if col != 0 and matrix[row][col-1].value >= value:
        result.append(matrix[row][col-1])
    if col != cols-1 and matrix[row][col+1].value >= value:
        result.append(matrix[row][col+1])
    if row != 0 and matrix[row-1][col].value >= value:
        result.append(matrix[row-1][col])
    if row != rows-1 and matrix[row+1][col].value >= value:
        result.append(matrix[row+1][col])
    return result

def bfs(matrix, node):
    visited = []
    visited.append(node)
    queue = []
    queue.append(node)
    count = 1

    while queue:
        s = queue.pop(0) 
        for neighbour in getNeighbour(matrix, matrix[s.row][s.col]):
            if neighbour not in visited and neighbour.value != 9:
                visited.append(neighbour)
                count += 1
                queue.append(neighbour)
    return count

# start at the low point and search out to edges and nines. BFS
basinSizes = []
for node in lowpoints:
    basinSize = bfs(matrix, node)
    basinSizes.append(basinSize)

largest = heapq.nlargest(3, basinSizes)
productPartTwo = reduce((lambda x, y: x * y), largest)
print(productPartTwo)