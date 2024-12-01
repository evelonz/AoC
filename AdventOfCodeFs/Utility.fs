module Utility

open System.IO

let loadPussleData baseDirectory pussleNumber = 
    let filePath = "Files\\" + pussleNumber + ".txt"
    let fullPath = Path.Combine(baseDirectory, filePath)
    System.IO.File.ReadLines(fullPath)

let rec gcd (a:int64) (b:int64) =
    if b = 0
        then abs a
    else gcd b (a % b)

let lcmSimple a b = a*b/(gcd a b)

let rec lcm = function
    | [a;b] -> lcmSimple a b
    | head::tail -> lcmSimple (head) (lcm (tail))
    | [] -> 1

let splits list = 
  let rec splitsAux acc list =
    match list with 
    | x::xs -> splitsAux ((x, xs)::acc) xs
    | _ -> acc |> List.rev
  splitsAux [] list

let getAll2ElementCombinations inputList =
    [ for x, xs in splits inputList do
        for y in xs do yield x, y ]