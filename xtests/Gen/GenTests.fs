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
        Directory.SetCurrentDirectory("Gen")
        File.OpenText "genesis.txt" :> TextReader |> Console.SetIn

        GenCommand.Implementation.cmd.parse [| "regl"; "gen" |]
        |> GenCommand.Implementation.exe

        InOut.Out.all |> testLog output

    [<Fact>]
    let ``test evcm cmd`` () =
        Directory.SetCurrentDirectory("Gen")
        File.OpenText "controller.cs" :> TextReader |> Console.SetIn

        GenCommand.Implementation.cmd.parse [| "regl"; "gen" |]
        |> GenCommand.Implementation.exe

        Environment.GetEnvironmentVariable("TResult")
        |> fun var -> Assert.Equal("object", var)

        InOut.Out.all
        |> testLog output
        |> fun txt -> File.WriteAllText("test_evcm_cmd.txt", txt)