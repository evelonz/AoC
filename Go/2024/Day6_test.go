package AoC2024

import (
	"bufio"
	"log"
	"os"
	"testing"
)

func Day6(filePath string) (int, int) {
	file, err := os.Open(filePath)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)
	runes, column, row := extractGuardMap(scanner)

	// count := 0
	countPartTwo := 0
	// rows := len(runes)
	// columns := len(runes[0])
	visitSet := make(map[Coordinate][]rune, 0)
	visited := guardsTravels(runes, visitSet, Coordinate{column, row})

	return len(visited), countPartTwo
}

type Coordinate struct {
	column int
	row    int
}

func guardsTravels(maps [][]rune, visited map[Coordinate][]rune, position Coordinate) map[Coordinate][]rune {
	direction := maps[position.row][position.column]
	visited[position] = append(visited[position], direction)

	for position.column >= 0 && position.row >= 0 &&
		position.column < len(maps) && position.row < len(maps[0]) {

		nxt := nextPosition(position, direction)
		if !(nxt.column >= 0 && nxt.row >= 0 &&
			nxt.column < len(maps) && nxt.row < len(maps[0])) {
			position = nxt
			continue
		}

		nextPos := maps[nxt.row][nxt.column]
		if nextPos == '#' {
			direction = nextDirection(direction)
			continue
		} else {
			// Check if we can place an obstruction.
			// Cannot have traveled here before, but could have placed an obstruction here before.
			//newDirection := nextDirection(direction)

		}

		position = nxt
		visited[position] = append(visited[position], direction)

	}
	return visited
}

func nextPosition(position Coordinate, direction rune) Coordinate {
	if direction == '^' {
		return Coordinate{position.column, position.row - 1}
	} else if direction == '>' {
		return Coordinate{position.column + 1, position.row}
	} else if direction == 'v' {
		return Coordinate{position.column, position.row + 1}
	} else {
		return Coordinate{position.column - 1, position.row}
	}
}

func nextDirection(direction rune) rune {
	if direction == '^' {
		return '>'
	} else if direction == '>' {
		return 'v'
	} else if direction == 'v' {
		return '<'
	} else {
		return '^'
	}
}

func extractGuardMap(scanner *bufio.Scanner) ([][]rune, int, int) {
	rows := make([][]rune, 0)
	column := 0
	row := 0
	rowCount := 0
	for scanner.Scan() {
		str := scanner.Text()
		strRune := []rune(str)
		for i, v := range strRune {
			if v == '^' {
				column = i
				row = rowCount
			}
		}
		rows = append(rows, strRune)
		rowCount++
	}
	return rows, column, row
}

func TestDay6Example1(t *testing.T) {
	one, two := Day6("./Files/day6ex.txt")
	if one != 41 {
		t.Errorf("PartOne = %d; want 41", one)
	}
	if two != 0 {
		t.Errorf("PartTwo = %d; want 0", two)
	}
}

// Part Two.
// Can we assume that he will only loop if
// We are able to place the object so he turns into an already traveled path?
// If so, simply traverse the map, and if we can project a line to the right, which hits
// an already traversed path (in that direction!), then it will for sure loop.

func TestDay6(t *testing.T) {
	one, two := Day6("./Files/day6.txt")
	if one != 5101 {
		t.Errorf("PartOne = %d; want 5101", one)
	}
	if two != 0 {
		t.Errorf("PartTwo = %d; want 0", two)
	}
}
