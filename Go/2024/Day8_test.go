package AoC2024

import (
	"bufio"
	"log"
	"os"
	"testing"
)

func Day8(filePath string) (int, int) {
	file, err := os.Open(filePath)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)
	maps, rowMax, ColMax := extractDay8Map(scanner)

	setPartOne := make(map[Coordinate]bool)
	setPartTwo := make(map[Coordinate]bool)
	for i := 0; i < len(maps)-1; i++ {
		base := maps[i]
		for j := i + 1; j < len(maps); j++ {
			nxt := maps[j]
			if base.char != nxt.char {
				continue
			}

			baseRow := base.position.row
			nxtRow := nxt.position.row
			rowDelta := baseRow - nxtRow
			if rowDelta < 0 {
				rowDelta = -rowDelta
			}
			baseColumn := base.position.column
			nxtColumn := nxt.position.column
			colDelta := baseColumn - nxtColumn
			if colDelta < 0 {
				colDelta = -colDelta
			}

			col1 := -1
			col2 := -1
			row1 := -1
			row2 := -1
			// 1:
			// 1...........
			// ..b.........
			// .....n......
			// ........2...

			// 2:
			// .........2..
			// ......n.....
			// ...b........
			// .1..........

			// 3:
			// 2...........
			// ..n.........
			// .....b.......
			// ........1...

			// 4:
			// .........1..
			// ......b.....
			// ...n........
			// .2..........
			col1d := -1
			col2d := -1
			row1d := -1
			row2d := -1
			if baseColumn <= nxtColumn && baseRow <= nxtRow {
				col1 = baseColumn - colDelta
				col2 = baseColumn + 2*colDelta
				row1 = baseRow - rowDelta
				row2 = baseRow + 2*rowDelta
				col1d = -colDelta
				col2d = colDelta
				row1d = -rowDelta
				row2d = rowDelta
			} else if baseColumn <= nxtColumn && baseRow >= nxtRow {
				col1 = baseColumn - colDelta
				col2 = baseColumn + 2*colDelta
				row1 = baseRow + rowDelta
				row2 = baseRow - 2*rowDelta
				col1d = -colDelta
				col2d = colDelta
				row1d = rowDelta
				row2d = -rowDelta
			} else if baseColumn >= nxtColumn && baseRow >= nxtRow {
				col1 = baseColumn + colDelta
				col2 = baseColumn - 2*colDelta
				row1 = baseRow + rowDelta
				row2 = baseRow - 2*rowDelta
				col1d = colDelta
				col2d = -colDelta
				row1d = rowDelta
				row2d = -rowDelta
			} else { // baseColumn > nxtColumn && baseRow < nxtRow
				col1 = baseColumn + colDelta
				col2 = baseColumn - 2*colDelta
				row1 = baseRow - rowDelta
				row2 = baseRow + 2*rowDelta
				col1d = colDelta
				col2d = -colDelta
				row1d = -rowDelta
				row2d = rowDelta
			}

			// Part 2, take delta and add/remove it while in limits.
			// including towers themselves.

			if row1 >= 0 && row1 < rowMax &&
				col1 >= 0 && col1 < ColMax {
				setPartOne[Coordinate{col1, row1}] = true
			}
			if row2 >= 0 && row2 < rowMax &&
				col2 >= 0 && col2 < ColMax {
				setPartOne[Coordinate{col2, row2}] = true
			}

			currentPos := base.position
			for currentPos.row >= 0 && currentPos.row < rowMax &&
				currentPos.column >= 0 && currentPos.column < ColMax {
				setPartTwo[currentPos] = true
				currentPos = Coordinate{currentPos.column + col1d, currentPos.row + row1d}
			}
			currentPos = base.position
			for currentPos.row >= 0 && currentPos.row < rowMax &&
				currentPos.column >= 0 && currentPos.column < ColMax {
				setPartTwo[currentPos] = true
				currentPos = Coordinate{currentPos.column + col2d, currentPos.row + row2d}
			}
		}
	}

	// count := 0
	// countPartTwo := 0

	return len(setPartOne), len(setPartTwo)
}

type day8antennas struct {
	char     rune
	position Coordinate
}

func extractDay8Map(scanner *bufio.Scanner) ([]day8antennas, int, int) {
	maps := make([]day8antennas, 0)
	rowCount := 0
	colCount := 0
	for scanner.Scan() {
		str := scanner.Text()
		strRune := []rune(str)
		for i, v := range strRune {
			if v != '.' {
				maps = append(maps, day8antennas{v, Coordinate{i, rowCount}})
			}
			colCount = i
		}
		rowCount++
	}
	return maps, rowCount, colCount + 1
}

func TestDay8Example1(t *testing.T) {
	one, two := Day8("./Files/day8ex.txt")
	if one != 14 {
		t.Errorf("PartOne = %d; want 14", one)
	}
	if two != 34 {
		t.Errorf("PartTwo = %d; want 34", two)
	}
}

// Part Two.
func TestDay8(t *testing.T) {
	one, two := Day8("./Files/day8.txt")
	if one != 364 {
		t.Errorf("PartOne = %d; want 364", one)
	}
	if two != 1231 {
		t.Errorf("PartTwo = %d; want 1231", two)
	}
}
