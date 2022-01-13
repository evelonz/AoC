import math
from typing import overload
import unittest
import sys
import itertools
from collections import namedtuple
import time
import copy

fake = [['.', '.', '.', '>', '.', '.', '.'], ['.', '.', '.', '.', '.', '.', '.'], ['.', '.', '.', '.', '.', '.', '>'], ['v', '.', '.', '.', '.', '.', '>'], ['.', '.', '.', '.', '.', '.', '>'], ['.', '.', '.', '.', '.', '.', '.'], ['.', '.', 'v', 'v', 'v', '.', '.']]
#input = [x.rstrip() for x in fileinput.input(encoding="utf-16")]
input = [x.rstrip() for x in open(sys.argv[1])] if sys.argv[0].endswith('day25.py') and len(sys.argv) > 1 else fake
print(input)

matrix = []
for line in input:
    matrix.append(list(line))
rows = len(matrix)
cols = len(matrix[0])
print(rows, cols)
#print(matrix)
#print('\n'.join([''.join(['{:1}'.format(item) for item in row]) for row in matrix]))

# for i in range(60):
moved = True
step = 0
while moved:
    step += 1
    print("step", step)
    moved = False
    tempMatrix = copy.deepcopy(matrix)
    for rowp, line in enumerate(matrix[::-1]):
        for colp, char in enumerate(line[::-1]):
            row = rows-1-rowp
            col = cols-1-colp
            check = (col+1) % cols
            # print(check)
            if matrix[row][col] == '>' and matrix[row][check] == '.':
                tempMatrix[row][col] = '.'
                tempMatrix[row][check] = '>'
                moved = True
    matrix = tempMatrix
    tempMatrix = copy.deepcopy(matrix)
    #print('\n'.join([''.join(['{:1}'.format(item) for item in row]) for row in matrix]))

    for rowp, line in enumerate(matrix[::-1]):
        for colp, char in enumerate(line[::-1]):
            row = rows-1-rowp
            col = cols-1-colp
            check = (row+1) % rows
            # print(check)
            if matrix[row][col] == 'v' and matrix[check][col] == '.':
                tempMatrix[row][col] = '.'
                tempMatrix[check][col] = 'v'
                moved = True
    matrix = tempMatrix
    if not moved:
        break;
# print(tempMatrix)
#print('\n'.join([''.join(['{:1}'.format(item) for item in row]) for row in matrix]))
