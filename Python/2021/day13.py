import fileinput

maxX = 0
maxY = 0
paper = set()
folds = []
input = [x.rstrip() for x in fileinput.input(encoding="utf-16")]

def printPaper(maxX, maxY, paper):
    for row in range(maxY+1):
        p = ""
        for col in range(maxX+1):
            if (col, row) in paper:
                p += "#"
            else:
                p += '.'
        print(p)

for line in input:
    if line == "":
        continue
    elif line.startswith('fold'):
        s = line.split(' ')[2]
        folds.append((s[0], int(s.split('=')[1])))
    else:
        x = int(line.split(',')[0])
        y = int(line.split(',')[1])
        if x > maxX:
            maxX = x
        if y > maxY:
            maxY = y
        paper.add((x, y))

# We always want an even number of lines, so we can fold it even on the given fold line.
if (maxY % 2) != 0:
    maxY += 1
if (maxX % 2) != 0:
    maxX += 1
#print(maxX, " ", maxY)
#printPaper(maxX, maxY, paper)

for index, line in enumerate(folds):
# line = folds[0]
    newPaper = set()
    if line[0] == 'y':
        y = line[1]
        for dot in paper:
            if dot[1] > y:
                newY = maxY - dot[1]
                newPaper.add((dot[0], newY))
            else:
                newPaper.add(dot)
        maxY = line[1] - 1
    elif line[0] == 'x':
        x = line[1]
        for dot in paper:
            if dot[0] > x:
                newX = maxX - dot[0]
                newPaper.add((newX, dot[1]))
            else:
                newPaper.add(dot)
        maxX = line[1] - 1
    if index == 0:
        print(len(newPaper))
    paper = newPaper

printPaper(maxX, maxY, paper)
