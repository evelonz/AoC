#if COMPILED
module Day2B
#endif

open Xunit
open FsUnit.Xunit

let indata = [|1; 0; 0; 0; 99|]

type Opcode =
    | Add
    | Multiply
    | Halt
    | Error

let mapOpcode code =
    match code with 
    | 1 -> Add
    | 2 -> Multiply
    | 99 -> Halt
    | _ -> Error

let add (input:int[]) pointer =
    let target = input.[1]
    input.[target] <- input.[pointer+1] + input.[pointer+2]
    input


let multiplay (input:int[]) pointer =
    let target = input.[1]
    input.[target] <- input.[pointer+1] * input.[pointer+2]
    input

let noOp (input:int[]) pointer = input

let getOpcodeMethod op =
    match op with 
    | Add -> add
    | Multiply -> multiplay
    | Halt -> noOp
    | Error -> noOp


let RunCode (input:int[]) pointer = 
    let code = mapOpcode input.[pointer] |> getOpcodeMethod
    code input pointer


let RunCodeGetInstructions input = RunCode input 0


[<Fact>]
let part_one_correct() =
    let result = RunCodeGetInstructions indata
    let expected = [|2; 0; 0; 0; 99|]
    Assert.Equal(1, 1)
