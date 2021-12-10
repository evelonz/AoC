import fileinput

count = 0
input = [x.rstrip() for x in fileinput.input(encoding="utf-16")]
for index, line in enumerate(input):
    output = line.split(" | ")[1].split(' ')
    for digits in output:
        c = len(digits)
        if c == 2 or c == 3 or c == 4 or c == 7:
            count += 1
print(count)

# part 2

def filterOutDigitAndSegment(digits, filter, length):
    for digit in digits:
        stripedSegment = digit.translate({ ord(c): None for c in filter })
        if len(stripedSegment) == 1 and len(digit) == length: # only 9 really needs the length
            return (digit, stripedSegment)

count2 = 0
for line in input:
    #line = "acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf"
    knownDigits = [""] * 10
    digits = line.split(" | ")[0].split(' ')

    # known digits
    
    one = next(x for x in digits if len(x) == 2)
    knownDigits[1] = one
    four = next(x for x in digits if len(x) == 4)
    knownDigits[4] = four
    seven = next(x for x in digits if len(x) == 3)
    knownDigits[7] = seven
    eight = next(x for x in digits if len(x) == 7)
    knownDigits[8] = eight
    # 7 minus 1 gives us the top segment.
    topSegment = next(x for x in seven if one.find(x) == -1)

    # 9 is the only digit with one more segment than 4 + top.
    # Need to check len as well, since there are shorter ones with one segment left.
    (knownDigits[9], bottomSegment) = filterOutDigitAndSegment(digits, four + topSegment, 6)
    # 3 is the only digit with one more segment than 1+ top + bottom
    (knownDigits[3], middleSegment) = filterOutDigitAndSegment(digits, one+topSegment+bottomSegment, 5)

    # left segments
    topLeftSegment = four.translate({ ord(c): None for c in middleSegment+one })
    bottomLeftSegment = eight.translate({ ord(c): None for c in knownDigits[9] })

    # five and right segments
    allButBottomRight = topSegment + middleSegment + bottomSegment + topLeftSegment
    (knownDigits[5], bottomRightSegment) = filterOutDigitAndSegment(digits, allButBottomRight, 5)

    topRightSegment = one.translate({ ord(c): None for c in bottomRightSegment })

    # Since the order of characters in the output digits differ from the input, then we
    # don't have to find exact matches. Instead we can construct 6 and 0 based of 8.
    knownDigits[6] = eight.translate({ ord(c): None for c in topRightSegment }) 
    knownDigits[0] = eight.translate({ ord(c): None for c in middleSegment })
    # Since 0 and 6 may not match the input, we just have to check the length as well to find 2.
    twoOptions = list(set(digits).difference(knownDigits))
    two = list(filter(lambda digit: len(digit) == 5, twoOptions))[0]
    knownDigits[2] = two

    output = line.split(" | ")[1].split(' ')
    result = ""
    for y in output:
        for index, x in enumerate(knownDigits):
            stripedSegment = x.translate({ ord(c): None for c in y })
            if len(stripedSegment) == 0 and len(x) == len(y):
                result += str(index)
                break
    count2 += int(result)

print(count2)