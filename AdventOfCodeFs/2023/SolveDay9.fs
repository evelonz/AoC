#if COMPILED
module Year2023.Day9
#endif
open Xunit
open FsUnit.Xunit
open System.Text.RegularExpressions

let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input9.txt""")

let exampleData1 = seq {
    yield "0 3 6 9 12 15"
    yield "1 3 6 10 15 21"
    yield "10 13 16 21 30 45"
  }

let parse (input: string array) =
  input |> Array.map int
let parse2 (input: string seq) =
  input 
  |> Seq.map (fun x -> x.Split " ")
  |> Seq.map parse

let reduce input =
  input |> Array.pairwise |> Array.map (fun (fst, snd) -> snd - fst)

let rec handleRow acc input =
  //printfn "%A" input
  match input |> Array.forall (fun x -> x = 0) with
  | true -> acc
  | false ->
    let nxt = reduce input
    let last = nxt |> Array.last
    handleRow (last::acc) nxt

let solve input =
  parse2 input
  |> Seq.map (fun x -> ([x |> Array.last], x))
  |> Seq.map (fun x -> x ||> handleRow)
  |> Seq.map (fun x -> x |> List.fold (fun acc cur -> acc + cur) 0)
  |> Seq.sum


let rec handleRow2 acc input =
  match input |> Array.forall (fun x -> x = 0) with
  | true -> acc
  | false ->
    let nxt = reduce input
    let first = nxt |> Array.head
    handleRow2 (first::acc) nxt

let solve2 input =
  parse2 input
  |> Seq.map (fun x -> ([x |> Array.head], x))
  |> Seq.map (fun x -> x ||> handleRow2)
  |> Seq.map (fun x -> x |> List.fold (fun acc cur -> cur - acc) 0)
  |> Seq.sum

let [<Fact>] ``part one examples``() =
  solve exampleData1 |> should equal 114

let [<Fact>] ``part one solution``() =
  solve input |> should equal 1887980197

let [<Fact>] ``part two examples``() =
  solve2 exampleData1 |> should equal 2

let [<Fact>] ``part two solution``() =
  solve2 input |> should equal 990

