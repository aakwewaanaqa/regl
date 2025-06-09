module XTests.Commands.Gen.Tpl.Tests

open System.IO
open Xunit
open Xunit.Abstractions
open Regl.CommandLine.Commands.GenCommand
open Regl.CommandLine.IO
open XTests.Shared

type Tests (helper : ITestOutputHelper) =
    [<Fact>]
    let ``test tpl`` () =
        cd "Commands/Gen/Tpl"
        setIn (File.ReadAllText("controller.cs"))
        Implementation.cmd.parse [ "gen" ] |> Implementation.exe

        ("[FromBody]", InOut.Out.lines[0]) |> Assert.Equal<string>
        ("[FromQuery]", InOut.Out.lines[0]) |> Assert.NotEqual<string>
        ("[FromBody] FirestoreDocDto dto", InOut.Out.lines[1]) |> Assert.Equal<string>
