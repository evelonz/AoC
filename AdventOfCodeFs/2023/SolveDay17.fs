#if COMPILED
module Year2023.Day17
#endif
open Xunit
open FsUnit.Xunit
open System.Collections.Generic

let input = System.IO.File.ReadLines($"""{__SOURCE_DIRECTORY__}\input17.txt""")

let exampleData1 = seq {
        yield "2413432311323"
        yield "3215453535623"
        yield "3255245654254"
        yield "3446585845452"
        yield "4546657867536"
        yield "1438598798454"
        yield "4457876987766"
        yield "3637877979653"
        yield "4654967986887"
        yield "4564679986453"
        yield "1224686865563"
        yield "2546548887735"
        yield "4322674655533"
    }

type Node = { X: int; Y: int; Cost: int }
type Direction = { DX: int; DY: int }

let directions = [
    { DX = 0; DY = 1 };
    { DX = 0; DY = -1 };
    { DX = 1; DY = 0 };
    { DX = -1; DY = 0 }
]

let isValid (x, y) (rows, cols) =
    x >= 0 && x < rows && y >= 0 && y < cols

let bfsWithStepLimit (matrix: int[,]) start =
    let rows = matrix.GetLength(0)
    let cols = matrix.GetLength(1)
    let visited = Array2D.init rows cols (fun _ _ -> Array2D.create 4 4 false)
    let queue = Queue<Node>()

    let enqueueNode node =
        queue.Enqueue(node)

    let exploreNode node =
        directions
        |> List.iter (fun dir ->
            for step = 1 to 3 do
                let newX = node.X + dir.DX * step
                let newY = node.Y + dir.DY * step
                if isValid (newX, newY) (rows, cols) then
                    if not visited.[newX, newY].[step, dir.DX + 1] then
                        visited.[newX, newY].[step, dir.DX + 1] <- true
                        let newCost = node.Cost + matrix.[newX, newY]
                        let newNode = { X = newX; Y = newY; Cost = newCost }
                        enqueueNode newNode
                else
                    ()
        )

    enqueueNode start

    while queue.Count > 0 do
        let current = queue.Dequeue()
        exploreNode current

    visited

let matrix = 
    Array2D.init 3 4 (fun i j ->
        match (i, j) with
        | 0, 0 -> 1
        | 0, 1 -> 2
        | 0, 2 -> 3
        | 0, 3 -> 4
        | 1, 0 -> 5
        | 1, 1 -> 6
        | 1, 2 -> 7
        | 1, 3 -> 8
        | 2, 0 -> 9
        | 2, 1 -> 10
        | 2, 2 -> 11
        | 2, 3 -> 12
        | _, _ -> failwith "Invalid index"
    )

let startNode = { X = 0; Y = 0; Cost = matrix.[0, 0] }

let result = bfsWithStepLimit matrix startNode

for i = 0 to matrix.GetLength(0) - 1 do
    for j = 0 to matrix.GetLength(1) - 1 do
        printf "Node (%d, %d): " i j
        //for step = 1 to 3 do
        //    for dir = 1 to 4 do
        printf "%A " result.[i, j]//.[step, dir]
        printfn ""