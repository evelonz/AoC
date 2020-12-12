#if COMPILED
module Year2020.Day5
#endif

open Xunit
open FsUnit.Xunit

let rec bisect data low high highchar =
    match data with
    | head :: tail -> 
        let delta = ((high - low) / 2) + 1
        match head with
        | c when c = highchar -> bisect tail low (high - delta) highchar
        | _ -> bisect tail (low + delta) high highchar
    | [] -> low

let getSeatNumber (input: string) =
    let rows = input.[0..6]
    let columns = input.[^2..]
    let r = bisect (Seq.toList rows) 0 127 'F'
    let c = bisect (Seq.toList columns) 0 7 'L'
    (r * 8) + c

let solution (input: seq<string>) = 
    let takenSeats = input |> Seq.map getSeatNumber
    let ans1 = takenSeats |> Seq.max
    let ans2 = takenSeats |> Seq.sort |> Seq.windowed 2 |> Seq.filter (fun win -> (win.[1] - win.[0]) = 2 ) |> Seq.map (fun win -> win.[0] + 1) |> Seq.tryHead
    ans1, ans2

[<Theory>]
[<InlineData("FBFBBFFRLR", 357)>]
[<InlineData("BFFFBBFRRR", 567)>]
[<InlineData("FFFBBBFRRR", 119)>]
[<InlineData("BBFFBBFRLL", 820)>]
let ``solve examples`` input expected =
    let (first, _) = solution (Seq.singleton input)
    first |> should equal expected

let [<Fact>] ``pussle solved``() =
    let data = Utility.loadPussleData __SOURCE_DIRECTORY__ "5"
    let (first, second) = solution data
    first |> should equal 850
    second.Value |> should equal 599

