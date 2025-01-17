package AoC2024

import (
	"bufio"
	"log"
	"os"
	"reflect"
	"strconv"
	"strings"
	"testing"
)

func Day5(filePath string) (int, int) {
	file, err := os.Open(filePath)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)
	orders, updates := extractDay5(scanner)
	if orders != nil && updates != nil {
	}
	count := 0
	countPartTwo := 0
	// rows := len(runes)
	// columns := len(runes[0])
	// for row := 0; row < rows; row++ {
	// 	for column := 0; column < columns; column++ {

	// 	}
	// }

	for _, pages := range updates {
		okUpdate := true
		for i, page := range pages {
			// We only care if a later page should have been before the current one.
			orderLookup := orders[page]
			// Should be i+1, but cannot be bothered with the extra bound check needed.
			for lookAhead := i; lookAhead < len(pages); lookAhead++ {
				nextPage := pages[lookAhead]
				_, ok := orderLookup[nextPage]
				okUpdate = !ok
				if !okUpdate {
					break // Invalid update
				}
			}
			if !okUpdate {
				break // Invalid update
			}
		}
		if okUpdate {
			// add middle to sum
			middle := len(pages) / 2
			count += pages[middle]
		} else {
			// For part two, swap the order of elements using the order rules, until valid.
			// Then sum the middle index of only those fixed updates.
			swapF := reflect.Swapper(pages)
			for !okUpdate {
				// Should extract the above code to a function, so I can re-use it.
				// But fuck it, it's Go, re-implement everything!
				okUpdate = true
				for i, page := range pages {
					// We only care if a later page should have been before the current one.
					orderLookup := orders[page]
					// Should be i+1, but cannot be bothered with the extra bound check needed.
					for lookAhead := i; lookAhead < len(pages); lookAhead++ {
						nextPage := pages[lookAhead]
						_, ok := orderLookup[nextPage]
						okUpdate = !ok
						if !okUpdate {
							swapF(i, lookAhead) // Swap and try again!
							break               // Invalid update
						}
					}
					if !okUpdate {
						break // Invalid update
					}
				}
			}
			middle := len(pages) / 2
			countPartTwo += pages[middle]
		}
	}
	return count, countPartTwo
}

type OrderDay5 struct {
	first  int
	second int
}

func filter[T any](ss []T, test func(T) bool) (ret []T) {
	for _, s := range ss {
		if test(s) {
			ret = append(ret, s)
		}
	}
	return
}

func extractDay5(scanner *bufio.Scanner) (map[int]map[int]bool, [][]int) {
	updates := make([][]int, 0)
	// orderRules := make([]OrderDay5, 0)
	orderDone := false
	orderRules := make(map[int]map[int]bool, 0)

	for scanner.Scan() {
		str := scanner.Text()
		if str == "" {
			orderDone = true
			continue
		}
		if !orderDone {
			split := strings.Split(str, "|")
			first, _ := strconv.Atoi(split[0])
			second, _ := strconv.Atoi(split[1])

			orderRule, ok := orderRules[second]
			if ok {
				orderRule[first] = true
			} else {
				s := map[int]bool{first: true}
				orderRules[second] = s
			}
		} else {
			split := strings.Split(str, ",")
			pages := make([]int, 0)
			for i := 0; i < len(split); i++ {
				d, _ := strconv.Atoi(split[i])
				pages = append(pages, d)
			}
			updates = append(updates, pages)
		}
	}
	return orderRules, updates
}

func TestDay5Example1(t *testing.T) {
	one, two := Day5("./Files/day5ex.txt")
	if one != 143 {
		t.Errorf("PartOne = %d; want 143", one)
	}
	if two != 123 {
		t.Errorf("PartTwo = %d; want 123", two)
	}
}

func TestDay5(t *testing.T) {
	one, two := Day5("./Files/day5.txt")
	if one != 5509 {
		t.Errorf("PartOne = %d; want 5509", one)
	}
	if two != 4407 {
		t.Errorf("PartTwo = %d; want 4407", two)
	}
}
