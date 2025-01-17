package AoC2024

import (
	"bufio"
	"image"
	"image/color"
	"image/png"
	"log"
	"os"
	"regexp"
	"strconv"
	"testing"
)

func Day14(filePath string, width int, height int, seconds int) (int, int) {
	file, err := os.Open(filePath)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)
	robots := extractDay14Map(scanner)
	endPositions := make([]Coordinate, 0)

	for i := 0; i < seconds; i++ {
		newRobots := make([]day14Robot, 0)
		for _, robot := range robots {
			pos := robot.position
			px := (pos.column + robot.velocity.column) % width
			py := (pos.row + robot.velocity.row) % height
			if px < 0 {
				px = width + px
			}
			if py < 0 {
				py = height + py
			}
			pos = Coordinate{px, py}
			newRobots = append(newRobots, day14Robot{pos, robot.velocity})
		}

		robots = newRobots
		// Pattern found at 19 and 74, then at 122 and 175.
		// If repeated, they will intersect at 8053.
		if i > 8050 && i < 8060 {
			day14Image(robots, width, height, i)
		}

		if i == 99 {
			for _, robot := range robots {
				endPositions = append(endPositions, robot.position)
			}
		}
	}

	if len(endPositions) == 0 {
		for _, robot := range robots {
			endPositions = append(endPositions, robot.position)
		}
	}

	// Count quadrants
	tl := 0
	tr := 0
	bl := 0
	br := 0
	halfWidth := width / 2
	halfHeight := height / 2
	for _, endPos := range endPositions {
		if endPos.column < halfWidth && endPos.row < halfHeight {
			tl++
		} else if endPos.column > halfWidth && endPos.row < halfHeight {
			tr++
		} else if endPos.column < halfWidth && endPos.row > halfHeight {
			bl++
		} else if endPos.column > halfWidth && endPos.row > halfHeight {
			br++
		}
	}
	count := tl * tr * bl * br
	countPartTwo := 0

	return count, countPartTwo
}

type day14Robot struct {
	position Coordinate
	velocity Coordinate
}

func extractDay14Map(scanner *bufio.Scanner) []day14Robot {
	re := regexp.MustCompile(`p=(?P<posx>-?\d*),(?P<posy>-?\d*) v=(?P<velx>-?\d*),(?P<vely>-?\d*)`)

	robots := make([]day14Robot, 0)
	for scanner.Scan() {
		str := scanner.Text()

		matches := re.FindStringSubmatch(str)
		posxIndex := re.SubexpIndex("posx")
		posyIndex := re.SubexpIndex("posy")
		velxIndex := re.SubexpIndex("velx")
		velyIndex := re.SubexpIndex("vely")

		posx, _ := strconv.Atoi(matches[posxIndex])
		posy, _ := strconv.Atoi(matches[posyIndex])
		velx, _ := strconv.Atoi(matches[velxIndex])
		vely, _ := strconv.Atoi(matches[velyIndex])
		robots = append(robots, day14Robot{Coordinate{posx, posy}, Coordinate{velx, vely}})
	}

	return robots
}

func day14Image(robots []day14Robot, width int, height int, index int) {

	upLeft := image.Point{0, 0}
	lowRight := image.Point{width, height}

	img := image.NewRGBA(image.Rectangle{upLeft, lowRight})

	// Colors are defined by Red, Green, Blue, Alpha uint8 values.
	green := color.RGBA{200, 20, 40, 0xff}

	// Set color for each pixel.
	for x := 0; x < width; x++ {
		for y := 0; y < height; y++ {
			img.Set(x, y, color.White)
		}
	}
	for _, robot := range robots {
		img.Set(robot.position.column, robot.position.row, green)
	}

	// Encode as PNG.
	f, _ := os.Create("./day14/image" + strconv.Itoa(index) + ".png")
	png.Encode(f, img)
}

func TestDay14Example1(t *testing.T) {
	one, two := Day14("./Files/day14ex.txt", 11, 7, 100)
	if one != 12 {
		t.Errorf("PartOne = %d; want 12", one)
	}
	if two != 0 {
		t.Errorf("PartTwo = %d; want 0", two)
	}
}

// Part Two.
func TestDay14(t *testing.T) {
	one, two := Day14("./Files/day14.txt", 101, 103, 8060)
	if one != 219150360 {
		t.Errorf("PartOne = %d; want 219150360", one)
	}
	// Did part two visually here...
	if two != 0 {
		t.Errorf("PartTwo = %d; want 0", two)
	}
}
