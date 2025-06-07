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

        GenCommand.Implementation.cmd.parse [| "gen" |]
        |> GenCommand.Implementation.exe

        InOut.Out.lines[0].StartsWith("4") |> Assert.True

        InOut.Out.lines[1].StartsWith("5") |> Assert.True

        InOut.Out.all |> testLog output

    [<Fact>]
    let ``test evcm cmd and tpl cmd`` () =
        Directory.SetCurrentDirectory("Gen")
        File.OpenText "controller.cs" :> TextReader |> Console.SetIn

        GenCommand.Implementation.cmd.parse [| "gen" |]
        |> GenCommand.Implementation.exe

        ("[FromBody]", InOut.Out.lines[0]) |> Assert.Equal<string>
        ("[FromQuery]", InOut.Out.lines[0]) |> Assert.NotEqual<string>
        ("[FromBody] FirestoreDocDto dto", InOut.Out.lines[1]) |> Assert.Equal<string>

        InOut.Out.all |> testLog output
