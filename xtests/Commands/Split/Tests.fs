module XTests.Commands.Split.Tests

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

    [<Fact>]
    let ``/bin/bash test split and echo`` () =
        let cmd =
            $"#!/bin/bash
{reglPathInCmd}
for e in $(echo 192.168.0.255 | regl split [.]);
    do
        echo $e
    done"

        File.WriteAllText ("tmp.sh", cmd)
        let result = doShell "tmp.sh"
        (result.code, 0) |> Assert.Equal
        ("192", result.lines[0]) |> Assert.Equal
        ("168", result.lines[1]) |> Assert.Equal
        ("0", result.lines[2]) |> Assert.Equal
        ("255", result.lines[3]) |> Assert.Equal
