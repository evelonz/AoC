#if COMPILED
module Year2023.Day10
#endif
open Xunit
open FsUnit.Xunit
open System.Text.RegularExpressions

let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input10.txt""")

let exampleData1 = seq {
        yield "....."
        yield ".S-7."
        yield ".|.|."
        yield ".L-J."
        yield "....."
    }

let parse input =
    input
    |> Seq.map (fun x -> x |> Seq.toArray)
    |> Seq.toArray

let t = parse exampleData1

let findStartSingleIndex matrix =
    matrix
    |> Array.concat
    |> Array.indexed
    |> Array.filter (fun x -> snd x = 'S')

let findStartIndex (matrix: char array array) =
    let len = matrix[0].Length
    let singelIndex = findStartSingleIndex matrix
    ((fst singelIndex[0])/len, (fst singelIndex[0])%len)


findStartIndex t

type Direction =
    | North
    | East
    | South
    | West
    | Nothing

//type Pipe =
//    | Vertical
//    | Horizontal
//    | NorthEast
//    | NorthWest
//    | SouthWest
//    | SouthEast
//    | None
//    | Start

let getPipeDirection char =
    match char with 
    | '|' -> North, South
    | '-' -> East, West
    | 'L' -> North, East
    | 'J' -> North, West
    | '7' -> South, West
    | 'F' -> East, South
    | '.' -> Nothing, Nothing
    | 'S' -> Nothing, Nothing
    | _ -> failwith "Invalid pipe character"

let isLinked targetChars startChar =
    targetChars |> List.contains startChar

let isLinkedNorth = isLinked ['|'; '7'; 'F']
let isLinkedSouth = isLinked ['|'; 'J'; 'L']
let isLinkedEast = isLinked ['-'; 'J'; '7']
let isLinkedWest = isLinked ['-'; 'L'; 'F']

let getStartChar up down right left =
    let connections = (isLinkedNorth up, isLinkedSouth down, isLinkedWest left, isLinkedEast right)
    printfn "%A" connections
    match connections with
    | (true, true, false, false) -> '|'
    | (false, false, true, true) -> '-'
    | (true, false, true, false) -> 'J'
    | (true, false, false, true) -> 'L'
    | (false, true, true, false) -> '7'
    | (false, true, false, true) -> 'F'
    | _ -> failwith "unmatched starting point"

getStartChar '|' '|' '.' '.' 
getStartChar '|' '-' '|' 'L'
getStartChar '-' '|' '-' '|'
isLinkedWest 'L'
let reducedPipeDirection enterDirection pipe =
    let first, second = getPipeDirection pipe
    if first = enterDirection then
        second
    else 
        first
    //match enterDirection with
    //| first when first = enterDirection -> second
    //| second when second = enterDirection -> first
    //| _ -> failwithf "direction does not match, got %A, enter %A" (first, second) enterDirection

reducedPipeDirection South '|'

let startIndexAndPipe matrix =
    let (row, col) = findStartIndex matrix
    //printfn "%A" (row, col)
    let up = matrix[row-1][col]
    let down = matrix[row+1][col]
    let left = matrix[row][col-1]
    let right = matrix[row][col+1]
    //printfn "%A, %A, %A, %A" up down right left
    let start = getStartChar up down right left
    start, row, col
    


let traverse (matrix: char array array) fromDirection row col =
    let next = matrix[row][col]
    let nextDirection = reducedPipeDirection fromDirection next
    match nextDirection with
    | North -> South, row-1, col
    | East -> West, row, col+1
    | South -> North, row+1, col
    | West -> East, row, col-1
    | Nothing -> failwithf "Invalid reduced direction: %A, %A" fromDirection next

let traverseStart startchar row col =
    let nextDirection = getPipeDirection startchar
    match nextDirection with
    | North, South -> (South, row-1, col), (North, row+1, col)
    | East, West -> (West, row, col+1), (East, row, col-1)
    | North, East -> (South, row-1, col), (West, row, col+1)
    | North, West -> (South, row-1, col), (East, row, col-1)
    | South, West -> (North, row+1, col), (East, row, col-1)
    | East, South -> (West, row, col+1), (North, row+1, col)
    | _ -> failwithf "Unable to find start directions %A, %A, %A" startchar row col

let solve input =
    let t = parse input
    let startChar, rows, cols = startIndexAndPipe t
    let start = traverseStart startChar rows cols

    let rec traversAux (matrix: char array array) index (fromDirection1, row1, col1) (fromDirection2, row2, col2) =
        //printfn "%A, %A, %A" index (fromDirection1, row1, col1) (fromDirection2, row2, col2)
        if row1 = row2 && col1 = col2 then
            index
        else 
            let next1 = traverse matrix fromDirection1 row1 col1
            let next2 = traverse matrix fromDirection2 row2 col2
            traversAux matrix (index+1) next1 next2

    traversAux t 1 (fst start) (snd start)

reducedPipeDirection West '-'
traverse t West 1 2
traverse t North 2 1
solve exampleData1

let tt = parse input
let startChar, rows, cols = startIndexAndPipe tt
solve input

// Part two
(140*140) - (6806*2)
// Binary search should be 13 steps.
5988/2 // guess for 2k? 2k high.
// https://en.wikipedia.org/wiki/Point_in_polygon
// Just map all points in the loop.
// For each point, traverse to edge of map.
// Count intersections with loop
// If even: outside, if odd: inside.


//let solve2 input =
//    let t = parse input
//    let startChar, rows, cols = startIndexAndPipe t
//    let start = traverseStart startChar rows cols
//    let rowTrackOfPath: int list array = Array.create 150 [] // Assume symetrical
//    rowTrackOfPath[rows] <- cols::rowTrackOfPath[rows]

//    let rec traversAux (matrix: char array array) index (fromDirection1, row1, col1) (fromDirection2, row2, col2) =
//        printfn "%A, %A, %A" index (fromDirection1, row1, col1) (fromDirection2, row2, col2)
//        if row1 = row2 && col1 = col2 then
//            rowTrackOfPath[row1] <- col1::rowTrackOfPath[row1]
//            index, rowTrackOfPath
//        else 
//            let next1 = traverse matrix fromDirection1 row1 col1
//            rowTrackOfPath[row1] <- col1::rowTrackOfPath[row1]
//            let next2 = traverse matrix fromDirection2 row2 col2
//            rowTrackOfPath[row2] <- col2::rowTrackOfPath[row2]
//            traversAux matrix (index+1) next1 next2

//    traversAux t 1 (fst start) (snd start)

//let exampleData2 = seq {
//        yield "..........."
//        yield ".S-------7." // Tricky, will the odd/even count work here? Would count as 9, butwe really only "cross" the loop once. Or even 0, sinec we end up outside again.
//        yield ".|F-----7|." // If we only check left or right, then only counting those with edges east and west should work?
//        yield ".||.....||." // So F 7 J L | would count as an intersection. But - would not.
//        yield ".||.....||."
//        yield ".|L-7.F-J|." // This dot would still count as inside, so we are still missing one check.
//        yield ".|..|.|..|." // So we need to count matching direction? L matches J, 7 matches F, | matches any?
//        yield ".L--J.L--J."
//        yield "..........."
//    }
////    F-7
//// F--J | // This would still incorrectly count as outside.
//// L----J

//let ttt = parse exampleData2
//let startChar3, rows3, cols3 = startIndexAndPipe ttt
//let start = traverseStart startChar3 rows3 cols3

//solve2 exampleData2

let [<Fact>] ``part one examples``() =
  solve exampleData1 |> should equal 4

let [<Fact>] ``part one solution``() =
  solve input |> should equal 6806
