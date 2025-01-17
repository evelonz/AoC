package AoC2024

import (
	"bufio"
	"log"
	"os"
	"strconv"
	"strings"
	"testing"
)

func Day2(filePath string) (int, int) {
	file, err := os.Open(filePath)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	count := 0
	countWithErrorFix := 0
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		str := scanner.Text()
		s := strings.Split(str, " ")
		integers := make([]int, 0, 5)
		for _, v := range s {
			i, _ := strconv.Atoi(v)
			integers = append(integers, i)
		}
		var check, _ = IsValidReport(integers, -1)
		if check {
			count++
		} else {
			for i := 0; i < len(integers); i++ {
				var newIntegers = deleteElement(integers, i)
				check, _ = IsValidReport(newIntegers, -1)
				if check {
					countWithErrorFix++
					break
				}
			}
		}
		// Not sure why the skip did not work. most likely if the first or last element was wrong.
		// if skip > -1 {
		// 	check, _ = IsValidReport(integers, skip)
		// 	if check {
		// 		countWithErrorFix++
		// 	}
		// }

	}

	return count, count + countWithErrorFix
}

func deleteElement(slice []int, index int) []int {
	tmp := make([]int, len(slice))
	copy(tmp, slice)
	var start = tmp[:index]
	var end = tmp[index+1:]
	return append(start, end...)
}

func TestDeleteElement(t *testing.T) {
	var a = []int{7, 6, 4, 2, 1}
	var b = deleteElement(a, len(a)-1)
	if b == nil {
		t.Errorf("Should be true")
	}

	a = []int{7, 6, 4, 2, 1}
	b = deleteElement(a, 0)
	if b == nil {
		t.Errorf("Should be true")
	}
}

const (
	Any int = iota
	Increasing
	Decreasing
)

func IsValidReport(input []int, skip int) (bool, int) {
	direction := Any
	for i := 0; i+2 <= len(input); i++ {
		if i == skip {
			continue
		}
		delta := input[i] - input[i+1]
		if i+1 == skip {
			delta = input[i] - input[i+2]
		}
		if delta == 0 {
			return false, i
		} else if delta > 0 && delta < 4 {
			if direction == Increasing {
				return false, i
			}
			direction = Decreasing
		} else if delta < 0 && delta > -4 {
			if direction == Decreasing {
				return false, i
			}
			direction = Increasing
		} else {
			return false, i
		}
	}

	return true, -1
}

func TestDay2Examples(t *testing.T) {
	var a = []int{7, 6, 4, 2, 1}
	var b, _ = IsValidReport(a, -1)
	if !b {
		t.Errorf("Should be true")
	}
}

func TestDay2Examples2(t *testing.T) {
	var a = []int{1, 2, 7, 8, 9}
	var b, c = IsValidReport(a, -1)
	if b {
		t.Errorf("Should be false")
	}
	if c != 1 {
		t.Errorf("Should return skip value 1, got %d", c)
	}
}

func TestDay2Ex(t *testing.T) {
	one, two := Day2("./Files/day2ex.txt")
	if one != 2 {
		t.Errorf("PartOne = %d; want 2", one)
	}
	if two != 4 {
		t.Errorf("PartTwo = %d; want -1", two)
	}
}

func TestDay2(t *testing.T) {
	one, two := Day2("./Files/day2.txt")
	if one != 321 {
		t.Errorf("PartOne = %d; want 321", one)
	}
	if two != 386 { // 355 too low.
		t.Errorf("PartTwo = %d; want 386", two)
	}
}
