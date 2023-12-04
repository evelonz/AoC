#if COMPILED
module Year2023.Day4
#endif
open Xunit
open FsUnit.Xunit
open System.Text.RegularExpressions

let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input4.txt""")

let exampleData1 = seq {
        yield "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53"
        yield "Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19"
        yield "Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1"
        yield "Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83"
        yield "Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36"
        yield "Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11"
    }

let convertToInt character =
    match System.Int32.TryParse(character |> string) with
    | (true,int) -> Some(int)
    | _ -> None

let parseRow (row:string) =
    let winningNumbers = ((row.Split(":")[1]).Split("|")[0]).Split(" ")
    let yourNumbers = ((row.Split(":")[1]).Split("|")[1]).Split(" ")
    let winningsAsDigits = winningNumbers |> Array.choose convertToInt
    let yoursAsDigits = yourNumbers |> Array.choose convertToInt
    (winningsAsDigits, yoursAsDigits)

let getMatchingNumbers (winnigs, numbers) =
    numbers 
    |> Array.filter (fun t -> winnigs |> Array.exists (fun t2 -> t=t2))

let getCardScore (matchingNumbers: int array) =
    pown 2 (matchingNumbers.Length-1)

let solution = parseRow >> getMatchingNumbers >> getCardScore

let solve input =
    input
    |> Seq.map solution
    |> Seq.sum

// Part two
let getRowWinnings ((total: int array), (index: int), parsedRow) =
    let gameId = index + 1
    let wins = getMatchingNumbers parsedRow
    let numberOfScratchcards = total[gameId]
    let mutable ind = 1
    for _ in wins do
        total[gameId+ind] <- total[gameId+ind] + numberOfScratchcards
        ind <- ind + 1
    total

let rec getscratchcardCount (total: int array) indexedParsedRows =
    match indexedParsedRows with
    | [] -> total
    | head::tail ->
        let newTotal = getRowWinnings (total, fst head, snd head)
        getscratchcardCount total tail

let solve2 input =
    let arrayForTotals = [| for _ in 1 .. (input |> Seq.toArray).Length + 1 -> 1 |]
    arrayForTotals[0] <- 0
    input
    |> Seq.map parseRow
    |> Seq.indexed
    |> Seq.toList
    |> getscratchcardCount arrayForTotals
    |> Array.sum


let [<Fact>] ``get winnings from single game part one``() =
    solution "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53" |> should equal 8

let [<Fact>] ``part one examples``() =
    solve exampleData1 |> should equal 13

let [<Fact>] ``part one solution``() =
    solve input |> should equal 25231

let [<Fact>] ``test simple scratchcard winnings increament part two``() =
    getRowWinnings ([|0; 1; 1; 1; 1; 1; |], 0, ([|41; 48; 83; 86; 17|], [|83; 86; 6; 31; 17; 9; 48; 53|])) |> should equal [|0; 1; 2; 2; 2; 2|]

let [<Fact>] ``part two examples``() =
    solve2 exampleData1 |> should equal 30

let [<Fact>] ``part two solution``() =
    solve2 input |> should equal 9721255
