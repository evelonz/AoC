#if COMPILED
module Year2023.Day19
#endif
open Xunit
open FsUnit.Xunit

let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input19.txt""")

let exampleData1 = seq {
        yield "px{a<2006:qkq,m>2090:A,rfg}"
        yield "pv{a>1716:R,A}"
        yield "lnx{m>1548:A,A}"
        yield "rfg{s<537:gd,x>2440:R,A}"
        yield "qs{s>3448:A,lnx}"
        yield "qkq{x<1416:A,crn}"
        yield "crn{x>2662:A,R}"
        yield "in{s<1351:px,qqz}"
        yield "qqz{s>2770:qs,m<1801:hdj,R}"
        yield "gd{a>3333:R,R}"
        yield "hdj{m>838:A,pv}"
        yield ""
        yield "{x=787,m=2655,a=1222,s=2876}"
        yield "{x=1679,m=44,a=2067,s=496}"
        yield "{x=2036,m=264,a=79,s=2244}"
        yield "{x=2461,m=1339,a=466,s=291}"
        yield "{x=2127,m=1623,a=2188,s=1013}"
    }

[<Struct>]
type xmas = { x: int; m: int; a: int; s: int }

// Split xmases
let parseSingleXmasRow (input: string) = 
    let dat = ((input.Split('{')[1]).TrimEnd('}')).Split(',') |> Array.map (fun x -> (x.Split('=')[1]))
    { x=(int dat[0]); m=(int dat[1]); a=(int dat[2]); s=(int dat[3]) }

let parseXmasRows (input: string seq) = 
    let data = input |> Seq.skipWhile (fun x -> x <> "") |> Seq.skip 1
    data
    |> Seq.map parseSingleXmasRow

let buildOperation splitConditions =
    match splitConditions with
    | 'x', '<', value -> (fun x -> x.x < value)
    | 'x', '>', value -> (fun x -> x.x > value)
    | 'm', '<', value -> (fun x -> x.m < value)
    | 'm', '>', value -> (fun x -> x.m > value)
    | 'a', '<', value -> (fun x -> x.a < value)
    | 'a', '>', value -> (fun x -> x.a > value)
    | 's', '<', value -> (fun x -> x.s < value)
    | 's', '>', value -> (fun x -> x.s > value)
    | _ -> failwith "error"

let opreationTemplate check retur (object:xmas) =
    if check object then Some(retur) else None // How do we get the next call to chain? We want to compose these in the end, >> >> >>

let opreationTemplateChainable template (xmas, lastValue) =
    match lastValue with
    | Some x -> xmas, lastValue
    | None -> xmas, (template xmas)

let buildChainableOpeartion templateFunction returnValue
    = opreationTemplateChainable (opreationTemplate templateFunction returnValue)

let opConditionStringToChainTemplate (opString: string) =
    let hasCondition = opString.Split(':')
    match hasCondition.Length with
    | 2 -> 
        let splitCondition = opString.Split(':')[0]
        let splitOperators = splitCondition[0], splitCondition[1], int (splitCondition[2..])
        let opFunc = buildOperation splitOperators
        buildChainableOpeartion opFunc (opString.Split(':')[1])
    | 1 -> 
        buildChainableOpeartion (fun x -> true) (opString.Split(':')[0])
    | _ -> failwithf "Unparsable single operator: %A" opString

let buildSingleInstructionSet (inputInst:string) =
    let opCode = inputInst.Split('{')[0]
    let operations = ((inputInst.Split('{')[1]).TrimEnd('}')).Split(',')
    let operator = 
        operations
        |> Array.map opConditionStringToChainTemplate
        |> Array.reduce (>>)
    opCode, operator

let buildMap input =
    let instructions = input |> Seq.takeWhile (fun x -> x <> "")
    instructions
    |> Seq.map buildSingleInstructionSet
    |> Map.ofSeq

let runOperations opMap start (object:xmas) =
    let rec runOpRec opMap (object:xmas) currentOption =
        match currentOption with
        | Some current ->
            match current with
            | "A" -> Some(object)
            | "R" -> None
            | _ -> 
                let nextOp = opMap |> Map.find current
                let _, nextCurrentOption = nextOp (object, None)
                runOpRec opMap (object:xmas) nextCurrentOption
        | None -> failwith "Dead End"
    runOpRec opMap (object:xmas) start

let applyOperations map xmases =
    let start = (Some "in")
    xmases
    |> Seq.choose (fun x -> runOperations map start x)
    |> Seq.map (fun accXmas -> accXmas.x + accXmas.m + accXmas.a + accXmas.s)
    |> Seq.sum

let solve input = 
    let real = buildMap input
    let datarows = parseXmasRows input
    applyOperations real datarows

let [<Fact>] ``part one examples``() =
    solve exampleData1 |> should equal 19114

let [<Fact>] ``part one solution``() =
    solve  input |> should equal 263678

// Part Two

// Bruteforce does not work, speed keep decreasing below 200 numbers / ms => 40 years.
//let madness maps =
//    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
//    let mutable approved = 0
//    let seq1 = seq { for i in 1 .. 4000 -> i }
//    for x in seq1 do
//        for m in seq1 do
//            for a in seq1 do
//                for s in seq1 do
//                    let res = runOperations maps (Some "in")  { x=x; m=m; a=a; s=s }
//                    match res with 
//                    | Some _ -> approved <- approved + 1
//                    | None -> ()

//            printfn "%d, %d: %d" x m approved
//            printfn "Op/ms: %f" (16000000.0 / (stopWatch.Elapsed.TotalMilliseconds))
//    stopWatch.Stop()

//madness testmap2

// If instead starting at all accepted endpoints, can we go backwards and build up all ranges that allow you to end up there?
// Start with this A from the example: 
// "px{a<2006:qkq,m>2090:A,rfg}" 

// x: [1..4000] m: [1..4000] a: [1..4000] s: [1..4000]

// a >= 2006 // need to miss this one.
// m > 2090 // need to hit this!

// x: [1..4000] m: [2091..4000] a: [2006..4000] s: [1..4000]

// Only one path to px
// "in{s<1351:px,qqz}"

// s < 1351 // need to hit this!
// x: [1..4000] m: [2091..4000] a: [2006..4000] s: [1..1350]

//4000L * (4000L-2091L+1L) * (4000L-2006L+1L) * (1350L) // 20576430000000L
//4000L*4000L*4000L*4000L

// There may be overlap, in which case we need to reduce/merge them somehow.
// Not sure the best option is to start with A.

// 256000000000000L
// 020576430000000L