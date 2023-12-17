#if COMPILED
module Year2023.Day14
#endif
open Xunit
open FsUnit.Xunit

let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input14.txt""")

let exampleData1 = seq {
        yield "O....#...."
        yield "O.OO#....#"
        yield ".....##..."
        yield "OO.#O....O"
        yield ".O.....O#."
        yield "O.#..O.#.#"
        yield "..O..#O..O"
        yield ".......O.."
        yield "#....###.."
        yield "#OO..#...."
    }

let transpose (llst: 'a list list) : 'a list list = 
    List.map (List.map List.head) (List.init llst.Head.Length (fun x -> List.fold (fun x f -> f x) llst (List.init x (fun x -> (List.map List.tail)))))

let transposeInput (input: string list) =
    let temp = input |> List.map (fun x -> x |> Seq.toList)
    transpose temp |> List.map (fun x -> System.String.Concat(Array.ofList(x)))

let singleRow max inp = 
    inp
    |> Seq.indexed
    |> Seq.fold (fun acc (index, cur) -> 
        if cur = '#' then
            (fst acc, (max - index - 1))
        else if cur = 'O' then
            (fst acc + snd acc , snd acc - 1)
        else
            acc
        ) (0, max)

let solve input =
    let transposed = transposeInput (input |> Seq.toList )
    let rowsToSouthPlatEdge = transposed[0].Length
    transposed
    |> List.map (singleRow rowsToSouthPlatEdge)
    |> List.map fst
    |> List.sum

let [<Fact>] ``part one examples``() =
    solve exampleData1 |> should equal 136

let [<Fact>] ``part one solution``() =
    solve  input |> should equal 108641

