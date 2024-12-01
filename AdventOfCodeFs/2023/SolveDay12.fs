#if COMPILED
module Year2023.Day12
#endif
open Xunit
open FsUnit.Xunit

let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input12.txt""")

let exampleData1 = seq {
        yield "???.### 1,1,3"
        yield ".??..??...?##. 1,1,3"
        yield "?#?#?#?#?#?#?#? 1,3,1,6"
        yield "????.#...#... 4,1,1"
        yield "????.######..#####. 1,6,5"
        yield "?###???????? 3,2,1"
    }


let rec generatePermutations (str: string) (index: int) (acc: string list) =
    if index = String.length str then
        acc
    else
        match str.[index] with
        | '?' ->
            let newAcc =
                acc
                |> List.collect (fun s ->
                    [s.Substring(0, index) + "#" + s.Substring(index + 1); s.Substring(0, index) + "." + s.Substring(index + 1)]
                )
            generatePermutations str (index + 1) newAcc
        | _ -> generatePermutations str (index + 1) acc

let getAllPermutations (input: string) =
    generatePermutations input 0 [input]

// Example usage:
let result = getAllPermutations "???..#"
printfn "%A" result

let checkEnd (input : string) index =
    match index with 
    | x when x = input.Length -> Some(index)
    | _ ->
        match input[index] with 
        | '#' -> None
        | _ -> Some(index)

let rec checkAgainstGroup (input : string) index groupCountLeft =
    match index with 
    | x when x >= input.Length -> None
    | _ ->
        match input[index] with 
        | '#' -> 
            match groupCountLeft with
            | 1 -> checkEnd input (index+1)
            | _ -> checkAgainstGroup input (index+1) (groupCountLeft-1)
        | _ -> None

checkEnd "###" 3
checkEnd "####" 3
checkEnd "###." 3
checkAgainstGroup "###" 0 3

checkAgainstGroup "####" 0 3
checkAgainstGroup "###." 0 3

let rec solveAux index currentGroup groupsLeft (input: string) =
    match index with
    | x when x >= input.Length -> if currentGroup = -1 then Some(1) else None // None // Don't think we should need this check: if currentGroup = -1 then Some(1) else None
    | _ ->
        match input[index] with 
        | '#' -> 
            let nextIndex = checkAgainstGroup input index currentGroup
            //printfn "%A, %A, %A, - %A"input currentGroup nextIndex groupsLeft
            match nextIndex with
            | Some newIndex ->
                match groupsLeft with
                | [] -> solveAux newIndex -1 [] input  //Some(1) // Here we exit, even if string has # left, which is wrong. We have to continue to make sure there are no more #.
                | head::tail -> solveAux newIndex head tail input 
            | None -> None
        | _ -> solveAux (index+1) currentGroup groupsLeft input

solveAux 0 3 [] "###" 
solveAux 0 3 [] "####" 
solveAux  0 3 [] "###."
solveAux  0 3 [1] "###.#"
solveAux  0 3 [2] "###.#"
solveAux  0 3 [1] "###.."
solveAux 0 3 [1] "###.#." 

solveAux 0 3 [2;1] "?###????????"
getAllPermutations "?###????????"

let checkAllPermutations input group =
    let permutations = getAllPermutations input
    let h = group |> List.head
    let t = group |> List.tail
    let prep = (solveAux 0 h t)
    permutations
    |> List.choose prep
    |> List.sum
    //|> List.iter (fun x ->
    //        if (prep x) = Some(1) then
    //            printfn "%A" x
    //        else () 
    //    )

checkAllPermutations ".??..??...?##." [1;1;3]
checkAllPermutations "?###????????" [3;2;1]
solveAux 0 3 [2;1] ".###.##.#.##" // Should fail as we have not reached the end of the string. There are more # but no capture groups.


let solve (input: string seq) =
    input
    |> Seq.map (fun x -> 
            let str = x.Split(' ')[0]
            let group = (x.Split(' ')[1]).Split(',') |> Array.toList |> List.map (int)
            checkAllPermutations str group
        )
    |> Seq.sum

//solve input

//let [<Fact>] ``part one examples``() =
//    solve exampleData1 |> should equal 21

//let [<Fact>] ``part one solution``() =
//    solve  input |> should equal 7653

// Part two

let test = "???.###????.###????.###????.###????.### 1,1,3,1,1,3,1,1,3,1,1,3,1,1,3"
let test2 = "???.### 1,1,3"
let str = test2.Split(' ')[0]
let group = (test2.Split(' ')[1]).Split(',') |> Array.toList |> List.map (int)

let repeatedStr = $"{str}?{str}?{str}?{str}?{str}"
let repeatedGroup = group |> List.replicate 5 |> List.concat
checkAllPermutations str group

let solve2 (input: string seq) =
    input
    |> Seq.map (fun x -> 
            let str = x.Split(' ')[0]
            let group = (x.Split(' ')[1]).Split(',') |> Array.toList |> List.map (int)
            
            let repeatedStr = $"{str}?{str}?{str}?{str}?{str}"
            let repeatedGroup = group |> List.replicate 5 |> List.concat
            let result = checkAllPermutations repeatedStr repeatedGroup
            printfn "%A: %A" x result
            result
        )
    |> Seq.sum

solve2 exampleData1
// First goes fast, rest never completes.
// Think we need to prune as we generate the permutations.

let rec permutationsWithPruning (str: string) (groups: int list) index currentGroupIndex (groupCount: int option) =
    if String.length str = index then
        if groups.Length <= currentGroupIndex || (groupCount.IsSome && (groups.Length - 1) = currentGroupIndex) then 
            1L
        else 
            0L
    else 
        match str[index] with
        | '.' -> handlePeriod str groups index currentGroupIndex groupCount
        | '#' -> handlePound str groups index currentGroupIndex groupCount
        | '?' ->
            let period = handlePeriod str groups index currentGroupIndex groupCount
            let pound = handlePound str groups index currentGroupIndex groupCount
            period + pound
        | any -> failwithf "invalud char %A in string %A" any str 

and handlePeriod (str: string) (groups: int list) index currentGroupIndex (groupCount: int option) =
    match groupCount with
    | Some activeGroupCount -> // End of a group.
        if groups[currentGroupIndex] = activeGroupCount then
            // New group index and reset count.
            // We increase currentGroupIndex, but since the last characters may not be #, we need to still be able to go out of bounds here.
            permutationsWithPruning str groups (index+1) (currentGroupIndex+1) None
        else
            0L // invalid count at end of group.
    // No active group, keep traversing.
    | None -> permutationsWithPruning str groups (index+1) currentGroupIndex None

and handlePound (str: string) (groups: int list) index currentGroupIndex (groupCount: int option) =
    //printfn "%A - %A - %A" index currentGroupIndex groupCount
    match groupCount with
    | Some activeGroupCount ->
        // Group count still less or equal to expected group size.
        if activeGroupCount < groups[currentGroupIndex] then
            permutationsWithPruning str groups (index+1) currentGroupIndex (Some (activeGroupCount+1))
        else
            0L // Size to big for the group.
    // Start a new count, but still on the set group.
    | None -> 
        // This is the first time we look at a new group. Since the group index may be out of bounds here,
        // we have to validate it.
        if currentGroupIndex < groups.Length then
            permutationsWithPruning str groups (index+1) currentGroupIndex (Some 1)
        else
            0L

let test22 = "???.### 1,1,3"
permutationsWithPruning "#.#.###" [1;1;3] 0 0 None
permutationsWithPruning "....###" [1;1;3] 0 0 None
permutationsWithPruning "#...###" [1;1;3] 0 0 None
permutationsWithPruning ".#..###" [1;1;3] 0 0 None
permutationsWithPruning "..#.###" [1;1;3] 0 0 None
permutationsWithPruning "###.###" [1;1;3] 0 0 None
permutationsWithPruning ".##.###" [1;1;3] 0 0 None
permutationsWithPruning "##..###" [1;1;3] 0 0 None

permutationsWithPruning "?...###" [1;1;3] 0 0 None
permutationsWithPruning "??..###" [1;1;3] 0 0 None
permutationsWithPruning "???.###" [1;1;3] 0 0 None

let test23 = "???.###????.###????.###????.###????.### 1,1,3,1,1,3,1,1,3,1,1,3,1,1,3"
permutationsWithPruning "???.###????.###????.###????.###????.###" [1;1;3;1;1;3;1;1;3;1;1;3;1;1;3] 0 0 None

let test33 = "?#?#?#?#?#?#?#? 1,3,1,6"
permutationsWithPruning "?#?#?#?#?#?#?#???#?#?#?#?#?#?#???#?#?#?#?#?#?#???#?#?#?#?#?#?#???#?#?#?#?#?#?#?" [1;3;1;6;1;3;1;6;1;3;1;6;1;3;1;6;1;3;1;6] 0 0 None

let test223 = "????.#...#... 4,1,1"

permutationsWithPruning "????.#...#...?????.#...#...?????.#...#...?????.#...#...?????.#...#..." [4;1;1;4;1;1;4;1;1;4;1;1;4;1;1] 0 0 None

permutationsWithPruning "?###??????????###??????????###??????????###??????????###????????" [3;2;1;3;2;1;3;2;1;3;2;1;3;2;1] 0 0 None

permutationsWithPruning "?###??????????###??????????###??????????###??????????###????????" [3;2;1;3;2;1;3;2;1;3;2;1;3;2;1] 0 0 None
// This is a factor 2,2 higher than expected. 1113750 vs 506250.
// So we are missing some condition or check here.
// We should be checking if there are more # than capture groups.
// But I think we are not checking that all groups have been captured when we reach the end of a string.
// So "....." [1] should validate.
permutationsWithPruning "...." [1] 0 0 None // 1 incorrect.
// And so it does.
permutationsWithPruning "..#." [1] 0 0 None // 1 correct.
permutationsWithPruning "#.#." [1] 0 0 None // 0 correct.

// That takes us down to 303750, which is instead 202500 less than expected...
// So either our check if all groups have been captured is to strict (berhaps off by one) or we have another issue.

// Now, I think the group index may match the length, or be one higher if the string ends with a '.'.
permutationsWithPruning "..#." [1] 0 0 None // 1 correct.
permutationsWithPruning "...#" [1] 0 0 None // 0 incorrect. So since we do not reach a ., then we are 1 behind on the group index.
// Lets check if we have an active catpure group and then allow for index to be one less then group length.
// That fixed that bug. But what about the full example data?

// 506250 to many now.
// So, when we reach string length, do we not validate the last character?
permutationsWithPruning ".###.##.##" [3;2;1] 0 0 None // 1 incorrect. 
permutationsWithPruning "...##" [1] 0 0 None // 1 incorrect.
// So... step one out of bounds? Or is it wrong to check if index equals string lenght?
// Ok, so we check if current count was less than or equal to current group. But as we increamented it after the check
// We needed to check for only less than.


let solve22 (input: string seq) =
    input
    |> Seq.map (fun x -> 
            let str = x.Split(' ')[0]
            let group = (x.Split(' ')[1]).Split(',') |> Array.toList |> List.map (int)
            
            let repeatedStr = $"{str}?{str}?{str}?{str}?{str}"
            let repeatedGroup = group |> List.replicate 5 |> List.concat
            let result = permutationsWithPruning repeatedStr repeatedGroup 0 0 None
            //printfn "%A: %A" x result
            result
        )
    |> Seq.sum

//solve22 exampleData1

//solve22 input



//solve22 (["..????##?????.????? 6,2,1"] |> List.toSeq)
let issueString = "..????##?????.??????..????##?????.??????..????##?????.??????..????##?????.??????..????##?????.?????"
let issueGroup = [6;2;1;6;2;1;6;2;1;6;2;1;6;2;1;]
issueString |> Seq.filter (fun x -> x = '?' || x = '#') |> Seq.length
[6;2;1;6;2;1;6;2;1;6;2;1;6;2;1] |> List.sum
issueGroup |> List.length // -1


let [<Fact>] ``part two solution``() =
    solve22 input |> should equal -1



let rec permutationsWithPruning2 (str: string) (groups: int list) index currentGroupIndex (groupCount: int option) hashNeeded periodsNeeded remainingHashOptions  =
    if String.length str = index then
        if groups.Length <= currentGroupIndex || (groupCount.IsSome && (groups.Length - 1) = currentGroupIndex) then 
            1L
        else 
            0L
    else if String.length str < (hashNeeded + periodsNeeded) then
        0
    else 
        match str[index] with
        | '.' -> handlePeriod2 str groups index currentGroupIndex groupCount hashNeeded periodsNeeded remainingHashOptions
        | '#' -> handlePound2 str groups index currentGroupIndex groupCount hashNeeded periodsNeeded (remainingHashOptions-1)
        | '?' ->
            let period = handlePeriod2 str groups index currentGroupIndex groupCount hashNeeded periodsNeeded (remainingHashOptions-1)
            let pound = handlePound2 str groups index currentGroupIndex groupCount hashNeeded periodsNeeded (remainingHashOptions-1)
            period + pound
        | any -> failwithf "invalud char %A in string %A" any str 

and handlePeriod2 (str: string) (groups: int list) index currentGroupIndex (groupCount: int option) hashNeeded periodsNeeded remainingHashOptions =
    match groupCount with
    | Some activeGroupCount -> // End of a group.
        if groups[currentGroupIndex] = activeGroupCount then
            // New group index and reset count.
            // We increase currentGroupIndex, but since the last characters may not be #, we need to still be able to go out of bounds here.
            permutationsWithPruning2 str groups (index+1) (currentGroupIndex+1) None hashNeeded (periodsNeeded-1) remainingHashOptions
        else
            0L // invalid count at end of group.
    // No active group, keep traversing.
    | None -> permutationsWithPruning2 str groups (index+1) currentGroupIndex None hashNeeded periodsNeeded remainingHashOptions

and handlePound2 (str: string) (groups: int list) index currentGroupIndex (groupCount: int option) hashNeeded periodsNeeded remainingHashOptions =
    //printfn "%A - %A - %A" index currentGroupIndex groupCount
    match groupCount with
    | Some activeGroupCount ->
        // Group count still less or equal to expected group size.
        if activeGroupCount < groups[currentGroupIndex] then
            permutationsWithPruning2 str groups (index+1) currentGroupIndex (Some (activeGroupCount+1)) (hashNeeded-1) periodsNeeded remainingHashOptions
        else
            0L // Size to big for the group.
    // Start a new count, but still on the set group.
    | None -> 
        // This is the first time we look at a new group. Since the group index may be out of bounds here,
        // we have to validate it.
        if currentGroupIndex < groups.Length then
            permutationsWithPruning2 str groups (index+1) currentGroupIndex (Some 1) (hashNeeded-1) periodsNeeded remainingHashOptions
        else
            0L


let solve23 (input: string seq) =
    input
    |> Seq.map (fun x -> 
            let str = x.Split(' ')[0]
            let group = (x.Split(' ')[1]).Split(',') |> Array.toList |> List.map (int)
            
            let repeatedStr = $"{str}?{str}?{str}?{str}?{str}"
            let repeatedGroup = group |> List.replicate 5 |> List.concat
            let hashesNeeded = repeatedGroup |> List.sum
            let periodsNeeded = issueGroup.Length - 1
            let remainingHashOptions = issueString |> Seq.filter (fun x -> x = '?' || x = '#') |> Seq.length
            let result = permutationsWithPruning2 repeatedStr repeatedGroup 0 0 None hashesNeeded periodsNeeded remainingHashOptions
            printfn "%A: %A" x result
            result
        )
    |> Seq.sum

solve23 exampleData1

solve23 input



//solve22 (["..????##?????.????? 6,2,1"] |> List.toSeq)
let issueString = "..????##?????.??????..????##?????.??????..????##?????.??????..????##?????.??????..????##?????.?????"
let issueGroup = [6;2;1;6;2;1;6;2;1;6;2;1;6;2;1;]
issueString |> Seq.filter (fun x -> x = '?' || x = '#') |> Seq.length
[6;2;1;6;2;1;6;2;1;6;2;1;6;2;1] |> List.sum
issueGroup |> List.length // -1

permutationsWithPruning2 "????.#...#...?????.#...#...?????.#...#...?????.#...#...?????.#...#..." [4;1;1;4;1;1;4;1;1;4;1;1;4;1;1] 0 0 None

permutationsWithPruning "?###??????????###??????????###??????????###??????????###????????" [3;2;1;3;2;1;3;2;1;3;2;1;3;2;1] 0 0 None

permutationsWithPruning "?###??????????###??????????###??????????###??????????###????????" [3;2;1;3;2;1;3;2;1;3;2;1;3;2;1] 0 0 None
// This is a factor 2,2 higher than expected. 1113750 vs 506250.
// So we are missing some condition or check here.
// We should be checking if there are more # than capture groups.
// But I think we are not checking that all groups have been captured when we reach the end of a string.
// So "....." [1] should validate.
permutationsWithPruning "...." [1] 0 0 None // 1 incorrect.
// And so it does.
permutationsWithPruning "..#." [1] 0 0 None // 1 correct.
permutationsWithPruning "#.#." [1] 0 0 None // 0 correct.

// That takes us down to 303750, which is instead 202500 less than expected...
// So either our check if all groups have been captured is to strict (berhaps off by one) or we have another issue.

// Now, I think the group index may match the length, or be one higher if the string ends with a '.'.
permutationsWithPruning "..#." [1] 0 0 None // 1 correct.
permutationsWithPruning "...#" [1] 0 0 None // 0 incorrect. So since we do not reach a ., then we are 1 behind on the group index.
// Lets check if we have an active catpure group and then allow for index to be one less then group length.
// That fixed that bug. But what about the full example data?

// 506250 to many now.
// So, when we reach string length, do we not validate the last character?
permutationsWithPruning ".###.##.##" [3;2;1] 0 0 None // 1 incorrect. 
permutationsWithPruning "...##" [1] 0 0 None // 1 incorrect.
// So... step one out of bounds? Or is it wrong to check if index equals string lenght?
// Ok, so we check if current count was less than or equal to current group. But as we increamented it after the check
// We needed to check for only less than.
