import fileinput

input = [x.rstrip() for x in fileinput.input(encoding="utf-16")]
# input = ["<{([{{}}[<[[[<>{}]]]>[]]"]
opens = { '(': ')', '[': ']', '{': '}', '<': '>' }
closes = { ')': '(', ']': '[', '}': '{', '>': '<' }
checkerScores = {
    ')': 3,
    ']': 57,
    '}': 1197,
    '>': 25137
    }
autocompleteScores = {
    ')': 1,
    ']': 2,
    '}': 3,
    '>': 4
    }
score = 0
score2 = []

for line in input:
    queue = []
    corrupted = False
    for character in line:
        if character in opens:
            queue.append(character)
        # else we expect a closing character
        elif character in closes:
            lastOpen = queue.pop()
            #print("char ", character, " compare ", closes[character], " with ", lastOpen, " result: ", closes[character] != lastOpen)
            if closes[character] != lastOpen:
                score += checkerScores[character]
                corrupted = True
                break
    # if we reach the end, then the line is either complete or incomplete.
    # It's incomplete if the queue is not zero. For this case we calculate the score for part two
    if not corrupted and len(queue) > 0:
        tempScore = 0
        for openChar in queue[::-1]:
            closingChar = opens[openChar]
            # print(tempScore)
            # print(tempScore * 5, " + ", autocompleteScores[closingChar])
            tempScore = ((tempScore * 5) + autocompleteScores[closingChar])
            # print(tempScore)
        score2.append(tempScore)

print(score)
score2.sort()
print(score2[int(len(score2)/2)])
# print(score2)