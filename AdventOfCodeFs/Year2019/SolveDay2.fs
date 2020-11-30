#if COMPILED
module Day2
#endif

open Xunit
open FsUnit.Xunit

type ComputerState =
    | Running
    | Halted

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

type Position = 
    { 
        inp1 : int 
        inp2 : int
        outp : int
    }

let getPositions (input:int[]) pointer = { inp1 = input.[pointer+1]; inp2 = input.[pointer+2]; outp = input.[pointer+3] }

let store (input:int[]) position value  = input.[position.outp] <- value

let operate (input:int[]) op position = op input.[position.inp1] input.[position.inp2]

let binaryExprOperation (input:int[]) pointer (op:int -> int -> int) = 
    let pos = getPositions input pointer
    operate input op pos 
    |> store input pos 

let add (input:int[]) pointer =
    let adder val1 val2 = val1 + val2
    binaryExprOperation input pointer adder
    Running

let mutliply (input:int[]) pointer =
    let multiplyer val1 val2 = val1 * val2
    binaryExprOperation input pointer multiplyer
    Running

let noOp (input:int[]) pointer = Halted

let getOpcodeMethod op =
    match op with 
    | Add -> add
    | Multiply -> mutliply
    | Halt -> noOp
    | Error -> noOp

let RunCode (input:int[]) pointer = 
    let code = mapOpcode input.[pointer] |> getOpcodeMethod
    let state = code input pointer
    (input, pointer+4, state)

let rec RunCodeGetInstructions input pointer = 
    let result = RunCode input pointer
    match result with
    |  (_, _, Halted) -> input
    |  (_, newPointer, Running) -> RunCodeGetInstructions input newPointer


let symbolTestData = 
    [ [|1;0;0;0;99|], [|2; 0; 0; 0; 99|]
      [|2;3;0;3;99|], [|2;3;0;6;99|]
      [|2;4;4;5;99;0|],           [|2;4;4;5;99;9801|]
      [|1;1;1;4;99;5;6;0;99|], [|30;1;1;4;2;5;6;0;99|]
    ]
    |> Seq.map (fun (a,b) -> [|a; b|])

[<Theory>]
[<MemberData("symbolTestData")>]
let ``examples should calcualte correctly``(input:int[], expected:int[]) =
    let result = RunCodeGetInstructions input 0
    result |> should equal expected

let private pussleInput = [|1;0;0;3;1;1;2;3;1;3;4;3;1;5;0;3;2;1;6;19;1;19;6;23;2;23;6;27;2;6;27;31;2;13;31;35;1;9;35;39;2;10;39;43;1;6;43;47;1;13;47;51;2;6;51;55;2;55;6;59;1;59;5;63;2;9;63;67;1;5;67;71;2;10;71;75;1;6;75;79;1;79;5;83;2;83;10;87;1;9;87;91;1;5;91;95;1;95;6;99;2;10;99;103;1;5;103;107;1;107;6;111;1;5;111;115;2;115;6;119;1;119;6;123;1;123;10;127;1;127;13;131;1;131;2;135;1;135;5;0;99;2;14;0;0|]

let [<Fact>] ``part one with gives correct result``() =
    pussleInput.[1] <- 12
    pussleInput.[2] <- 2
    let result = RunCodeGetInstructions pussleInput 0
    result.[0] |> should equal 3224742

let [<Fact>] ``part two with gives correct result``() =
    let inputCopy = Array.copy pussleInput
    pussleInput.[1] <- 12
    pussleInput.[2] <- 2
    let result = RunCodeGetInstructions inputCopy 0
    result.[0] |> should equal 3224742
