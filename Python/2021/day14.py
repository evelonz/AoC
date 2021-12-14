from collections import defaultdict
import fileinput

input = [x.rstrip() for x in fileinput.input(encoding="utf-16")]
pairInsertion = {}
template = []

for line in input:
    if line == "":
        continue
    elif line.__contains__('->'):
        s = line.split(' -> ')
        left = s[0]
        right = s[1]
        pairInsertion[left] = right
    else:
        template = line

# optimize for part 2
templatePairs = defaultdict(int)
for j in range(len(template)-1):
    find = template[j] + template[j+1]
    templatePairs[find] += 1

def printResult(templatePairs):
    distinct = defaultdict(int)
    for find, value in templatePairs.items():
        distinct[find[0]] += value
        distinct[find[1]] += value

    minCount = float('inf')
    maxCount = 0
    for _, count in distinct.items():
        if count < minCount:
            minCount = count
        if count > maxCount:
            maxCount = count

    # Since we count most characters twice in each pair, we have to divide by 2.
    # Since one character may be at the end of the string, we do a celling devision.
    # Not sure if this will work if the same character is at both ends of the string.
    minCount = -(minCount // -2)
    maxCount = -(maxCount // -2)
    print(maxCount - minCount)

for i in range(40):
    tempTemplatePairs = defaultdict(int)
    for find, value in templatePairs.items():
        if find in pairInsertion:
            insert = pairInsertion[find]
            tempTemplatePairs[find[0] + insert] += value
            tempTemplatePairs[insert + find[1]] += value
        else:
            tempTemplatePairs[find] = value
    templatePairs = tempTemplatePairs
    if(i == 9):
        printResult(templatePairs)

printResult(templatePairs)
