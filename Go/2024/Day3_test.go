package AoC2024

import (
	"bufio"
	"log"
	"os"
	"regexp"
	"strconv"
	"strings"
	"testing"
)

func Day3(filePath string) (int, int) {
	file, err := os.Open(filePath)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)
	count, partTwo := extractNumbers(scanner)

	return count, partTwo
}

func extractNumbers(scanner *bufio.Scanner) (int, int) {
	re := regexp.MustCompile(`mul\((?P<digits>\d{1,3},\d{1,3})\)|don't\(\)|do\(\)`)

	count := 0
	disabledCount := 0
	enabled := true
	for scanner.Scan() {
		str := scanner.Text()

		matches := re.FindAllStringSubmatch(str, -1)
		digitIndex := re.SubexpIndex("digits")
		for _, match := range matches {
			if match[0] == "do()" {
				enabled = true
			} else if match[0] == "don't()" {
				enabled = false
			} else {
				digits := match[digitIndex]
				d := strings.Split(digits, ",")

				l, _ := strconv.Atoi(d[0])
				r, _ := strconv.Atoi(d[1])
				count += r * l
				if !enabled {
					disabledCount += r * l
				}
			}
		}

	}
	return count, count - disabledCount
}

func TestDay3Example(t *testing.T) {
	one, two := Day3("./Files/day3ex.txt")
	if one != 161 {
		t.Errorf("PartOne = %d; want 161", one)
	}
	if two != 48 {
		t.Errorf("PartTwo = %d; want 48", two)
	}
}

func TestDay3(t *testing.T) {
	one, two := Day3("./Files/day3.txt")
	if one != 165225049 {
		t.Errorf("PartOne = %d; want 165225049", one)
	}
	if two != 108830766 {
		t.Errorf("PartTwo = %d; want 108830766", two)
	}
}
