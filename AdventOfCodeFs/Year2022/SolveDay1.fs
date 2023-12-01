#if COMPILED
module Year2022.Day1
#endif
open Xunit
open FsUnit.Xunit

let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input1.txt""")

// Shared
let exampleData = [
        "1000";
        "2000";
        "3000";
        "";
        "4000";
        "";
        "5000";
        "6000";
        "";
        "7000";
        "8000";
        "9000";
        "";
        "10000";
    ]
let solver input = 
    ([|0|], input)
    ||> Seq.fold (fun acc snack ->
        match snack with
        | "" -> Array.append [|0|] acc
        | _ -> acc[0] <- int snack + acc[0]; acc)
    |> Seq.sortDescending
    |> Seq.scan (+) 0 
    |> Seq.take 4
// Can remove the remaining tail variable I think. https://stackoverflow.com/a/31246390
let subSeq predicate accFunc init inSeq =
    seq {
        let mutable ret = init
        let mutable remainingTail = false;
        for current in inSeq do
            match predicate current with
            | true -> remainingTail <- false; yield ret; ret <- init
            | false -> ret <- accFunc ret current; remainingTail <- true;
        if remainingTail then
            yield ret
    }

let pred n: bool = n = ""
let acc (curr: int) (next: string) : int = curr + (int next)
//let lazySolver = subSeq pred acc 0
let se = 
    exampleData
    |> subSeq pred acc 0
se |> Seq.iter (fun f -> printf "%d; " f)
solver exampleData

solver input

let result = ([|0|], List.ofSeq exampleData) ||> Seq.fold (fun acc snack ->
     match snack with
     | "" -> Array.append [|0|] acc
     | _ -> acc[0] <- int snack + acc[0]; acc)
let ordered = Array.sortDescending result

let [<Fact>] ``part one examples``() =
    let result = solver exampleData
    Seq.item 1 result  |> should equal 24000

let [<Fact>] ``part two examples``() =
    let result = solver exampleData
    Seq.item 3 result |> should equal 45000


