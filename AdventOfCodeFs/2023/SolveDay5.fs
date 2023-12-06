#if COMPILED
module Year2023.Day5
#endif
#if !COMPILED
#r @"nuget: FSharpPlus"
#endif

open FSharpPlus
open Xunit
open FsUnit.Xunit
open System.Text.RegularExpressions

// Play with parser
let (a:int) = parse "5"
let (b:int option) = tryParse "5"
//let (a:int list) = parse " 79 14 55 13"
//let (c: Net.IPAddress option) = tryParse "10.0.0.1"

// end play

let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input5.txt""")

let exampleData1 = seq {
        yield "seeds: 79 14 55 13"
        yield ""
        yield "seed-to-soil map:"
        yield "50 98 2"
        yield "52 50 48"
        yield ""
        yield "soil-to-fertilizer map:"
        yield "0 15 37"
        yield "37 52 2"
        yield "39 0 15"
        yield ""
        yield "fertilizer-to-water map:"
        yield "49 53 8"
        yield "0 11 42"
        yield "42 0 7"
        yield "57 7 4"
        yield ""
        yield "water-to-light map:"
        yield "88 18 7"
        yield "18 25 70"
        yield ""
        yield "light-to-temperature map:"
        yield "45 77 23"
        yield "81 45 19"
        yield "68 64 13"
        yield ""
        yield "temperature-to-humidity map:"
        yield "0 69 1"
        yield "1 0 69"
        yield ""
        yield "humidity-to-location map:"
        yield "60 56 37"
        yield "56 93 4"
    }

let array = "0 69 1".Split(" ") |> Array.map int64
let mapToTuple a =
    match a with
    | [|d; s; r|] -> d, s, r
    | _ -> failwith "incorrect map input count"

let parseMapRow (string: string) =
    let intArray = string.Split(" ") |> Array.map int64
    intArray |> mapToTuple

let parseSeeds (string: string) =
    (string.Split(": ")[1]).Split(" ") |> Array.map int64

parseMapRow "0 69 1"

let rec parseCategoryMapping currentMapings inputList =
    match inputList with
    | [] -> currentMapings, []
    | head::tail when head = "" -> currentMapings, tail
    | head::tail -> parseCategoryMapping ((parseMapRow head) :: currentMapings) tail

let adam = seq {
        yield "45 77 23"
        yield "81 45 19"
        yield "68 64 13"
        yield ""
        yield "68 64 13"
        }
parseCategoryMapping [] (adam |> Seq.toList)

let rec parseInput currentMapings inputList =
    match inputList with
    | [] -> currentMapings
    | list ->
        let newMap, remainder = parseCategoryMapping [] list
        if remainder.Length = 0 then
            newMap :: currentMapings
        else
            let withoutDescriptiveLine = remainder.Tail
            parseInput ((newMap |> List.rev) :: currentMapings) withoutDescriptiveLine

let benny = seq {
        //yield "temperature-to-humidity map:"
        yield "0 69 1"
        yield "1 0 69"
        yield ""
        yield "humidity-to-location map:"
        yield "60 56 37"
        yield "56 93 4"
}
parseInput [] (benny |> Seq.toList) |> List.rev

let chooseMap (tooList: (int64 * int64 * int64) list) (from: int64) =
    let foundMatch = tooList |> List.choose (fun x ->
            match x with
            | destination, source, range when source <= from && from < (source + range) -> Some (source, destination)
            | _ -> None
            )
    match foundMatch with
    | [] -> from
    | [source, destination] -> from + (destination - source)
    | head::tail -> failwithf "To many matches in mapping: %A - %A" head tail

// from 55
// sstart 50 range 48 destination 52
// 79 -> destination 52 - sstart 50 = delta = 2
// 79 + delta = 81
let p = chooseMap [(50, 98, 2); (52, 50, 48)] 
p 79 // 81
p 14 // 14
p 55 // 57
p 13 // 13
p 99 // 51
p 200 // 200
p -200 // -200

let rec mapSeeds seeds mappings =
    match mappings with
    | [] -> seeds
    | head::tail ->
        let newSeeds = seeds |> Array.map (chooseMap head)
        printfn "%A" newSeeds
        mapSeeds newSeeds tail

let solve input = 
    let inputA = input |> Seq.toList
    let seeds = parseSeeds inputA.Head
    let restOfList = inputA.Tail.Tail.Tail
    let mappings = parseInput [] restOfList |> List.rev
    let locations = mapSeeds seeds mappings
    locations |> Array.min

solve exampleData1
solve input
// 1181555926L



let [<Fact>] ``part one examples``() =
    solve exampleData1 |> should equal 35L

let [<Fact>] ``part one solution``() =
    solve input |> should equal 1181555926L
