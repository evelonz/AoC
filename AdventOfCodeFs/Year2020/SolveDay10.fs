#if COMPILED
module Year2020.Day10
#endif

//open Xunit
//open FsUnit.Xunit
//open System.Collections.Generic

//let example1 = [|16;10;15;5;1;11;7;19;6;12;4;|]
//let example2 = [|28; 33; 18; 42; 31; 14; 46; 20; 48; 47; 24; 23; 49; 45; 19; 38; 39; 11; 1; 32; 25; 35; 8; 17; 7; 9; 4; 2; 34; 10; 3;|]

//let getJoltDifferences input =
//    input |> Seq.sort |> Seq.pairwise |> Seq.map (fun adapter -> snd adapter - fst adapter) |> Seq.countBy (fun joltDiff -> joltDiff)

//let getOneAndThreeDiffs input =
//    input 
//    |> Seq.filter (fun x -> fst x = 1 || fst x = 3) 
//    |> Seq.map (fun x -> if fst x = 3 then snd x+1 else snd x) // Add +1 to 3, since the built in adapter adds one +3 jolt link, and it is not in the list.

//let solution input =
//    input |> Seq.append (Seq.singleton 0)
//    |> getJoltDifferences
//    |> getOneAndThreeDiffs
//    |> Seq.fold (fun x y ->  x *  y) 1

//type TraverselType = Branch | Main

//let rec travel current (cache: Dictionary<int, int64>) travelType (input: list<int>)  =
//    match input with
//    | [] -> match travelType with
//        | Branch -> 1L
//        | Main -> 0L
//    | head :: tail ->
//        match (head - current) with
//        | 1|2|3 ->
//            let mutable hits = 0L;
//            match cache.TryGetValue head with
//            | true, value -> hits <- hits + value
//            | _ ->
//                // Traverse the new node and check it's brances recursivly. It is only here we set the hits to +1 when we reach the end of the list.
//                let newHits = travel head cache Branch tail 
//                cache.Add(head, newHits);
//                hits <- hits + newHits
//            // This is the recursive call to keep checking for possible matches in a distance of 1-3 Jolts.
//            hits + travel current cache Main tail
//        | _ -> 0L

//let solution2 input =
//    let cache = new Dictionary<int, int64>()
//    let res = input |> Seq.sort |> Seq.toList |> travel 0 cache Main
//    res

//[<Fact>]
//let ``solve examples 1``() =
//    let first = solution example1
//    first |> should equal 35

//[<Fact>]
//let ``solve examples 1 part two``() =
//    let first = solution2 example1
//    first |> should equal 8L

//[<Fact>]
//let ``solve examples 2``() =
//    let first = solution example2
//    first |> should equal 220

//[<Fact>]
//let ``solve examples 2 part two``() =
//    let first = solution2 example2
//    first |> should equal 19208L

//let [<Fact>] ``solved part one``() =
//    let data = Utility.loadPussleData __SOURCE_DIRECTORY__ "10"
//    let first = solution (data |> Seq.map int)
//    first |> should equal 2380

//let [<Fact>] ``solved part two``() =
//    let data = Utility.loadPussleData __SOURCE_DIRECTORY__ "10"
//    let first = solution2 (data |> Seq.map int)
//    first |> should equal 48358655787008L