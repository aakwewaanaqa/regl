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
        
    [<Fact>]
    let ``test split to array`` () =
        Console.SetIn (new StringReader ("192.168.0.255") :> TextReader)

        Split.cmd.parse [| "split" ; "[.]"; "--array" |] |> Split.exe

        helper.WriteLine $"{InOut.Out.all}"
        
        let buffer = LinesBuffer(ByNone)
        buffer.appendLine "#!/bin/bash"
        buffer.appendLine "array=$(echo 192.168.0.255 | regl split [.] --array)"
        buffer.appendLine "echo ${array[3]}"
        let output = buffer.executeInBash()
        
        ("255", output) |> Assert.Equal
