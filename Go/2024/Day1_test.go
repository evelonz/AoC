package AoC2024

import (
	"bufio"
	"log"
	"os"
	"regexp"
	"sort"
	"strconv"
	"testing"
)

func Day1(filePath string) (int, int) {
	file, err := os.Open(filePath)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)
	left, right := ExtractNumbers(scanner)

	// Sort lists
	sort.Slice(left, func(i, j int) bool {
		return left[i] < left[j]
	})
	sort.Slice(right, func(i, j int) bool {
		return right[i] < right[j]
	})

	sumPartOne, sumPartTwo := GetCounts(left, right)

	return sumPartOne, sumPartTwo
}

func ExtractNumbers(scanner *bufio.Scanner) ([]int, []int) {
	re := regexp.MustCompile(`(?P<Left>\d*)   (?P<Right>\d*)`)

	var left []int
	var right []int
	for scanner.Scan() {
		str := scanner.Text()

		matches := re.FindStringSubmatch(str)
		leftIndex := re.SubexpIndex("Left")
		rightIndex := re.SubexpIndex("Right")

		l, _ := strconv.Atoi(matches[leftIndex])
		left = append(left, l)
		r, _ := strconv.Atoi(matches[rightIndex])
		right = append(right, r)
	}
	return left, right
}

func GetCounts(left []int, right []int) (int, int) {
	sumPartOne := 0
	sumPartTwo := 0
	for index, v := range left {
		sumPartOne = CountPartOne(v, right, index, sumPartOne)
		sumPartTwo = CountPartTwo(v, right, sumPartTwo)
	}

	return sumPartOne, sumPartTwo
}

func CountPartOne(v int, right []int, index int, sumPartOne int) int {
	tmp := v - right[index]
	if tmp < 0 {
		tmp = -tmp
	}
	sumPartOne += tmp
	return sumPartOne
}

func CountPartTwo(v int, right []int, sumPartTwo int) int {
	count := 0
	// Omit memoization, as it makes little difference in performance.
	for _, s := range right {
		if s == v {
			count++
		}
	}
	sumPartTwo += v * count

	return sumPartTwo
}

func TestDay1Example(t *testing.T) {
	one, two := Day1("./Files/day1ex.txt")
	if one != 11 {
		t.Errorf("PartOne = %d; want 11", one)
	}
	if two != 31 {
		t.Errorf("PartTwo = %d; want 31", two)
	}
}

func TestDay1(t *testing.T) {
	one, two := Day1("./Files/day1.txt")
	if one != 1189304 {
		t.Errorf("PartOne = %d; want 1189304", one)
	}
	if two != 24349736 {
		t.Errorf("PartTwo = %d; want 24349736", two)
	}
}
