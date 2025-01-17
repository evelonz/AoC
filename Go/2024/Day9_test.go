package AoC2024

import (
	"bufio"
	"log"
	"os"
	"testing"
)

func Day9(filePath string) (int, int) {
	file2, err := os.Open(filePath)
	if err != nil {
		log.Fatal(err)
	}
	defer file2.Close()

	scanner := bufio.NewScanner(file2)
	files, spaces, _ := extractDay9Map(scanner)

	count := solvePartOne(files, spaces)

	// Part two is pretty much the same, just move entire files and check until
	// there are no spaces with a lower index left?
	countPartTwo := 0

	return count, countPartTwo
}

func solvePartOne(files []day9File, spaces []day9File) int {
	head := len(files) - 1
	tail := 0
	tailFile := 0
	compressed := make([]day9File, 0)
	compressed = append(compressed, files[tailFile])
	tailFile++
	var file = day9File{}
	var space = day9File{}
	for head > tail {
		if file == (day9File{}) {
			file = files[head]
		}
		if space == (day9File{}) {
			space = spaces[tail]
		}
		if file.length == space.length {
			compressed = append(compressed, file)
			tail++
			space = day9File{}
			head--
			file = day9File{}
			if head > tailFile {
				compressed = append(compressed, files[tailFile])
				tailFile++
			}
		} else if file.length > space.length {
			compressed = append(compressed, day9File{file.id, space.length})
			file.length -= space.length
			tail++
			space = day9File{}
			if head > tailFile {
				compressed = append(compressed, files[tailFile])
				tailFile++
			}
		} else { // file.length < space.length
			compressed = append(compressed, day9File{file.id, file.length})
			space.length -= file.length
			head--
			file = day9File{}
		}
	}

	// Add any remainder
	if file.length > 0 {
		compressed = append(compressed, file)
	}

	count := 0
	index := 0
	for _, file := range compressed {
		for i := 0; i < file.length; i++ {
			count += file.id * (index + i)
		}
		// 0 1 2 3 4
		// index = 5
		// 5 6 7 8 9
		index += file.length
	}
	return count
}

func solvePartTwo(files []day9File, spaces []day9File) int {
	head := len(files) - 1
	tail := 0
	tailFile := 0
	compressed := make([]day9File, 0)
	compressed = append(compressed, files[tailFile])
	tailFile++
	var file = day9File{}
	var space = day9File{}
	for head > tail {
		if file == (day9File{}) {
			file = files[head]
		}
		if space == (day9File{}) {
			space = spaces[tail]
		}
		if file.length == space.length {
			compressed = append(compressed, file)
			tail++
			space = day9File{}
			head--
			file = day9File{}
			if head > tailFile {
				compressed = append(compressed, files[tailFile])
				tailFile++
			}
		} else if file.length > space.length {
			compressed = append(compressed, day9File{file.id, space.length})
			file.length -= space.length
			tail++
			space = day9File{}
			if head > tailFile {
				compressed = append(compressed, files[tailFile])
				tailFile++
			}
		} else { // file.length < space.length
			compressed = append(compressed, day9File{file.id, file.length})
			space.length -= file.length
			head--
			file = day9File{}
		}
	}

	// Add any remainder
	if file.length > 0 {
		compressed = append(compressed, file)
	}

	count := 0
	index := 0
	for _, file := range compressed {
		for i := 0; i < file.length; i++ {
			count += file.id * (index + i)
		}
		// 0 1 2 3 4
		// index = 5
		// 5 6 7 8 9
		index += file.length
	}
	return count
}

type day9File struct {
	id     int
	length int
}

func extractDay9Map(scanner *bufio.Scanner) ([]day9File, []day9File, int) {
	files := make([]day9File, 0)
	spaces := make([]day9File, 0)
	totalSpace := 0
	for scanner.Scan() {
		str := scanner.Text()
		fileID := 0
		file := true
		for _, v := range str {
			digit := int(v - '0')
			if file {
				files = append(files, day9File{fileID, digit})
				fileID++
			} else {
				spaces = append(spaces, day9File{-1, digit})
			}
			file = !file
			totalSpace += digit
		}
	}
	return files, spaces, totalSpace
}

func TestDay9Example1(t *testing.T) {
	one, two := Day9("./Files/day9ex.txt")
	if one != 1928 {
		t.Errorf("PartOne = %d; want 1928", one)
	}
	if two != 0 {
		t.Errorf("PartTwo = %d; want 0", two)
	}
}

// Part Two.
func TestDay9(t *testing.T) {
	one, two := Day9("./Files/day9.txt")
	if one != 6370402949053 {
		t.Errorf("PartOne = %d; want 6370402949053", one)
	}
	if two != 0 {
		t.Errorf("PartTwo = %d; want 0", two)
	}
}
