#if COMPILED
module Year2023.Day16
#endif
open Xunit
open FsUnit.Xunit
open System.Collections.Generic

let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input16.txt""")

let exampleData1 = seq {
        yield ".|...\...."
        yield "|.-.\....."
        yield ".....|-..."
        yield "........|."
        yield ".........."
        yield ".........\\"
        yield "..../.\\\\.."
        yield ".-.-/..|.."
        yield ".|....-|.\\"
        yield "..//.|...."
    }

exampleData1 |> Seq.iter (printfn "%A")

let exploreRight row index visited =
    row
    |> Seq.skip index
    |> Seq.takeWhile (fun x -> x <> '|' && x <> '/' && x <> '\\')

let row = ".|...\...."
let index = 0
row
|> Seq.indexed
|> Seq.skip 2
|> Seq.takeWhile (fun x ->
    let xx = snd x
    xx <> '|' && xx <> '/' && xx <> '\\'
    )
|> Seq.toList
// Either we found a match, and we redirect/split.
// Or we are at the end of the map

// We need to mark those found along the way as visited with a direction.

let visited = Set.empty
let newSet = 
    [(2, '.'); (3, '.'); (4, '.')]
    |> List.map (fun x -> ((fst x), 3, 'R'))
    |> Seq.ofList
printfn $"The new set is: {newSet}"
