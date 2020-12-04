#if COMPILED
module Year2020.Day2
#endif

open Xunit
open FsUnit.Xunit

// Shared
let exampleData = [
        "1-3 a: abcde"
        "1-3 b: cdefg"
        "2-9 c: ccccccccc"
    ]

type PasswordRecord = 
    { FirstInt: int
      SecondInt: int
      Character: char
      Password: string }

let parseInput (input: string) =
    let words = input.Split [|'-'; ' '; ':'|]
    let record = 
        { FirstInt = words.[0] |> int; 
        SecondInt = words.[1] |> int; 
        Character = words.[2] |> char;
        Password = words.[4]; }
    record

// Part 1
let correctNumberOfCharacters record =
    let passwordCharCount = record.Password |> Seq.filter (fun s -> s = record.Character) |> Seq.length
    passwordCharCount >= record.FirstInt   && passwordCharCount <= record.SecondInt 

let solution1 input = input |> Seq.map parseInput |> Seq.filter correctNumberOfCharacters |> Seq.length

// Part 2
let correctCharacterPositions record =
    (record.Password.[record.FirstInt - 1] = record.Character) <> (record.Password.[record.SecondInt - 1] = record.Character)

let solution2 input = input |> Seq.map parseInput |> Seq.filter correctCharacterPositions |> Seq.length


let [<Fact>] ``part one examples``() =
    let result = solution1 exampleData
    result |> should equal 2

let [<Fact>] ``part one solved``() =
    let data = Utility.loadPussleData __SOURCE_DIRECTORY__ "2"
    let result = solution1 data
    Assert.Equal(414, result)

let [<Fact>] ``part two examples``() =
    let result = solution2 exampleData
    result |> should equal 1

let [<Fact>] ``part two solved``() =
    let data = Utility.loadPussleData __SOURCE_DIRECTORY__ "2"
    let result = solution2 data
    Assert.Equal(413, result)

