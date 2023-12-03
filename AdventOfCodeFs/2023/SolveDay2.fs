#if COMPILED
module Year2023.Day2
#endif
open Xunit
open FsUnit.Xunit

let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input2.txt""")

let exampleData1 = seq {
        yield "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"
        yield "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue"
        yield "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red"
        yield "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red"
        yield "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green"
    }

// A union type did not turn out to be such a hit.
// At least not with the values stored in it.
// As the group by became rather messy anyway. Could have used a (marble, count:int) tuple.
type Marble =
    | Red of int 
    | Green of int
    | Blue of int
    | Unknown of int

// Function or memeber?
// should a hand and game be a type or not?
type Marble with
    member this.isInvalid =
            match this with
            | Red x when x > 12 -> true
            | Green x when x > 13 -> true
            | Blue x when x > 14 -> true
            | _ -> false

    member this.getValue =
            match this with
            | Red x -> x
            | Green x -> x
            | Blue x -> x
            | _ -> -1

let parseGame (row: string) = 
    let mutable allHands = []
    let games = ((row.Split ':')[1]).Split ';'
    for game in games do
        let mutable hand = []
        let marbels = game.Split ','
        for marbel in marbels do
            let (_, count) = System.Int32.TryParse((marbel.Split ' ')[1])
            let colour = (marbel.Split ' ')[2]
            let marble =
                match colour with
                | "red" -> Red count
                | "green" -> Green count
                | "blue" -> Blue count
                | _ -> Unknown count
            hand <- List.append hand [marble]
        allHands <- List.append allHands [(hand)]
    allHands

let rec invalidHand (hand: Marble list) =
    hand |> List.exists (fun x -> x.isInvalid)

let parseGameId (row: string) =
    let gameId = (((row.Split ':')[0]).Split ' ')[1]
    let (_, id) = System.Int32.TryParse(gameId:string)
    id

let getGameScore gameRow =
    let game = parseGame gameRow
    let gameIsValid = 
        game
        |> List.exists (fun x -> invalidHand x)
        |> not
    if gameIsValid then parseGameId gameRow else 0

let solve input =
    input
    |> Seq.map getGameScore
    |> Seq.sum

// Part 2
let getMaxOfColour (singleColorMarbleList: Marble list) =
    let max = List.maxBy (fun x -> x) singleColorMarbleList
    max.getValue

let getPowerOfCubes groupedInputOfHand =
    groupedInputOfHand
    |> Seq.map getMaxOfColour
    |> Seq.fold (fun total n -> total * n) 1

let gamesGroupedByColor allGames =
    allGames
    |> Seq.map (fun x ->
        x
        |> parseGame
        |> List.concat // Flattens the games into a single list
        |> List.groupBy (fun e -> e.GetType()) // This is a bit of a bummer, that out of the box it groups on type and value.
        |> List.map snd
    )

let solve2 input =
    input
    |> gamesGroupedByColor
    |> Seq.map getPowerOfCubes
    |> Seq.sum


let [<Fact>] ``part one examples``() =
    solve exampleData1 |> should equal 8

let [<Fact>] ``part one solution``() =
    solve input |> should equal 2149

let [<Fact>] ``part two examples``() =
    solve2 (Seq.toList exampleData1) |> should equal 2286

let [<Fact>] ``part two solution``() =
    solve2 (Seq.toList input) |> should equal 71274


let [<Fact>] ``parse game id``() =
   parseGameId "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green" |> should equal 1

let [<Fact>] ``parse game``() =
    parseGame "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green" |> should equal [[Blue 3; Red 4]; [Red 1; Green 2; Blue 6]; [Green 2]]

let [<Fact>] ``invalid hand``() =
    invalidHand [Blue 3; Red 14] |> should equal true

let [<Fact>] ``valid hand``() =
    invalidHand [Blue 3; Red 1] |> should equal false

let [<Fact>] ``get games score``() =
    getGameScore "Game 40: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 14 blue, 10 red" |> should equal 40

let [<Fact>] ``max of colour``() =
    getMaxOfColour [Blue 3; Blue 6] |> should equal 6

let [<Fact>] ``power of cubes``() =
    getPowerOfCubes [[Blue 3; Blue 6]; [Red 4; Red 1]; [Green 2; Green 2]] |> should equal 48
