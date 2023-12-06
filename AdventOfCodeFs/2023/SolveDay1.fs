#if COMPILED
module Year2023.Day1
#endif
open Xunit
open FsUnit.Xunit

let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input1.txt""")

let exampleData1 = seq {
        yield "1abc2"
        yield "pqr3stu8vwx"
        yield "a1b2c3d4e5f"
        yield "treb7uchet"
    }

let convertToIndexedInt (index, character) =
    match System.Int32.TryParse(character |> string) with
    | (true,int) -> Some((index, int))
    | _ -> None

let createCompleteDigit (indexedDigits: (int * int) seq) : int =
    let min = Seq.minBy fst indexedDigits |> snd
    let max = Seq.maxBy fst indexedDigits |> snd
    (min*10)+max

let digitForEachRow input =
    input
    |> Seq.indexed
    |> Seq.choose convertToIndexedInt
    |> createCompleteDigit

let solve input =
    input
    |> Seq.map digitForEachRow
    |> Seq.sum

let findDigitAsString (str: string) compare =
    let low = str.IndexOf(snd compare:string)
    let high = str.LastIndexOf(snd compare:string)
    match low, high with
    | -1, -1 -> []
    | -1, x -> [(x, fst compare + 1)]
    | x, -1 -> [(x, fst compare + 1)]
    | x, y -> [(x, fst compare + 1); (y, fst compare + 1)]

findDigitAsString "one432onetow" (0, "one")
let numbers = [ "one"; "two"; "three"; "four"; "five"; "six"; "seven"; "eight"; "nine" ] |> List.indexed
//let prepedStringDigits = findDigitAsString numbers
let toIndexedFromStringDigits s =
    numbers |> List.map (findDigitAsString s) |> List.concat |> List.distinct

let toIndexedFromIntChars input =
    input
    |> Seq.indexed
    |> Seq.choose convertToIndexedInt

let createCompleteDigit2 list1 list2 =
    list1
    |> List.append list2
    |> createCompleteDigit

let digitForEachRow2 input =
    let fromIntegers = toIndexedFromIntChars input |> Seq.toList
    let fromStringIntegers = toIndexedFromStringDigits input
    printf "%A ->" input
    let a = createCompleteDigit2 fromIntegers fromStringIntegers
    printfn "%A" a
    a

let solve2 input =
    input
    |> Seq.map digitForEachRow2
    |> Seq.sum

let solve3 input =
    input
    |> Seq.map digitForEachRow2

toIndexedFromStringDigits "4nineeightseven2"
toIndexedFromIntChars "4nineeightseven2" |> Seq.toList

let exampleData2 = seq {
        yield "two1nine"
        yield "eightwothree"
        yield "abcone2threexyz"
        yield "xtwone3four"
        yield "4nineeightseven2"
        yield "zoneight234"
        yield "7pqrstsixteen"
    }

solve2 input

let [<Fact>] ``part one individual rows``() =
    digitForEachRow "1abc2" |> should equal 12
    digitForEachRow "8erwer3rewr4" |> should equal 84
    digitForEachRow "qweqwe7rtry" |> should equal 77

let [<Fact>] ``part one examples``() =
    solve exampleData1 |> should equal 142

let [<Fact>] ``part one solution``() =
    solve input |> should equal 53080
