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
    let ``test normal split`` () =
        Console.SetIn (new StringReader ("192.168.0.255") :> TextReader)

        Split.cmd.parse [| "split" ; "[.]" |] |> Split.exe

        helper.WriteLine $"{InOut.Out.all}"
