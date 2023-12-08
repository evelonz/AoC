#if COMPILED
module Year2023.Day5
#endif

open FSharpPlus
open Xunit
open FsUnit.Xunit
open System.Text.RegularExpressions

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

let mapToTuple a =
    match a with
    | [|d; s; r|] -> d, s, r
    | _ -> failwith "incorrect map input count"

let parseMapRow (string: string) =
    let intArray = string.Split(" ") |> Array.map int64
    intArray |> mapToTuple

let parseSeeds (string: string) =
    (string.Split(": ")[1]).Split(" ") |> Array.map int64

let rec parseCategoryMapping currentMapings inputList =
    match inputList with
    | [] -> currentMapings, []
    | head::tail when head = "" -> currentMapings, tail
    | head::tail -> parseCategoryMapping ((parseMapRow head) :: currentMapings) tail

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

// Part 2

let parseSeedRanges (string: string) =
    let intArray = (string.Split(": ")[1]).Split(" ") |> Array.map int64
    let pairs = 
        intArray
        |> Array.chunkBySize 2
        |> Array.choose (function [|x; y|] -> Some (x, y) | _ -> None)

    pairs |> Array.map (fun (start, length) ->
            [| for i in start .. (start + length - 1L) -> i |]
        )

let parseSeedRanges2 (string: string) =
    let intArray = (string.Split(": ")[1]).Split(" ") |> Array.map int64
    let pairs = 
        intArray
        |> Array.chunkBySize 2
        |> Array.choose (function [|x; y|] -> Some (x, y) | _ -> None)
    pairs |> Array.map (fun (start, length) -> (start, start+length-1L))
parseSeedRanges "seeds: 79 14 55 13" |> Array.concat
parseSeedRanges2 "seeds: 79 14 55 13"

//parseSeedRanges "seeds: 364807853 408612163 302918330 20208251 1499552892 200291842 3284226943 16030044 2593569946 345762334 3692780593 17215731 1207118682 189983080 2231594291 72205975 3817565407 443061598 2313976854 203929368" |> Array.concat  |> Array.length

let solve2 input = 
    let inputA = input |> Seq.toList
    let seeds = parseSeedRanges inputA.Head |> Array.concat
    let restOfList = inputA.Tail.Tail.Tail
    let mappings = parseInput [] restOfList |> List.rev
    let locations = mapSeeds seeds mappings
    locations |> Array.min

solve2 exampleData1

let rangeSeedSplitter (lowSeed: int64, highSeed: int64, seedDelta: int64) (lowMap: int64, highMap: int64, delta: int64) =
    // Cases
    // highSeed < lowMap -> no intersection, seed higher than map
    // lowSeed > highMap -> no intersection, seed lower than map
    // lowSeed > lowMap && highSeed < highMap -> Seed within map, 1 part
    // lowSeed < lowMap && highSeed > highMap -> Seed larger and smaller than map, 3 parts
    // lowSeed < lowMap && highSeed < highMap -> Seed intersect lower half, 2 parts
    // lowSeed > lowMap && highSeed > highMap -> Seed intersect upper half, 2 parts

    // As one section may hit multiple maps, or rather the parts that did not map may be part of another mapping, we need to re-iterate them on the same mapping layer. (fold?)
    // This means we also don't translate them before all maps have been checked.
    // We make the assumption that one value will not hit multiple maps on one layer.
    // We could "consolidate" the ranges after each layer. But it should only be the product of the number of maps at each layer * 3 at most.
    // But we need to return the delta and map it afterwards in this case
    
    // For now, let's ignore the seedDelta. In reality, we would want to make sure we don't map it multiple times.
    // But again, we assume to not have multiple hits in one layer. As in no map overlap.
    let result = 
        if lowSeed > highMap || highSeed < lowMap then [(lowSeed, highSeed, seedDelta)]
        else if lowSeed >= lowMap && highSeed <= highMap then [(lowSeed, highSeed, delta)]
        else if lowSeed < lowMap && highSeed > highMap then [(lowSeed, lowMap - 1L, seedDelta); (lowMap, highMap, delta); (highMap + 1L, highSeed, seedDelta)]
        else if lowSeed < lowMap && highSeed <= highMap then [(lowSeed, lowMap - 1L, seedDelta); (lowMap, highSeed, delta)]
        else if lowSeed >= lowMap && highSeed > highMap then [(lowSeed, highMap, delta); (highMap + 1L, highSeed, seedDelta)]
        else failwith "Missed mapping"
    result


let solve3 input = 
    let inputA = input |> Seq.toList
    let seeds = (parseSeedRanges2 inputA.Head) |> Array.toList |> List.map (fun s -> (fst s, snd s, 0L))
    let restOfList = inputA.Tail.Tail.Tail
    let mappings = parseInput [] restOfList |> List.rev
    let mappings2 = 
        mappings
        |> List.map (fun x ->
            x
            |> List.map (fun y ->
                let (destination, start, range) = y
                (start, start+range-1L, destination-start)
            )
        )

    let mapSeeds seeds seedMaps =
        seeds
        |> List.map (fun x ->
            seedMaps 
            |> List.fold (fun acc map -> 
                acc |> List.map (fun seed1 -> rangeSeedSplitter seed1 map) |> List.concat
            ) [x]
        )
        |> List.concat
        |> List.map (fun f -> 
            let (start, endd, delta) = f
            (start+delta, endd+delta, 0L) // update ranges.
            )
    let rec innerLoop seeds mappers =
        match mappers with
        | [] -> seeds
        | head::tail -> innerLoop (mapSeeds seeds head) tail
    
    innerLoop seeds mappings2
    // Flatten seed ranges.
    // Apply delta to each seed range. We ignore overlap for now.
    // Break this out to a rec fun and loop it over the mappings until done.
    // Then find the min value, which would be the min start value of all seed ranges.

solve3 input
|> List.map (fun x -> 
    let (y, _, _ ) = x
    y)
|> List.min

let [<Fact>] ``part one examples``() =
    solve exampleData1 |> should equal 35L

let [<Fact>] ``part one solution``() =
    solve input |> should equal 1181555926L

let [<Fact>] ``some mappings``() =
    let singleDigitMapping = chooseMap [(50, 98, 2); (52, 50, 48)]
    singleDigitMapping 79L |> should equal 81L
    singleDigitMapping 14L |> should equal 14L
    singleDigitMapping 55L |> should equal 57L
    singleDigitMapping 13L |> should equal 13L
    singleDigitMapping 99L |> should equal 51L
    singleDigitMapping 200L |> should equal 200L
    singleDigitMapping -200L |> should equal -200L

let [<Fact>] ``map ranges of seeds``() =
    let mapp = (10L, 20L, 99L)
    rangeSeedSplitter (1L, 0L, 5L) mapp |> should equal [(1L, 0L, 5L)]
    rangeSeedSplitter (22L, 23L, 0L) mapp |> should equal [(22L, 23L, 0L)]
    rangeSeedSplitter (10L, 20L, 0L) mapp |> should equal [(10L, 20L, 99L)]
    rangeSeedSplitter (5L, 25L, 0L) mapp |> should equal [(5L, 9L, 0L); (10L, 20L, 99L); (21L, 25L, 0L)]
    rangeSeedSplitter (8L, 14L, 0L) mapp |> should equal [(8L, 9L, 0L); (10L, 14L, 99L)]
    rangeSeedSplitter (15L, 25L, 0L) mapp |> should equal [(15L, 20L, 99L); (21L, 25L, 0L)]
