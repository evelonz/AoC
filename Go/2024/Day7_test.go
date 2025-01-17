package AoC2024

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strconv"
	"strings"
	"testing"
)

func Day7(filePath string) (int, int) {
	file, err := os.Open(filePath)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)
	rows := extractDay7Map(scanner)

	count := 0
	// maxSum := 0
	countPartTwo := 0
	for i, val := range rows {
		valid := calculatePossibility(val)
		if valid {
			count += val.target
		}
		// maxSum += val.target
		fmt.Printf("iteration: %d, result, %t \n", i, valid)
	}
	// fmt.Printf("maxSum: %d\n", maxSum)

	return count, countPartTwo
}

type day7Struct struct {
	target int
	digits []int
}

func calculatePossibility(input day7Struct) bool {
	root := 0
	for _, val := range input.digits {
		root += val
	}
	if root == input.target {
		return true
	}

	variations := []int{0}
	for _, val := range input.digits {
		rightSide := root - val
		variationsTmp := make([]int, len(variations)+2)
		for _, leftSide := range variations {
			add := leftSide + val
			multi := leftSide * val
			concat, _ := strconv.Atoi(strconv.Itoa(leftSide) + strconv.Itoa(val))
			if add+rightSide == input.target || multi+rightSide == input.target || concat+rightSide == input.target {
				return true
			}
			// if add+rightSide < input.target {
			variationsTmp = append(variationsTmp, add)
			// }
			// if multi+rightSide < input.target {
			variationsTmp = append(variationsTmp, multi)
			// }
			// if concat+rightSide < input.target {
			variationsTmp = append(variationsTmp, concat)
			// }

			// if add+rightSide > input.target {
			// return false
			// }
		}
		root = rightSide
		variations = variationsTmp
	}
	return false
}

func extractDay7Map(scanner *bufio.Scanner) []day7Struct {
	rows := make([]day7Struct, 0)
	for scanner.Scan() {

		str := scanner.Text()
		tmp := strings.Split(str, ": ")
		target, _ := strconv.Atoi(tmp[0])
		tmp2 := strings.Split(tmp[1], " ")
		digits := make([]int, 0)

		for _, v := range tmp2 {
			digit, _ := strconv.Atoi(v)
			digits = append(digits, digit)
		}
		rows = append(rows, day7Struct{target, digits})
	}

	return rows
}

func TestDay7Example1(t *testing.T) {
	one, two := Day7("./Files/day7ex.txt")
	if one != 3749 {
		t.Errorf("PartOne = %d; want 3749", one)
	}
	if two != 0 {
		t.Errorf("PartTwo = %d; want 0", two)
	}
}

// Part Two.
// TODO: Part two takes more than 30 seconds and times out.
// Need to run in debug mode for it to work.
// Also, both answers are returned as part one, needs to be fixed.
func TestDay7(t *testing.T) {
	one, two := Day7("./Files/day7.txt")
	if one != 1611660863222 {
		t.Errorf("PartOne = %d; want 1611660863222", one)
	}
	if two != 945341732469724 {
		t.Errorf("PartTwo = %d; want 945341732469724", two)
	}
}
