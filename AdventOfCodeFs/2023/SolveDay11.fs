#if COMPILED
module Year2023.Day11
#endif
open Xunit
open FsUnit.Xunit
//open Utility

let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input11.txt""")

let exampleData1 = seq {
        yield "...#......"
        yield ".......#.."
        yield "#........."
        yield ".........."
        yield "......#..."
        yield ".#........"
        yield ".........#"
        yield ".........."
        yield ".......#.."
        yield "#...#....."
    }

let parseRow (row, string) =
    string
    |> Seq.indexed
    |> Seq.choose (fun (col, x) ->
        if x = '#' then Some(col, row)
        else None
    )

let parseInput input =
    input
    |> Seq.indexed
    |> Seq.map parseRow
    |> Seq.concat


let expander getStartIndex findGalaxyOnAxel expandRowOrCol map =
    let maxRow = getStartIndex map
    let rec expandRowOrColRec currentMap index =
        let galaxyOnRow = currentMap |> List.exists (findGalaxyOnAxel index)
        let nextMap = 
            match galaxyOnRow with
            | true -> currentMap
            | false -> currentMap |> List.map (expandRowOrCol index)
        match index with 
        | t when t <= 0 -> nextMap
        | tt -> expandRowOrColRec nextMap (index - 1) 
    expandRowOrColRec map maxRow

let getMaxRow map = map |> List.maxBy snd |> snd
let getMaxCol map = map |> List.maxBy fst |> fst
let findGalaxyInRow index x = (snd x) = index
let findGalaxyInCol index x = (fst x) = index

let splits list = 
  let rec splitsAux acc list =
    match list with 
    | x::xs -> splitsAux ((x, xs)::acc) xs
    | _ -> acc |> List.rev
  splitsAux [] list

let getAll2ElementCombinations inputList =
    [ for x, xs in splits inputList do
        for y in xs do yield x, y ]

let manhattenDistance ((x1, y1), (x2, y2)) =
    abs(x1 - x2) + abs(y1 - y2)

// Part 2
// Examples of 10 and 100
// Real expanssion is 1 000 000
let rowExpander size index (col, row) = if row > index then (col, row+size) else (col, row)
let colExpander size index (col, row) = if col > index then (col+size, row) else (col, row)

// error FS0030: Value restriction. The value 'expandRow' has been inferred to have generic type
    //val expandRow: (('_a * int) list -> ('_a * int) list)    
//Either make the arguments to 'expandRow' explicit or, if you do not intend for it to be generic, add a type annotation.
// https://stackoverflow.com/a/70131251
// Is an issue when using REPL/Interactive and the place of usage is not included in the evaluation.
let expandRow = expander getMaxRow findGalaxyInRow
let expandCol = expander getMaxCol findGalaxyInCol

let solve delta input =
    let map = parseInput input |> Seq.toList
    let map2 = expandRow (rowExpander delta) map
    let map3 = expandCol (colExpander delta) map2
    let pairs = getAll2ElementCombinations map3
    pairs |> List.map manhattenDistance |> List.map int64 |> List.sum

let [<Fact>] ``part one examples``() =
    solve 1 exampleData1 |> should equal 374L

let [<Fact>] ``part one solution``() =
    solve 1 input |> should equal 9724940L

let [<Fact>] ``part two examples``() =
    // 1 lower than expected because there is already an empty row/column.
    solve (10-1) exampleData1 |> should equal 1030L
    solve (100-1) exampleData1 |> should equal 8410L

let [<Fact>] ``part two solution``() =
    solve (1000000-1) input |> should equal 569052586852L

