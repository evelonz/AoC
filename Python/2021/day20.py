import sys
import itertools
from collections import namedtuple

# Since index 0 returns #, and index 511 returns ., and infinite number of pixels will flicker.
# I suspect it's like this on everyones input.

fakeInput= ["..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#",
            "",
            "#..#.",
            "#....",
            "##..#",
            "..#..",
            "..###"]
Bounds = namedtuple('Bounds', 'minX maxX minY maxY')
#input = [x.rstrip() for x in fileinput.input(encoding="utf-16")]
input = [x.rstrip() for x in open(sys.argv[1])] if sys.argv[0].endswith('day20.py') and len(sys.argv) > 1 else fakeInput
#print(input)
algoKey = input[0]
currentImage = set()
for y, line in enumerate(input[2:]):
    for x, char  in enumerate(line):
        if char == "#":
            currentImage.add((x, y))
#print(currentImage)

def getCharIndex(pixel, image, bounds, step):
    charIndex = list("000000000")
    charIndex[0] = "1" if (pixel[0]-1, pixel[1]-1) in image else "0"
    charIndex[1] = "1" if (pixel[0]  , pixel[1]-1) in image else "0"
    charIndex[2] = "1" if (pixel[0]+1, pixel[1]-1) in image else "0"

    charIndex[3] = "1" if (pixel[0]-1, pixel[1]) in image else "0"
    charIndex[4] = "1" if (pixel[0]  , pixel[1]) in image else "0"
    charIndex[5] = "1" if (pixel[0]+1, pixel[1]) in image else "0"

    charIndex[6] = "1" if (pixel[0]-1, pixel[1]+1) in image else "0"
    charIndex[7] = "1" if (pixel[0]  , pixel[1]+1) in image else "0"
    charIndex[8] = "1" if (pixel[0]+1, pixel[1]+1) in image else "0"

    # Custom logic for infinite flickering pixels.
    # If flickered on, and outside the bound of the last check, then set to 1.
    # 
    if step % 2 == 0:
        return ''.join(charIndex)
    if pixel == (4, -1):
        a=2
    charIndex2 = list("000000000")
    charIndex2[0] = "1" if pixel[0]-1 < bounds.minX or pixel[1]-1 < bounds.minY else charIndex[0]
    charIndex2[1] = "1" if pixel[1]-1 < bounds.minY else charIndex[1]
    charIndex2[2] = "1" if pixel[0]-1 > bounds.maxX or pixel[1]-1 < bounds.minY else charIndex[2]

    charIndex2[3] = "1" if pixel[0]-1 < bounds.minX else charIndex[3]
    charIndex2[4] = charIndex[4]
    charIndex2[5] = "1" if pixel[0]-1 > bounds.maxX else charIndex[5]

    charIndex2[6] = "1" if pixel[0]-1 < bounds.minX or pixel[1]+1 > bounds.maxY else charIndex[6]
    charIndex2[7] = "1" if pixel[1]+1 > bounds.maxY else charIndex[7]
    charIndex2[8] = "1" if pixel[0]-1 > bounds.maxX or pixel[1]+1 > bounds.maxY else charIndex[8]
    
    if charIndex != charIndex2:
        print(pixel)
    return ''.join(charIndex2)

def setPixelsToCheck(image):
    pixelsToCheck = set()
    minX = 9999
    minY = 9999
    maxX = -9999
    maxY = -9999
    for pixel in image:
        pixelsToCheck.add((pixel[0]-1, pixel[1]-1))
        pixelsToCheck.add((pixel[0]  , pixel[1]-1))
        pixelsToCheck.add((pixel[0]+1, pixel[1]-1))

        pixelsToCheck.add((pixel[0]-1, pixel[1]))
        pixelsToCheck.add((pixel[0]  , pixel[1]))
        pixelsToCheck.add((pixel[0]+1, pixel[1]))

        pixelsToCheck.add((pixel[0]-1, pixel[1]+1))
        pixelsToCheck.add((pixel[0]  , pixel[1]+1))
        pixelsToCheck.add((pixel[0]+1, pixel[1]+1))

        if pixel[0]-1 < minX:
            minX = pixel[0]-1
        if pixel[0]+1 > maxX:
            maxX = pixel[0]+1
        if pixel[1]-1 < minY:
            minY = pixel[1]-1
        if pixel[1]+1 > maxY:
            maxY = pixel[1]+1
    return (pixelsToCheck, Bounds(minX, maxX, minY, maxY))

def printImage(image):
    minX = 9999
    minY = 9999
    maxX = -9999
    maxY = -9999
    for pixel in image:
        if pixel[0] < minX:
            minX = pixel[0]
        if pixel[0] > maxX:
            maxX = pixel[0]
        if pixel[1] < minY:
            minY = pixel[1]
        if pixel[1] > maxY:
            maxY = pixel[1]
    for y in range(minY, maxY+1):
        p = ""
        for x in range(minX, maxX+1):
            p += "#" if (x, y) in image else "."
        print(p)
    print()
print(len(currentImage))
#printImage(currentImage)


for step in range(2):
    (pixelsToCheck, bounds) = setPixelsToCheck(currentImage)
    print(bounds)
    newImage = set()
    for pixel in pixelsToCheck:
        if pixel[0] == 2 and pixel[1] == 2:
            a = 1
        charIndex = getCharIndex(pixel, currentImage, bounds, step)
        decimalIndex = int(charIndex, 2)
        newPixel = algoKey[decimalIndex]
        if newPixel == "#":
            newImage.add((pixel[0], pixel[1]))
    currentImage = newImage

# 5398 too high
# 5300 too high
# 675 too low
    print(len(currentImage))
    printImage(currentImage)

    #print(currentImage)