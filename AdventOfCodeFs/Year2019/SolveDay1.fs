﻿#if COMPILED
module AoC2
#endif

// Shared
let indata = [109024; 137172; 80445; 80044; 107913; 108989; 59638; 120780; 139262; 139395; 56534; 129398; 101732; 101212; 142352; 123971; 75207; 121384; 145719; 66925; 71782; 102129; 83220; 147045; 99092; 132909; 114504; 141549; 99552; 128422; 134505; 58295; 142325; 107896; 66181; 86080; 71393; 58839; 143851; 149540; 108206; 68353; 123196; 61256; 83896; 122756; 133066; 138085; 129872; 63965; 105520; 141513; 90305; 92651; 113284; 66895; 72068; 144011; 82963; 136919; 111222; 54134; 82662; 107612; 87366; 131791; 144708; 116894; 142784; 52299; 138414; 56330; 80146; 73422; 60308; 125678; 95910; 116374; 136257; 100387; 114960; 62651; 102946; 56912; 91399; 146005; 147222; 125962; 129805; 101208; 67998; 85297; 117332; 101967; 94339; 130878; 79396; 140312; 147746; 136975]

let fuelForMass m = (m / 3 - 2)

// Part 1
let resPart1 = indata |> List.map fuelForMass |> List.sum

// Part 2

// Recursive
let (|NeedMoreFuel|WishingReallyHard|) input = if input > 0 then NeedMoreFuel else WishingReallyHard

let rec sumFuelForFuel fuel =
    match fuel with
    | NeedMoreFuel -> sumFuelForFuel (fuelForMass fuel) + fuel
    | WishingReallyHard -> 0

let fuelForModule = fuelForMass >> sumFuelForFuel

let res2Rec = indata |> List.map fuelForModule |> List.sum

// tail recursive
let totalFuelForMass moduelWeigth =
   let rec fuelForFuel fuel acc =
       match fuel with
       | NeedMoreFuel -> fuelForFuel (fuelForMass fuel) (acc + fuel)
       | WishingReallyHard -> acc
   fuelForFuel (fuelForMass moduelWeigth) 0

let res2TailRec = indata |> List.map totalFuelForMass |> List.sum

