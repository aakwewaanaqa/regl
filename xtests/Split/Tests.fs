module XTests.Split.Tests

open System
open System.IO
open Regl.CommandLine.Commands
open Regl.CommandLine.IO
open XTests.Shared
open Xunit
open Xunit.Abstractions

type Tests (helper : ITestOutputHelper) =
    [<Fact>]
    let ``test split`` () =
        Console.SetIn (new StringReader ("192.168.0.255") :> TextReader)
        Split.cmd.parse [ "[.]" ] |> Split.exe
        ("192", InOut.Out.lines[0]) |> Assert.Equal
        ("168", InOut.Out.lines[1]) |> Assert.Equal
        ("0", InOut.Out.lines[2]) |> Assert.Equal
        ("255", InOut.Out.lines[3]) |> Assert.Equal
