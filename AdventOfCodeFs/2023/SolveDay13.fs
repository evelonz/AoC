#if COMPILED
module Year2023.Day13
#endif
open Xunit
open FsUnit.Xunit
//open Utility

let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input13.txt""")

let exampleData1 = seq {
        yield "#.##..##."
        yield "..#.##.#."
        yield "##......#"
        yield "##......#"
        yield "..#.##.#."
        yield "..##..##."
        yield "#.#.##.#."
        yield ""
        yield "#...##..#"
        yield "#....#..#"
        yield "..##..###"
        yield "#####.##."
        yield "#####.##."
        yield "..##..###"
        yield "#....#..#"
    }

let rec parse accTotal accCurr input =
    match input with 
    | [] -> (accCurr |> List.rev)::accTotal
    | head::tail ->
        let endd, accCurr' = 
            match head with 
            | "" -> true, accCurr |> List.rev
            | s -> false, s::accCurr
        let newTotal = if endd then accCurr'::accTotal else accTotal
        let newCurrent = if endd then [] else accCurr'
        parse newTotal newCurrent tail

let reverse (input:string) = input |> Seq.rev |> System.String.Concat

let matchRow pivot (row: string) =
    let left = row[..(pivot-1)]
    let right = row[(pivot)..]
    match left, right with
    | left, right when right.Length < left.Length -> left.Substring(left.Length-right.Length, right.Length) = (right |> reverse)
    | l, r when r.Length > l.Length -> (l |> reverse) = r.Substring(0, l.Length)
    | l, r -> l = (r |> reverse)

let matchAllRows pivot rows =
    rows
    |> List.forall (matchRow pivot)

let transpose (llst: 'a list list) : 'a list list = 
    List.map (List.map List.head) (List.init llst.Head.Length (fun x -> List.fold (fun x f -> f x) llst (List.init x (fun x -> (List.map List.tail)))))

let transposeInput (input: string list) =
    let temp = input |> List.map (fun x -> x |> Seq.toList)
    transpose temp |> List.map (fun x -> System.String.Concat(Array.ofList(x)))


let iterativeSearch (mirrors: string list) =
    let mutable found = -1
    for pivot in 1..((mirrors[0].Length)-1) do
        let isMirrored = matchAllRows pivot mirrors
        found <- if isMirrored then pivot else found
    match found with
    | -1 -> None
    | found -> Some(found)

let solveOneMap input =
    let rowCheck = iterativeSearch input
    match rowCheck with
    | Some(x) -> x
    | None -> 
        let columnCheck = iterativeSearch (transposeInput input)
        match columnCheck with
        | Some(y) -> y*100
        | None -> 
            printfn "%A" input
            failwith "Unable to find pivot"

let solve input =
    let allMaps = parse [] [] (input |> Seq.toList)
    allMaps |> List.map solveOneMap |> List.sum

let [<Fact>] ``part one examples``() =
    solve exampleData1 |> should equal 405

let [<Fact>] ``part one solution``() =
    solve  input |> should equal 31265

