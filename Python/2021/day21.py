from collections import defaultdict
from collections import namedtuple
from queue import PriorityQueue

# Ignoring the parsing of the file.
inputP1 = 4
inputP2 = 1

playerOnePos = inputP1
playerTwoPos = inputP2
dieValue = 1
dieThrowCount = 0
playerOneScore = 0
playerTwoScore = 0

def rollDie(dieValue, playerPos):
    d1 = (dieValue - 1) % 100 + 1
    d2 = (dieValue + 1 - 1) % 100 + 1
    d3 = (dieValue + 2 - 1) % 100 + 1
    playerPosIncrease = playerPos + d1 + d2 + d3
    playerPosWrapped = (playerPosIncrease - 1) % 10 + 1
    newDieValue = (dieValue + 3 - 1) % 100 + 1
    return (newDieValue, playerPosWrapped, d1, d2, d3)

while playerOneScore < 1000 and playerTwoScore < 1000:
    (dieValue, playerOnePos, d1, d2, d3) = rollDie(dieValue, playerOnePos)
    playerOneScore += playerOnePos
    dieThrowCount += 3
    if playerOneScore >= 1000:
        break
    (dieValue, playerTwoPos, d1, d2, d3) = rollDie(dieValue, playerTwoPos)
    playerTwoScore += playerTwoPos
    dieThrowCount += 3

minValue = min(playerOneScore, playerTwoScore)
print(minValue*dieThrowCount)

# Part two
# I think we need to count the number of universes per position and score.
# So in the example, starting at 4 and 8 (with one die for simplicity):

# r1: 1: (5, 8)(5,0)[1]. 2: (6, 8)(6,0)[1]. 3: (7, 8)(7,0)[1]
# r2: 1.1: (5, 9)(5,9)[1]. 1.2: (5, 10)(5,10)[1]. 1.3: (5, 1)(5,1)[1]
# r2: 1.1: (6, 9)(6,9)[1]. 1.2: (6, 10)(6,10)[1]. 1.3: (6, 1)(6,1)[1]
# r2: 1.1: (7, 9)(7,9)[1]. 1.2: (7, 10)(7,10)[1]. 1.3: (7, 1)(7,1)[1]

# Then keep playing as long as there are games with score < 21.
# This should give us a space complexity of pos1 * pos2 * score1 * score2
# Which is about 10 * 10 * 23 * 23 = 52900 positions
# I think we need to track which player turn it is as well, so 105800 positions?
# (5, 8)(5,0)(1)[1], (2) here indicates that last move was done by player one,
# so we can just sum based on this for finished games.

games = defaultdict(int)
State = namedtuple('State', 'p1Pos p2Pos p1Score p2Score lastPlayer')

def playRound(state):
    startPos = state.p1Pos if state.lastPlayer == 2 else state.p2Pos
    result = []
    for die1 in range(1,4):
        for die2 in range(1,4):
            for die3 in range(1,4):
                newPos = (startPos + die1 + die2 + die3 - 1) % 10 + 1
                if state.lastPlayer == 2:
                    result.append(State(newPos, state.p2Pos, state.p1Score + newPos, state.p2Score, 1))
                else:
                    result.append(State(state.p1Pos, newPos, state.p1Score, state.p2Score + newPos, 2))
    return result

winsPlayerOne = 0
winsPlayerTwo = 0
pq = PriorityQueue()
pq.put((0, State(inputP1, inputP2, 0, 0, 2)))
games[State(inputP1, inputP2, 0, 0, 2)] = 1
while not pq.empty():
    (_, currentState) = pq.get()
    currentCount = games.pop(currentState)

    res = playRound(currentState)
    for newState in res:
        if newState.p1Score >= 21:
            winsPlayerOne += currentCount
        elif newState.p2Score >= 21:
            winsPlayerTwo += currentCount
        else:
            if newState not in games:
                pq.put((min(newState.p1Score, newState.p2Score), newState))
            games[newState] += currentCount

print(winsPlayerOne)
print(winsPlayerTwo)
