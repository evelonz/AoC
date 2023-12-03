#if COMPILED
module Year2023.Day3
#endif
open Xunit
open FsUnit.Xunit
open System.Text.RegularExpressions

let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input3.txt""")

let exampleData1 = seq {
        yield "467..114.."
        yield "...*......"
        yield "..35..633."
        yield "......#..."
        yield "617*......"
        yield ".....+.58."
        yield "..592....."
        yield "......755."
        yield "...$.*...."
        yield ".664.598.."
    }

let rx =
    Regex(@"((?:\d+)+)", RegexOptions.Compiled)
let rx2 =
    Regex(@"[^\d.]", RegexOptions.Compiled)

type Value = int
type Column = int
type Range2 = int * int
type Digit = Value * Column * Range2

let check ((index: int), e) =
    let m = rx.Matches(e)
    m 
    |> Seq.cast
    |> Seq.map (fun (regMatch:Match) ->
        let v = int regMatch.Value
        let c = index
        let r = Range2 (int regMatch.Index, int (regMatch.Index + regMatch.Length - 1))
        Digit (v, c, r)
        )
let check2 ((index: int), e) =
    let m = rx2.Matches(e)
    m 
    |> Seq.cast
    |> Seq.map (fun (regMatch:Match) ->
        let c = index
        let r = int regMatch.Index
        (c, r)
        )

let inline (>=<) a (b,c) = a >= b && a<= c

let digitChooser symbols (digit: Digit) =
    let value, column, range = digit
    let columnRange = (column-1, column+1)
    let row = (fst range-1, snd range+1)
    let exists =
        symbols
        |> List.exists (fun x -> fst x >=< columnRange && snd x >=< row)
    match exists with
    | true -> printf "- %A, %A, %A; -" value columnRange row; Some(int value)
    | _ -> None // printf "- %A, %A, %A; -" value columnRange row; 

let digitNextToSymbol chooser digits =
    digits
    |> Seq.choose chooser

let getDigits input =
    input
    |> Seq.indexed 
    |> Seq.map check 
    |> Seq.concat

let getSymbols input =
    input
    |> Seq.indexed 
    |> Seq.map check2 
    |> Seq.concat
    |> Seq.toList

let solve input =
    let symbols = getSymbols input
    let triggeredChooser = digitChooser symbols
    input
    |> getDigits
    |> digitNextToSymbol triggeredChooser
    |> Seq.sum

//digitChooser [(5,5)] (467, 6, (-7, 8))

// Part 2
let IsGear (digits: Digit list) (symbol: (int * int * string)) =
    let c, r, _ = symbol
    let inRange = 
        digits
        |> List.map (fun digit ->
                let value, column, range = digit
                let columnRange = (column-1, column+1)
                let rowRange = (fst range-1, snd range+1)
                (value, columnRange, rowRange)
            )
        |> List.choose (fun rangeDigit -> 
            let value, columnRange, rowRange = rangeDigit
            let isInRange = c >=< columnRange && r >=< rowRange
            match isInRange with
                | true -> Some(value)
                | _ -> None
            )
    if inRange.Length = 2 then Some(inRange[0] * inRange[1]) else None

let symbol2 ((index: int), e) =
    let m = rx2.Matches(e)
    m 
    |> Seq.cast
    |> Seq.map (fun (regMatch:Match) ->
        let c = index
        let r = int regMatch.Index
        let value = regMatch.Value
        (c, r, value)
        )

let getSymbols2 input =
    input
    |> Seq.indexed 
    |> Seq.map symbol2
    |> Seq.concat
    |> Seq.filter (fun f ->
        let _, _, value = f
        value = "*"
        )
    |> Seq.toList

let solve2 input =
    let digits = getDigits input |> Seq.toList
    let chooser = IsGear digits
    input
    |> getSymbols2
    |> Seq.choose chooser
    |> Seq.sum


let [<Fact>] ``part one examples``() =
    solve exampleData1 |> should equal 4361

let [<Fact>] ``part one solution``() =
    solve input |> should equal 535235

let [<Fact>] ``part two examples``() =
    solve2 exampleData1 |> should equal 467835

let [<Fact>] ``part two solution``() =
    solve2 input |> should equal 79844424
