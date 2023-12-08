#if COMPILED
module Year2023.Day6
#endif
open Xunit
open FsUnit.Xunit
open System.Text.RegularExpressions

let input = [(40L, 215L); (70L, 1051L); (98L, 2147L); (79L, 1005L)]
let exampleData = [(7L, 9L); (15L, 40L); (30L, 200L)]

let rec race (wins: int64) ((totalTime: int64), (recordDistance: int64)) (heldDown: int64) =
    match heldDown with
    | 0L -> wins
    | speed ->
        let timeLeft = totalTime - heldDown
        let distance = timeLeft * speed
        let isWin = if distance > recordDistance then 1L else 0L
        race (wins + isWin) (totalTime, recordDistance) (heldDown - 1L)

let totalWins input =
    input
    |> List.map (fun (totalTime, recordDistance) -> race 0L (totalTime, recordDistance) totalTime)
    |> List.reduce (*)

let inputPartTwo = [(40709879L, 215105121471005L)]
let exampleDataPartTwo = [(71530L, 940200L)]

let [<Fact>] ``part one examples``() =
    totalWins exampleData |> should equal 288L

let [<Fact>] ``part one solution``() =
    totalWins input |> should equal 1084752L

let [<Fact>] ``part two examples``() =
    totalWins exampleDataPartTwo |> should equal 71503L

let [<Fact>] ``part two solution``() =
    totalWins inputPartTwo |> should equal 28228952L
