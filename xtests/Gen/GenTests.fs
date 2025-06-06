module XTests.Gen.GenTests

open System
open System.IO
open Xunit
open Xunit.Abstractions
open XTests.Shared
open Regl.CommandLine.IO
open Regl.CommandLine.Commands

type GenTestsImpl(output: ITestOutputHelper) =
    [<Fact>]
    let ``test copy cmd`` () =
        File.OpenText "Gen/genesis.txt" :> TextReader |> Console.SetIn

        GenCommand.Implementation.cmd.parse [| "regl"; "gen" |]
        |> GenCommand.Implementation.exe

        InOut.Out.all |> testLog output
