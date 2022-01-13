
class Stack:
    def __init__(self, acccepts, start):
        self.accepted = acccepts
        self.stack = start

    def __str__(self):
        return str(self.stack)

    def pop(self):
        if len(self.stack) == 0:
            return None, None
        if all(p == self.accepted for p in self.stack):
            return None, None
        newList = self.stack.copy()
        char = newList.pop()
        return char, Stack(self.accepted, newList)

a = Stack('A', ['A', 'B'])
print(a)
(char, newStack) = a.pop()
print(char, newStack)

#############
#...........#
###B#C#B#D###
  #A#D#C#A#
  #########
halway = [''] * 11
aStack = Stack('A', ['A', 'B'])
bStack = Stack('B', ['D', 'C'])
cStack = Stack('C', ['C', 'B'])
dStack = Stack('D', ['A', 'D'])
print(halway)