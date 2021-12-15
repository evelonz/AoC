from collections import namedtuple
import fileinput
from queue import PriorityQueue

input = [x.rstrip() for x in fileinput.input(encoding="utf-16")]
Node = namedtuple('Node', 'value, row col')
rows = len(input)
cols = len(input[0])
matrix = []

for rowindex, line in enumerate(input):
    row = []
    for col, char in enumerate(line):
        row.append(Node(int(char), rowindex, col))
    matrix.append(row)

#print('\n'.join([''.join(['{:1}'.format(item.value) for item in row]) for row in matrix]))

# Part two, expand the map.
newMatrix = []
for i in range(rows * 5):
    newMatrix.append([])

tmpRow = []
for row in range(5):
    for col in range(5):
        for innerRow in range(rows):
            tmp = [Node(((item.value + col + row - 1) % 9) + 1, innerRow + (rows * row), item.col + (cols * col)) for item in matrix[innerRow]]
            newMatrix[innerRow + (rows * row)] = newMatrix[innerRow + (rows * row)] + tmp

#print('\n'.join([''.join(['{:1}'.format(item.value) for item in row]) for row in newMatrix]))

def getNeighbour(matrix, node, gridSize):
    (_, row, col) = node
    result = []
    if col != 0:
        result.append(matrix[row][col-1])
    if col != gridSize-1:
        result.append(matrix[row][col+1])
    if row != 0:
        result.append(matrix[row-1][col])
    if row != gridSize-1:
        result.append(matrix[row+1][col])
    return result

def dijkstra(graph, startNode, gridSize):
    distanceMap = []
    for _ in range(gridSize):
        tmp = []
        for _ in range(gridSize):
            tmp.append(float('inf'))
        distanceMap.append(tmp)

    distanceMap[startNode.row][startNode.col] = 0
    visited = set()

    pq = PriorityQueue()
    pq.put((0, startNode))

    while not pq.empty():
        (_, currentNode) = pq.get()
        visited.add(currentNode)

        for neighbor in getNeighbour(graph, currentNode, gridSize):
            costOfEntry = graph[neighbor.row][neighbor.col].value
            if neighbor not in visited:
                old_cost = distanceMap[neighbor.row][neighbor.col]
                new_cost = distanceMap[currentNode.row][currentNode.col] + costOfEntry
                if new_cost < old_cost:
                    pq.put((new_cost, neighbor))
                    distanceMap[neighbor.row][neighbor.col] = new_cost
    return distanceMap

# There is a shorter path through the extended matrix, so still need to run the small one!
distances1 = dijkstra(matrix, matrix[0][0], rows)
print(distances1[rows-1][cols-1])

distances2 = dijkstra(newMatrix, newMatrix[0][0], rows*5)
print(distances2[(rows*5)-1][(cols*5)-1])