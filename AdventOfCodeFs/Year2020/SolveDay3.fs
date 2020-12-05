#if COMPILED
module Year2020.Day3
#endif

open Xunit
open FsUnit.Xunit

// Shared
let exampleData = [
        "..##.......";
        "#...#...#..";
        ".#....#..#.";
        "..#.#...#.#";
        ".#...##..#.";
        "..#.##.....";
        ".#.#.#....#";
        ".#........#";
        "#.##...#...";
        "#...##....#";
        ".#..#...#.#";
    ]

// Part 1
let runSlope (row, col) (slope: List<string>) = seq { 
    let mutable curcol = col;
    let width = slope.[curcol].Length
    while curcol < slope.Length  do
        yield if slope.[curcol].[(curcol * row) % width] = '#' then 1 else 0
        curcol <- curcol+col }

let solution1 input = input |> Seq.toList |> runSlope (3, 1) |> Seq.sum

// Part 2
let slopes = [|(1, 1); (3, 1); (5, 1); (7, 1); (1, 2);|]
let runAllSlopes slopes map = seq { 
    for slope in slopes do
        yield runSlope slope map |> Seq.map uint |> Seq.sum }

let solution2 input = input |> Seq.toList |> runAllSlopes slopes |> Seq.fold (*) 1u

let [<Fact>] ``part one examples``() =
    let result = solution1 exampleData
    result |> should equal 7

let [<Fact>] ``part one solved``() =
    let data = Utility.loadPussleData __SOURCE_DIRECTORY__ "3"
    let result = solution1 data
    result |> should equal 262

let [<Fact>] ``part two examples``() =
    let data = Utility.loadPussleData __SOURCE_DIRECTORY__ "3"
    let result = solution2 data
    result |> should equal 2698900776u
