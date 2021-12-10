import fileinput
from typing import List

mostCommon = []
binaryInput = [x.rstrip() for x in fileinput.input(encoding="utf-16")]
mostCommon = [0] * len(binaryInput[0])
for line in binaryInput:
    for index, bit in enumerate(line):
        if bit == '1':
            mostCommon[index] += 1
        else:
            mostCommon[index] -= 1
gamma = ""
epsilon = ""
oxygen = binaryInput.copy()
co2scrubber = binaryInput.copy()
for index, x in enumerate(mostCommon):
    if len(oxygen) == 2:
        print(x)
    if x >= 0:
        gamma += "1"
        epsilon += "0"
        # Cannot be done here, filter is based on the oxygen/scrubber list themselves...
        if len(oxygen) > 1:
            print(oxygen)
            oxygen = list(filter(lambda x: x[index] == "1", oxygen))
        if len(co2scrubber) > 1:
            co2scrubber = list(filter(lambda x: x[index] == "0", co2scrubber))
    else:
        gamma += "0"
        epsilon += "1"
        if len(oxygen) > 1:
            print(oxygen)
            oxygen = list(filter(lambda x: x[index] == "0", oxygen))
        if len(co2scrubber) > 1:
            co2scrubber = list(filter(lambda x: x[index] == "1", co2scrubber))

powerConsumption = int(gamma, 2) * int(epsilon, 2)
print(powerConsumption)
print(oxygen)
print(co2scrubber[0])
print(int(oxygen[0], 2))
print(int(co2scrubber[0], 2))
lifeSupport = int(oxygen[0], 2) * int(co2scrubber[0], 2)
print(lifeSupport)