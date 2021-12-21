import sys
import itertools
from collections import namedtuple

# Since index 0 returns #, and index 511 returns ., and infinite number of pixels will flicker.
# I suspect it's like this on everyones input.

fakeInput= ["#.#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#...",
            "",
            "...#.",
            "#....",
            ".....",
            "....#",
            ".#..."]
Bounds = namedtuple('Bounds', 'minX maxX minY maxY')
#input = [x.rstrip() for x in fileinput.input(encoding="utf-16")]
input = [x.rstrip() for x in open(sys.argv[1])] if sys.argv[0].endswith('day20_2.py') and len(sys.argv) > 1 else fakeInput
algoKey = input[0]
currentImage = set()
for y, line in enumerate(input[2:]):
    for x, char  in enumerate(line):
        if char == "#":
            currentImage.add((x, y))

def getBounds(image):
    minX = float('inf')
    minY = float('inf')
    maxX = float('-inf')
    maxY = float('-inf')
    for pixel in image:
        if pixel[0] < minX:
            minX = pixel[0]
        if pixel[0] > maxX:
            maxX = pixel[0]
        if pixel[1] < minY:
            minY = pixel[1]
        if pixel[1] > maxY:
            maxY = pixel[1]
    return Bounds(minX, maxX, minY, maxY)

def printImage(image, bounds):
    for y in range(bounds.minY-1, bounds.maxY+2):
        p = ""
        for x in range(bounds.minX-1, bounds.maxX+2):
            p += "#" if (x, y) in image else "."
        print(p)
    print()

def isOutOfBounds(x, y, bounds):
    return x < bounds.minX or x > bounds.maxX or y < bounds.minY or y > bounds.maxY

def getChar(x, y, image, bounds, oobValue):
    return "1" if (x, y) in image else oobValue if isOutOfBounds(x, y, bounds) else "0"

def getCharIndex(pixel, image, bounds, oobValue):
    charIndex = list("000000000")
    charIndex[0] = getChar(pixel[0]-1, pixel[1]-1, image, bounds, oobValue)
    charIndex[1] = getChar(pixel[0]  , pixel[1]-1, image, bounds, oobValue)
    charIndex[2] = getChar(pixel[0]+1, pixel[1]-1, image, bounds, oobValue)

    charIndex[3] = getChar(pixel[0]-1, pixel[1]  , image, bounds, oobValue)
    charIndex[4] = getChar(pixel[0]  , pixel[1]  , image, bounds, oobValue)
    charIndex[5] = getChar(pixel[0]+1, pixel[1]  , image, bounds, oobValue)

    charIndex[6] = getChar(pixel[0]-1, pixel[1]+1, image, bounds, oobValue)
    charIndex[7] = getChar(pixel[0]  , pixel[1]+1, image, bounds, oobValue)
    charIndex[8] = getChar(pixel[0]+1, pixel[1]+1, image, bounds, oobValue)

    return ''.join(charIndex)

#print(currentImage)
zeroValue = "0" if algoKey[0] == '.' else "1"
maxValue = "1" if algoKey[:-1] == '#' and algoKey[0] == '#' else "0"
for step in range(50):
    bounds = getBounds(currentImage)
    #print(len(currentImage))
    #print(bounds)
    #printImage(currentImage, bounds)
    oobValue = maxValue if step % 2 == 0 else zeroValue
    newImage = set()
    for y in range(bounds.minY-1, bounds.maxY+2):
        for x in range(bounds.minX-1, bounds.maxX+2):
            if x == 5 and y == 0:
                a=1
            charIndex = getCharIndex((x, y), currentImage, bounds, oobValue)
            decimalIndex = int(charIndex, 2)
            newPixel = algoKey[decimalIndex]
            if newPixel == "#":
                newImage.add((x, y))
    currentImage = newImage
    if step == 1:
        print(len(currentImage))

print(len(currentImage))
