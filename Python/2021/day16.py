import math
import unittest
import sys

#input = [x.rstrip() for x in fileinput.input(encoding="utf-16")]
input = [x.rstrip() for x in open(sys.argv[1])] if sys.argv[0].endswith('day16.py') and len(sys.argv) > 1 else []

def parseLiteral(binStr):
    #print("----------------------")
    #print("To literal: ", binStr)
    notEnd = True
    index = 0
    literalValue = ""
    while notEnd:
        notEnd = int(binStr[index:index+1]) == 1
        literalValue += binStr[index+1:index+5]
        index += 5
    #print("parsed: ", literalValue)
    converted = int(literalValue, 2)
    #print("converted: ", converted)
    return (index, converted)

def calculateValue(values, typeId):
    value = 0
    match typeId:
        case 0: # sum
            value = sum(values)
        case 1: # product
            value = math.prod(values)
        case 2: # minimum
            value = min(values)
        case 3: # maximum
            value = max(values)
        case 5: # greater than
            if values[0] > values[1]:
                value = 1
            else:
                value = 0
        case 6: # less than
            if values[0] < values[1]:
                value = 1
            else:
                value = 0
        case 7: # equal to
            if values[0] == values[1]:
                value = 1
            else:
                value = 0
        case _:
            raise ValueError("invalid operator type ID: " + typeId)
    return value

def parseOperator(binStr, typeId):
    #print("----------------------")
    #print("To operator: ", binStr)
    #print(binStr)
    lengthTypeId = int(binStr[0:1], 2)
    #print("lengthType: ", lengthTypeId)
    index = 0
    versionSumTotal = 0
    values = []
    if lengthTypeId == 0: # Bit length
        length = int(binStr[1:16], 2)
        index = 16
        offset = 0
        while (length-offset > 6 and int(binStr[index:index+6], 2) > 0) or (length-offset > 22 and int(binStr[index:index+22], 2) > 0): # check that first 6 > 0 (should always be true I think?)
            (indexOffset, versionSum, value) = parse(binStr[index:])
            index += indexOffset
            offset += indexOffset
            versionSumTotal += versionSum
            values.append(value)
    else: # package count
        length = int(binStr[1:12], 2)
        index = 12
        for _ in range(length):
            (indexOffset, versionSum, value) = parse(binStr[index:])
            index += indexOffset
            versionSumTotal += versionSum
            values.append(value)

    value = calculateValue(values, typeId)
    return (index, versionSumTotal, value)

def parse(binStr):
    #print("----------------------")
    #print("Parse: ", binStr)
    version = int(binStr[:3], 2)
    #print("Version: ", version)
    typeId = int(binStr[3:6], 2)
    if(typeId == 4):
        (indexOffset, value) = parseLiteral(binStr[6:])
        return (indexOffset+6, version, value)
    else:
        (indexOffset, versionSum, value) = parseOperator(binStr[6:], typeId)
        return (indexOffset+6, versionSum+version, value)

for hex in input:
    binary = bin(int(hex, 16))[2:].zfill(len(hex)*4)
    #print(binary)
    (_, versionSum, value) = parse(str(binary))
    print("Answer1: ", versionSum)
    print("Answer2: ", value)

class TestDay16(unittest.TestCase):
    def test_literal_value(self):
        hex = "D2FE28"
        binary = bin(int(hex, 16))[2:].zfill(len(hex)*4)
        (_, versionSum, value) = parse(str(binary))
        self.assertEqual(value, 2021)

    def test_operator_fixed_length(self):
        hex = "38006F45291200"
        binary = bin(int(hex, 16))[2:].zfill(len(hex)*4)
        (_, versionSum, value) = parse(str(binary))
        self.assertEqual(value, 1)
        self.assertEqual(versionSum, 9)

    def test_operator_count_length(self):
        hex = "EE00D40C823060"
        binary = bin(int(hex, 16))[2:].zfill(len(hex)*4)
        (_, versionSum, value) = parse(str(binary))
        self.assertEqual(value, 3)
        self.assertEqual(versionSum, 14)

    def test_part_one_examples(self):
        hexes = [
            ("8A004A801A8002F478", 16), 
            ("620080001611562C8802118E34", 12),
            ("C0015000016115A2E0802F182340", 23),
            ("A0016C880162017C3686B18A3D4780", 31)
        ]
        for (hex, expected) in hexes:
            with self.subTest(hex = hex):
                binary = bin(int(hex, 16))[2:].zfill(len(hex)*4)
                (_, versionSum, _) = parse(str(binary))
                self.assertEqual(versionSum, expected)

    def test_part_two_examples(self):
        hexes = [
            ("C200B40A82", 3), 
            ("04005AC33890", 54),
            ("880086C3E88112", 7),
            ("CE00C43D881120", 9),
            ("D8005AC2A8F0", 1),
            ("F600BC2D8F", 0),
            ("9C005AC2F8F0", 0),
            ("9C0141080250320F1802104A08", 1),
        ]
        for (hex, expected) in hexes:
            with self.subTest(hex = hex):
                binary = bin(int(hex, 16))[2:].zfill(len(hex)*4)
                (_, _, value) = parse(str(binary))
                self.assertEqual(value, expected)

if __name__ == '__main__':
    if len(sys.argv) != 2:
        print(len(sys.argv))
        print(sys.argv)
        unittest.main()
