import fileinput

horizontal = 0
depth = 0
aim = 0
depth2 = 0
for line in fileinput.input(encoding="utf-16"):
    data = line.split(' ')
    match data[0]:
        case 'forward':
            horizontal += int(data[1])
            depth2 += aim * int(data[1])
        case 'down':
            depth += int(data[1])
            aim += int(data[1])
        case 'up':      
            depth -= int(data[1])
            aim -= int(data[1])

print(horizontal * depth)
print(horizontal * depth2)