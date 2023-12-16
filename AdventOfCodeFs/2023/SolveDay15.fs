#if COMPILED
module Year2023.Day15
#endif
open Xunit
open FsUnit.Xunit
//open Utility

let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input15.txt""")

let exampleData1 = seq {
        yield "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7"
    }

let hashi str =
    str |> Seq.map int |> Seq.fold (fun acc x -> ((x + acc) * 17)%256 ) 0

let solve (input: string seq) = 
    let inputAsStr = input |> Seq.head
    inputAsStr.Split ',' |> Array.map hashi |> Array.sum

// Part two
let subtract (filter: string) (box: (string * int) list) =
    box |> List.filter (fun x -> (fst x) <> filter)

let add (replacement: string*int) (box: (string * int) list) =
    let exists = box |> List.filter (fun x -> (fst x) = (fst replacement))
    match exists with
    | [] -> box@[replacement]
    | _ -> box |> List.map (fun x -> if (fst x) = (fst replacement) then replacement else x)

let matchInstruction (boxes: (string * int) list array) (instr: string) =
    let operationIndex = instr.IndexOfAny [|'='; '-'|]
    let op = instr[operationIndex]
    let label = instr[..(operationIndex-1)]
    let hash = hashi label
    let box = boxes[hash]
    let newBox = 
        match op with
        | '=' -> add (label, (int(string (instr[(operationIndex+1)])))) box
        | '-' -> subtract label box
        | _ -> failwithf "invalid operation: %A" instr
    boxes[hash] <- newBox
    boxes

let solve2 (input: string seq) = 
    let boxes: (string * int) list array = Array.create 256 []
    let inputAsStr = input |> Seq.head
    let instr = inputAsStr.Split ',' |> Array.fold (fun acc ínstr -> matchInstruction acc ínstr) boxes
    let temp = 
        instr
        |> Array.indexed
        |> Array.map (fun (index, x) -> 
                x |> List.indexed |> List.map (fun (i, y) -> (index+1) * (i+1) * (snd y))
            )
    temp |> Seq.concat |> Seq.sum

let [<Fact>] ``part one examples``() =
    solve exampleData1 |> should equal 1320

let [<Fact>] ``part one solution``() =
    solve  input |> should equal 510273

let [<Fact>] ``part two examples``() =
    solve2 exampleData1 |> should equal 145

let [<Fact>] ``part two solution``() =
    solve2  input |> should equal 212449

