#if COMPILED
module Year2023.Day8
#endif
open Xunit
open FsUnit.Xunit

let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input8.txt""")
let exampleData = seq {
        yield "RL"
        yield ""
        yield "AAA = (BBB, CCC)"
        yield "BBB = (DDD, EEE)"
        yield "CCC = (ZZZ, GGG)"
        yield "DDD = (DDD, DDD)"
        yield "EEE = (EEE, EEE)"
        yield "GGG = (GGG, GGG)"
        yield "ZZZ = (ZZZ, ZZZ)"
    }
let exampleData2 = seq {
        yield "LLR"
        yield ""
        yield "AAA = (BBB, BBB)"
        yield "BBB = (AAA, ZZZ)"
        yield "ZZZ = (ZZZ, ZZZ)"
    }
let exdata3 = seq {
        yield "LR"
        yield ""
        yield "11A = (11B, XXX)"
        yield "11B = (XXX, 11Z)"
        yield "11Z = (11B, XXX)"
        yield "22A = (22B, XXX)"
        yield "22B = (22C, 22C)"
        yield "22C = (22Z, 22Z)"
        yield "22Z = (22B, 22B)"
        yield "XXX = (XXX, XXX)"
    }

let parseInstructions (string: string) =
    string |> Seq.toArray

let parseRow (string: string) =
    let current = string.Split(" = ")[0]
    let maps = string.Split(" = (")[1]
    let left = maps.Split(", ")[0]
    let right = (maps.Split(", ")[1]).TrimEnd(')')
    (current, left, right)

let rec parseRows map (strings: string list) =
    match strings with
    | [] -> map
    | head::tail ->
        let current, left, right = parseRow head
        let newMap = map |> Map.add current (left, right)
        parseRows newMap tail

let parseInput input = 
    let inputA = input |> Seq.toList
    let instructions = parseInstructions inputA.Head
    let restOfList = inputA.Tail.Tail
    let mappings = parseRows Map.empty restOfList
    (instructions, mappings)

let rec mapInstruction map current index (instructions: char array) instrL =
    let (left, right) = map |> Map.find current
    let instruction = instructions[(index % instrL)]
    match current with
    | "ZZZ" -> index
    | _ ->
        match instruction with
        | 'L' -> mapInstruction map left (index + 1) instructions instrL
        | 'R' -> mapInstruction map right (index + 1) instructions instrL
        | _ -> failwith "invalid instruction"

let solve input =
    let instructions, map = parseInput input
    mapInstruction map "AAA" 0 instructions instructions.Length

// Part two

let rec mapInstructionFindLoopIndex map history lastEnding index (instructions: char array) instrL (current:string)=
    let (left, right) = map |> Map.find current
    let relativeInstIndex = (index % instrL)
    let instruction = instructions[relativeInstIndex]
    // The data is nice enough that we only need the last, and for our input only, ending
    let newEnding = if current.EndsWith('Z') then index else lastEnding
    let repeated = history |> Set.contains (current, relativeInstIndex, instruction)
    match repeated with
    | true -> newEnding
    | false ->
        let newHistory = history |> Set.add (current, relativeInstIndex, instruction)
        match instruction with
        | 'L' -> mapInstructionFindLoopIndex map newHistory newEnding (index + 1) instructions instrL left
        | 'R' -> mapInstructionFindLoopIndex map newHistory newEnding (index + 1) instructions instrL right
        | _ -> failwith "invalid instruction"

let rec gcd (a:int64) (b:int64) =
    if b = 0
        then abs a
    else gcd b (a % b)
let lcmSimple a b = a*b/(gcd a b)
let rec lcm = function
    | [a;b] -> lcmSimple a b
    | head::tail -> lcmSimple (head) (lcm (tail))
    | [] -> 1

let solve2 input =
    let instructions, maps = parseInput input
    let preEmptyMapper = mapInstructionFindLoopIndex maps Set.empty 0 0 instructions instructions.Length 
    let startingPositions = maps |> Map.filter (fun key _ -> key.EndsWith('A')) |> Map.toList |> List.map fst
    
    startingPositions
    |> List.map preEmptyMapper
    |> List.map int64
    |> lcm

let [<Fact>] ``part one examples``() =
    solve exampleData |> should equal 2

let [<Fact>] ``part one solution``() =
    solve exampleData2 |> should equal 6

let [<Fact>] ``part two examples``() =
    solve2 exdata3 |> should equal 6L

let [<Fact>] ``part two solution``() =
    solve2 input |> should equal 14935034899483L

// TODO: Start writing down things that we looked into as part of this solution
// 8: Set
// 7: Map. Order by type
// 6: More recursion. List reduce
// 5: List rec
// 4: array indexing
// 3: Regex with repeated patterns. Flatten nested lists
// 2: Types. Group by type. Pattern matching. List fold
// 1: Indexed lists. Int parsing. List choose
