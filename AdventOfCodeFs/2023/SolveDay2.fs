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

let parseGameId (row: string) =
    let gameId = (((row.Split ':')[0]).Split ' ')[1]
    let (_, id) = System.Int32.TryParse(gameId:string)
    id

let parseGame (row: string) = 
    let mutable allHands = []
    let games = ((row.Split ':')[1]).Split ';'
    for game in games do
        let mutable hand = []
        let marbels = game.Split ','
        for marbel in marbels do
            let (_, count) = System.Int32.TryParse((marbel.Split ' ')[1])
            let colour = (marbel.Split ' ')[2]
            hand <- List.append hand [(count, colour)]
        allHands <- List.append allHands [(hand)]
    allHands

let handNotPossible hand =
    match hand with
    | (count, color) when color = "red" && count > 12 -> true
    | (count, color) when color = "green" && count > 13 -> true
    | (count, color) when color = "blue" && count > 14 -> true
    | _ -> false

let rec PossibleGame hands =
    match hands with
    | [] -> true
    | head::tail ->
        let notPossibleGame = handNotPossible head
        if notPossibleGame
            then false
            else PossibleGame tail

let PossibleGames gameRow =
    let game = parseGame gameRow
    let mutable validgame = true
    for hands in game do
        let result = PossibleGame hands
        validgame <- if validgame && not result then false else validgame
    if validgame then parseGameId gameRow else 0

let solve input =
    input
    |> Seq.map PossibleGames
    |> Seq.sum

let getMaxOfColour input =
    let colour = fst input
    let marbles = snd input
    let max = List.maxBy (fun x -> fst x) marbles |> fst
    //printf "%A, %A" colour max
    max

let getPowerOfCubes groupedInputOfHand =
    groupedInputOfHand
    |> List.map getMaxOfColour
    |> List.fold (fun total n -> total * n) 1

let solve2 input =
    input
    |> List.map (fun x ->
            parseGame x
            |> List.concat
            |> List.groupBy (fun x -> snd x)    
        )
    |> List.map getPowerOfCubes
    |> List.sum

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
    parseGame "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green" |> should equal [[(3, "blue"); (4, "red")]; [(1, "red"); (2, "green"); (6, "blue")];[(2, "green")]]

let [<Fact>] ``impossible marble``() =
    handNotPossible (13, "red") |> should equal true

let [<Fact>] ``possible hand``() =
    PossibleGame [(3, "blue"); (4, "red")] |> should equal true

let [<Fact>] ``possible game``() =
    PossibleGames "Game 40: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 10 red" |> should equal 0

let [<Fact>] ``max of colour``() =
    getMaxOfColour ("blue", [(3, "blue"); (6, "blue")]) |> should equal 6

let [<Fact>] ``power of cubes``() =
    getPowerOfCubes [("blue", [(3, "blue"); (6, "blue")]); ("red", [(4, "red"); (1, "red")]);("green", [(2, "green"); (2, "green")])] |> should equal 48
