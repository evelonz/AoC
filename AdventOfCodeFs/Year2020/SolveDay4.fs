#if COMPILED
module Year2020.Day4
#endif

open Xunit
open FsUnit.Xunit

// Shared
let exampleData = [
    "ecl:gry pid:860033327 eyr:2020 hcl:#fffffd"; // OK
    "byr:1937 iyr:2017 cid:147 hgt:183cm";
    "";
    "iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884"; // Missing hgt
    "hcl:#cfa07d byr:1929";
    "";
    "hcl:#ae17e1 iyr:2013"; // OK
    "eyr:2024";
    "ecl:brn pid:760753108 byr:1931";
    "hgt:179cm";
    "";
    "hcl:#cfa07d eyr:2025 pid:166559648"; // Missing byr
    "iyr:2011 ecl:brn hgt:59in";
    ]

let exampleDataValid = [
        "pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980";
        "hcl:#623a2f";
        "";
        "eyr:2029 ecl:blu cid:129 byr:1989";
        "iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm";
        "";
        "hcl:#888785";
        "hgt:164cm byr:2001 iyr:2015 cid:88";
        "pid:545766238 ecl:hzl";
        "eyr:2022";
        "";
        "iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719";
    ]

let exampleDataInvalid = [
    "eyr:1972 cid:100";
    "hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926";
    "";
    "iyr:2019";
    "hcl:#602927 eyr:1967 hgt:170cm";
    "ecl:grn pid:012533040 byr:1946";
    "";
    "hcl:dab227 iyr:2012";
    "ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277";
    "";
    "hgt:59cm ecl:zzz";
    "eyr:2038 hcl:74454a iyr:2023";
    "pid:3556412378 byr:2007";
    ]

let fields = [
    "byr";
    "iyr";
    "eyr";
    "hgt";
    "hcl";
    "ecl";
    "pid";
    ]

let getAllFields (passportRows: List<string>) = seq {
    let mutable record = []
    for line in passportRows do
        if (line.Length = 0) then
            yield record |> Map.ofList
            record <- []
        else if record.Length > 0 then 
            record <- record @  (line.Split(' ') |> Seq.map (fun x -> x.Split(':')) |> Seq.map (fun x -> x.[0], x.[1]) |> Seq.toList)
        else
            record <- line.Split(' ') |> Seq.map (fun x -> x.Split(':')) |> Seq.map (fun x -> x.[0], x.[1]) |> Seq.toList
        }

let inRange min max value =
    let inline (.<=) (leftResult, middle) right = leftResult && (middle <= right)
    let inline (<=.) left middle = (left <= middle, middle)
    min <=. value .<= max

let validateRecord input =
    match input with
    | ("byr", value) -> value |> int |> inRange 1920 2002
    | ("iyr", value) -> value |> int |> inRange 2010 2020
    | ("eyr", value) -> value |> int |> inRange 2020 2030
    | ("hgt", value: string) -> 
        let unit = value.[^1..]
        match unit with
        | "cm" -> value.[0..^2] |> int |> inRange 150 193
        | "in" -> value.[0..^2] |> int |> inRange 59 76
        | _ -> false
    | ("hcl", value) -> 
        let a = value.Length = 7 
        let b = value.[0] = '#'
        let e = value.[1..]
        let c = System.Text.RegularExpressions.Regex.IsMatch(value.[1..], "^[0-9a-fA-F]+$")
        let cc = 1
        a && b && c
    | ("ecl", value) -> List.contains value ["amb"; "blu"; "brn"; "gry"; "grn"; "hzl"; "oth";]
    | ("pid", value) -> value.Length = 9 && value |> Seq.forall System.Char.IsDigit
    | ("cid", _) -> true
    | _ -> false

let requiredFieldsPresent passportRecord =
    let mutable valid = true;
    for field in fields do
        if Seq.contains field passportRecord then () else valid <- false
    valid

let hasRequiredFields input =
    let c = input |> Seq.map (fun s -> fst s) |> requiredFieldsPresent
    match c with
    | true -> Some input
    | false -> None

let isRecordValid input = 
    match input with
    | Some record -> record |> Seq.map validateRecord |> Seq.contains false |> not
    | None -> false

let getRecordOptions input =
    input |> (fun x -> seq { yield! x; yield "" }) |> Seq.toList |> getAllFields |> Seq.map Map.toSeq |> Seq.map hasRequiredFields

let solution input = 
    let recordOptions = input |> getRecordOptions
    let answer1 = recordOptions |> Seq.map Option.count |> Seq.sum
    let answer2 = recordOptions |> Seq.filter isRecordValid |> Seq.length
    (answer1, answer2)

let [<Fact>] ``solve examples``() =
    let (first, second) = solution exampleData
    first |> should equal 2
    second |> should equal 2

let [<Fact>] ``solve examples 2``() =
    let (first, second) = solution exampleDataInvalid
    first |> should equal 4
    second |> should equal 0

let [<Fact>] ``solve examples 3``() =
    let (first, second) = solution exampleDataValid
    first |> should equal 4
    second |> should equal 4

let [<Fact>] ``pussle solved``() =
    let data = Utility.loadPussleData __SOURCE_DIRECTORY__ "4"
    let (first, second) = solution data
    first |> should equal 222
    second |> should equal 140

