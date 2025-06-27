module XTests.Codebase.Exts.Tests

open Regl.CommandLine.Types
open Regl.CommandLine.Types.FlagsAndParams
open XTests.Shared
open XTests.Types
open Xunit
open Xunit.Abstractions
open Regl

type Tests (helper : ITestOutputHelper) =
    inherit TestBase (helper)

    [<Fact>]
    let ``test powerset`` () =
        let power = Exts.powerset [ 1 ; 2 ; 3 ; 4 ]

        power
        |> List.iter (fun l ->
            match l with
            | [] -> helper.WriteLine "[]"
            | l ->
                l
                |> List.map (fun e -> $"{e}")
                |> List.reduce (fun a b -> $"{a}; {b}")
                |> fun str -> $"[{str}]"
                |> testLog helper
                |> ignore)

        (16, power.Length) |> Assert.Equal

    [<Fact>]
    let ``test powerset on flags`` () =
        let f = BoolFlag "-f" :> IFlag
        let d = BoolFlag "-d" :> IFlag
        let R = BoolFlag "-R" :> IFlag
        let pattern = StringFlag "--pattern" :> IFlag
        let power = Exts.powerset [ f ; d ; R ; pattern ]

        power
        |> List.iter (fun l ->
            match l with
            | [] -> helper.WriteLine "[]"
            | l ->
                l
                |> List.map (fun e -> $"{e}")
                |> List.reduce (fun a b -> $"{a}; {b}")
                |> fun str -> $"[{str}]"
                |> testLog helper
                |> ignore)

        (16, power.Length) |> Assert.Equal
