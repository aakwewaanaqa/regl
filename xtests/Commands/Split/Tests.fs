module XTests.Commands.Split.Tests

open System
open System.IO
open Regl.CommandLine.Commands
open Regl.CommandLine.IO
open XTests.Shared
open XTests.Types
open Xunit
open Xunit.Abstractions

type Tests (helper : ITestOutputHelper) =
    inherit TestBase (helper)

    [<Fact>]
    let ``test split`` () =
        setIn "192.168.0.255"
        Split.cmd.parse [ "[.]" ] |> Split.exe
        ("192", InOut.Out.lines[0]) |> Assert.Equal
        ("168", InOut.Out.lines[1]) |> Assert.Equal
        ("0", InOut.Out.lines[2]) |> Assert.Equal
        ("255", InOut.Out.lines[3]) |> Assert.Equal

    [<Fact>]
    let ``test split --quote`` () =
        setIn "192.168.0.255"
        Split.cmd.parse [ "[.]" ; "--quote" ] |> Split.exe
        ("\"192\"", InOut.Out.lines[0]) |> Assert.Equal
        ("\"168\"", InOut.Out.lines[1]) |> Assert.Equal
        ("\"0\"", InOut.Out.lines[2]) |> Assert.Equal
        ("\"255\"", InOut.Out.lines[3]) |> Assert.Equal
