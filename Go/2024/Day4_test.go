package AoC2024

import (
	"bufio"
	"log"
	"os"
	"testing"
)

const (
	X = rune('X')
	M = rune('M')
	A = rune('A')
	S = rune('S')
)

func Day4(filePath string) (int, int) {
	file, err := os.Open(filePath)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)
	runes := extractText(scanner)

	count := 0
	countPartTwo := 0
	rows := len(runes)
	columns := len(runes[0])
	for row := 0; row < rows; row++ {

		for column := 0; column < columns; column++ {
			if runes[row][column] == X {
				// Check in each direction
				if column+3 < columns {
					if runes[row][column+1] == M &&
						runes[row][column+2] == A &&
						runes[row][column+3] == S {
						count++
					}
				}
				if column-3 >= 0 {
					if runes[row][column-1] == M &&
						runes[row][column-2] == A &&
						runes[row][column-3] == S {
						count++
					}
				}
				if row+3 < rows {
					if runes[row+1][column] == M &&
						runes[row+2][column] == A &&
						runes[row+3][column] == S {
						count++
					}
				}
				if row-3 >= 0 {
					if runes[row-1][column] == M &&
						runes[row-2][column] == A &&
						runes[row-3][column] == S {
						count++
					}
				}
				if column+3 < columns && row+3 < rows {
					if runes[row+1][column+1] == M &&
						runes[row+2][column+2] == A &&
						runes[row+3][column+3] == S {
						count++
					}
				}
				if column+3 < columns && row-3 >= 0 {
					if runes[row-1][column+1] == M &&
						runes[row-2][column+2] == A &&
						runes[row-3][column+3] == S {
						count++
					}
				}
				if column-3 >= 0 && row-3 >= 0 {
					if runes[row-1][column-1] == M &&
						runes[row-2][column-2] == A &&
						runes[row-3][column-3] == S {
						count++
					}
				}
				if column-3 >= 0 && row+3 < rows {
					if runes[row+1][column-1] == M &&
						runes[row+2][column-2] == A &&
						runes[row+3][column-3] == S {
						count++
					}
				}
			}

			if runes[row][column] == A {
				if column+1 < columns && column-1 >= 0 && row+1 < rows && row-1 >= 0 {
					upLeftDownRight := runes[row-1][column-1] == M && runes[row+1][column+1] == S || runes[row-1][column-1] == S && runes[row+1][column+1] == M
					upRightDownLeft := runes[row-1][column+1] == M && runes[row+1][column-1] == S || runes[row-1][column+1] == S && runes[row+1][column-1] == M
					if upLeftDownRight && upRightDownLeft {
						countPartTwo++
					}
				}
			}
		}
	}

	return count, countPartTwo
}

func extractText(scanner *bufio.Scanner) [][]rune {
	rows := make([][]rune, 0)
	for scanner.Scan() {
		str := scanner.Text()

		rows = append(rows, []rune(str))
	}
	return rows
}

func TestDay4Example1(t *testing.T) {
	one, two := Day4("./Files/day4ex1.txt")
	if one != 4 {
		t.Errorf("PartOne = %d; want 4", one)
	}
	if two != 0 {
		t.Errorf("PartTwo = %d; want 0", two)
	}
}

func TestDay4Example2(t *testing.T) {
	one, two := Day4("./Files/day4ex2.txt")
	if one != 18 {
		t.Errorf("PartOne = %d; want 18", one)
	}
	if two != 9 {
		t.Errorf("PartTwo = %d; want 9", two)
	}
}

func TestDay4(t *testing.T) {
	one, two := Day4("./Files/day4.txt")
	if one != 2496 {
		t.Errorf("PartOne = %d; want 2496", one)
	}
	if two != 1967 {
		t.Errorf("PartTwo = %d; want 1967", two)
	}
}
