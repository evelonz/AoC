#if COMPILED
module Year2023.Day7
#endif
open Xunit
open FsUnit.Xunit
open System.Text.RegularExpressions
open System
//let input = [(40L, 215L); (70L, 1051L); (98L, 2147L); (79L, 1005L)]
let exampleData = seq {
        yield "32T3K 765"
        yield "T55J5 684"
        yield "KK677 28"
        yield "KTJJT 220"
        yield "QQQJA 483"
    }

// For type, count each type. Get the two max counts, this gives us all we need to define types.
// As the card does not matter for type nor `second ordering rule`, we don't need to track card rank with counts.

// My my, these sweet map functions <3
let defaultMap = Map.empty |> Map.add "A" 0 |> Map.add "K" 0
let ace = defaultMap |> Map.change "Q" (fun x ->
    match x with
    | Some(x) -> Some(x + 1)
    | None -> Some(1)
    )
'a' < 'b'
'9' > '2'
'a' > 'b'
let charToSortableValues char =
    let rev = 
        match char with
        | 'A' -> 'e'
        | 'K' -> 'd'
        | 'Q' -> 'c'
        | 'J' -> 'b'
        | 'T' -> 'a'
        | x -> x
    'z' - rev
let handToSortableValues (input: string) =
    input
    |> Seq.map charToSortableValues
    |> String.Concat
handToSortableValues "AKQJT98765432"
//A, K, Q, J, T, 9, 8, 7, 6, 5, 4, 3, or 2
"AKQJT98765432" |> Seq.map charToSortableValues |> Seq.toList |> List.sortDescending
let incrementMap map char =
    map 
    |> Map.change char (fun x ->
        match x with
        | Some(x) -> Some(x + 1)
        | None -> Some(1)
        )

let countCardsOfEachType string =
    let map = Map.empty |> Map.add '*' -99 // Add a default just to allow Seq.take 2 with 5 of a kind
    let rankedMap = string |> Seq.fold (fun acc x -> incrementMap acc x) map
    rankedMap |> Map.values |> Seq.sortDescending |> Seq.take 2 |> Seq.toList


countCardsOfEachType "QQQQQ"
type HandType =
    // Re-order for compare to work
    | ``High card``
    | ``One pair``
    | ``Two pair``
    | ``Three of a kind``
    | ``Full house``
    | ``Four of a kind``
    | ``Five of a kind``

let getHandType list =
    match list with
    | [5; _] -> ``Five of a kind``
    | [4; _] -> ``Four of a kind``
    | [3; 2] -> ``Full house``
    | [3; _] -> ``Three of a kind``
    | [2; 2] -> ``Two pair``
    | [2; _] -> ``One pair``
    | [1; 1] -> ``High card``
    | _ -> failwith "invalid hand type"

countCardsOfEachType "QQQQQ" |> getHandType //|> should equal ``Five of a kind``
countCardsOfEachType "QKQQQ" |> getHandType //|> should equal ``Four of a kind``
countCardsOfEachType "QQJJQ" |> getHandType //|> should equal ``Full house``
countCardsOfEachType "KQQJQ" |> getHandType //|> should equal ``Three of a kind``
countCardsOfEachType "QQ2K2" |> getHandType //|> should equal ``Two pair``
countCardsOfEachType "AKQ22" |> getHandType //|> should equal ``One pair``
countCardsOfEachType "Q4352" |> getHandType //|> should equal ``High card``

// Need custom sort for relative strenght... A lookup table perhaps.
'A' > 'K'
``Five of a kind`` > ``Four of a kind``

let parseAndSortHands hands =
    hands
    //|> Seq.map handToSortableValues
    |> Seq.map (fun xx ->
        let x = fst xx
        let handType = countCardsOfEachType x |> getHandType
        //let sortableString = HandToSortableValues x
        (handType, snd xx, x)
        )
    |> Seq.sortByDescending (fun x -> 
        let (handType, bet, sortStr) = x
        (handType, handToSortableValues sortStr)
        )
    |> Seq.indexed

//HandToSortableValues
let parseInput (input:string ) =
    let hand = input.Split(" ")[0]
    let bet = int (input.Split(" ")[1])
    (hand, bet)
let parsed = exampleData |> Seq.map parseInput
let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input7.txt""")
let parsed2 = input |> Seq.map parseInput
parseAndSortHands parsed2
|> Seq.map (fun x ->
    let (index, (_, bet, _)) = x
    (index + 1) * bet
    )
|> Seq.fold (+) 0

// Part 2
// Simple to change individual strength of Joker
let charToSortableValues2 char =
    let rev = 
        match char with
        | 'A' -> 'e'
        | 'K' -> 'd'
        | 'Q' -> 'c'
        | 'J' -> '1'
        | 'T' -> 'a'
        | x -> x
    'z' - rev
// TODO Fix this to accept the charToSort method.
let handToSortableValues2 (input: string) =
    input
    |> Seq.map charToSortableValues2
    |> String.Concat

// Now, how to handle hands?
// Do we extract J from the ranked Map?
// But then how do we applay it again to get the best result?
// If house can be ignored, then it's pretty straight forward.
// Is house only the best option if you have 2 pair + one J?
// It seems so, let try it.
// if [2; 2] + J -> [2+J; 2] -> [3; 2] -> Full house.
// Else [x; _] + J -> [x+J; _]
// Well, that just means [x+J; _] is always the best option? Since the card you match does 
// not matter to the ranking.

let countCardsOfEachType2 string =
    let map = Map.empty |> Map.add '*' 0 |> Map.add '^' 0 // Need two for part 2, as all cards may be Jokers. Need to be zero as well in this case.
    let rankedMap = string |> Seq.fold (fun acc x -> incrementMap acc x) map
    let jokers = Map.tryFind 'J' rankedMap
    let mapWoJokers = Map.remove 'J' rankedMap
    let [topOne; topTwo] = mapWoJokers |> Map.values |> Seq.sortDescending |> Seq.take 2 |> Seq.toList
    match jokers with
    | Some(j) -> [topOne + j; topTwo]
    | None -> [topOne; topTwo]


let parseAndSortHands2 hands =
    hands
    //|> Seq.map handToSortableValues
    |> Seq.map (fun xx ->
        let x = fst xx
        let handType = countCardsOfEachType2 x |> getHandType
        //let sortableString = HandToSortableValues x
        (handType, snd xx, x)
        )
    |> Seq.sortByDescending (fun x -> 
        let (handType, bet, sortStr) = x
        (handType, handToSortableValues2 sortStr)
        )
    |> Seq.indexed

"T55J5" |> countCardsOfEachType2 |> getHandType
parseAndSortHands2 parsed2 |> Seq.toList
|> Seq.map (fun x ->
    let (index, (_, bet, _)) = x
    (index + 1) * bet
    )
|> Seq.fold (+) 0

//let [<Fact>] ``part one examples``() =
//    totalWins exampleData |> should equal 288L

//let [<Fact>] ``part one solution``() =
//    totalWins input |> should equal 1084752L

