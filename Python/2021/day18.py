import sys
import json
import math
import itertools
# initializing string representation of a list
# ini_list = "[[1,2],3]"
  
# # printing initialized string of list and its type
# print ("initial string", ini_list)
# print (type(ini_list))
  
# # Converting string to list
# res = json.loads(ini_list)
  
# # printing final result and its type
# print ("final list", res)
# print (type(res))
# print (type(res[1]))

def printNumber(list):
    print(str(list).replace(' ', ''))

def add(leftList, rightList):
    return [leftList, rightList] if leftList is not None else rightList

def split(node, left):
    toSplit = node[0]/2 if left else node[1]/2
    leftValue = math.floor(toSplit)
    rightValue = math.ceil(toSplit)
    if left:
        node[0] = [leftValue, rightValue]
    else:
        node[1] = [leftValue, rightValue]


def traverseLeftToRight(node):
    if type(node[0]) is list:
        traverseLeftToRight(node[0])
    if type(node[1]) is list:
        traverseLeftToRight(node[1])

def traverseAndSplit(node):
    if type(node[0]) is list:
        done = traverseAndSplit(node[0])
        if done:
            return done
    else:
        if node[0] > 9:
            split(node, True)
            return True
    if type(node[1]) is list:
        return traverseAndSplit(node[1])
    else:
        if node[1] > 9:
            split(node, False)
            return True
    return False

def traverseAndExplode(node, depth, leftMostNumber, isLeft, last, lastIsLeft, toAdd):
    if toAdd is not None and type(node[0]) is int:
        node[0] += toAdd
        return (True, 0, True, None, False)
    if toAdd is None and depth >= 4 and type(node[0]) is int and type(node[1]) is int:
        # Explode
        # Add to left:
        if leftMostNumber is not None:
            if isLeft:
                leftMostNumber[0] += node[0]
            else:
                leftMostNumber[1] += node[0]
        # replace with zero
        if lastIsLeft:
            last[0] = 0
        else:
            last[1] = 0
        # add to right:
        return (True, node[1], False, None, False)
    
    if type(node[0]) is list:
        (exploded, newToAdd, done, leftMostNumber, isLeft) = traverseAndExplode(node[0], depth+1, leftMostNumber, isLeft, node, True, toAdd)
        if done:
            return (True, 0, True, None, False)
        exploded = exploded
        toAdd = newToAdd
    else:
        leftMostNumber = node
        isLeft = True
    if type(node[1]) is list:
        (exploded, newToAdd, done, leftMostNumber, isLeft) = traverseAndExplode(node[1], depth+1, leftMostNumber, isLeft, node, False, toAdd)
        if done:
            return (True, 0, True, None, False)
        return (exploded, newToAdd, done, leftMostNumber, isLeft)
    else:
        if toAdd is not None:
            node[1] += toAdd
            return (True, 0, True, None, False)
        leftMostNumber = node
        isLeft = False

    return (False, None, False, leftMostNumber, isLeft)

def traverseAndGetMagnitude(node):
    if type(node[0]) is list:
        left = traverseAndGetMagnitude(node[0])
    else:
        left = node[0]
    if type(node[1]) is list:
        right = traverseAndGetMagnitude(node[1])
    else:
        right = node[1]
    return left*3 + right*2

def reduce(number):
    exploded = True
    splited = True
    while exploded or splited:
        #printNumber(number)
        (exploded, _, _, _, _) = traverseAndExplode(number, 0, None, False, None, False, None)
        if exploded:
            continue
        splited = traverseAndSplit(number)
    #printNumber(number)
    return number

# test = [1,[[15,3],13]]
# printNumber(test)
# done = traverseAndSplit(test)
# printNumber(test)
# print(done)

# testexp = [[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]
# done = traverseAndExplode(testexp, 0, None, False, None, False, None)
# printNumber(testexp)
# print(done)

# testMagnitude = [[1,2],[[3,4],5]]
# magnitude = traverseAndGetMagnitude(testMagnitude)
# printNumber(magnitude)
# exit()
fakeInput= ["[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]",
            "[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]"]
number = None
input = [x.rstrip() for x in open(sys.argv[1])] if sys.argv[0].endswith('day18.py') and len(sys.argv) > 1 else fakeInput
for row in input:
    toAdd = json.loads(row)
    number = add(number, toAdd)
    #printNumber(number)
    # reduce
    number = reduce(number)

magnitude  = traverseAndGetMagnitude(number)
print(magnitude )

maxMagnitude = 0
for l, r in itertools.combinations(input, 2):
    left = json.loads(l)
    right = json.loads(r)
    numberPartTwo = add(left, right)

    numberPartTwo = reduce(numberPartTwo)
    mag2 = traverseAndGetMagnitude(numberPartTwo)
    if mag2 > maxMagnitude:
        maxMagnitude = mag2

    left = json.loads(l)
    right = json.loads(r)
    numberPartTwo = add(right, left)

    numberPartTwo = reduce(numberPartTwo)
    mag2 = traverseAndGetMagnitude(numberPartTwo)
    if mag2 > maxMagnitude:
        maxMagnitude = mag2

print(maxMagnitude)