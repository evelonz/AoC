

# target area: x=88..125, y=-157..-103

# part one
TargetYMin = -157
maxStartVelocityY = abs(TargetYMin)-1
peakY = int((abs(TargetYMin)*(maxStartVelocityY+0)) / 2)
print(peakY)

# part two
TargetYMax = -103
TargetXMin = 88
TargetXMax = 125

maxStartVelocityX = TargetXMax
minStartVelocityX = -1
minStartVelocityY = TargetYMin
minPeakX = 0
while minPeakX < TargetXMin:
    minStartVelocityX += 1
    minPeakX = int((abs(TargetYMin)*(minStartVelocityX+0)) / 2)

def simulateLaunch(startX, startY):
    x = 0
    y = 0
    dx = startX
    dy = startY
    while x <= TargetXMax and y >= TargetYMin:
        x += dx
        if dx > 0:
            dx -= 1
        y += dy
        dy -= 1
        if x >= TargetXMin and x <= TargetXMax and y >= TargetYMin and y <= TargetYMax:
            return 1
    return 0

count = 0
for startX in range(minStartVelocityX, maxStartVelocityX+1):
    for startY in range(minStartVelocityY, maxStartVelocityY+1):
        count += simulateLaunch(startX, startY)
print(count)