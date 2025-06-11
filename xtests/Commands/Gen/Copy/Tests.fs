module XTests.Commands.Gen.Copy.Tests

open System
open System.IO
open Regl.CommandLine.Commands.GenCommand
open Regl.CommandLine.IO
open XTests.Shared
open Xunit
open Xunit.Abstractions

type Tests (helper : ITestOutputHelper) =
    [<Fact>]
    let ``test copy by line`` () =
        let sourceFile =
            "//#!
1. Building a house?
    i.  On sand
    ii. On rock
//#!copy 3
2. Build the house.
    i.  By listening
    ii. By actions"

        setIn sourceFile
        Implementation.cmd.parse [] |> Implementation.exe

        (3, InOut.Out.length) |> Assert.Equal
        ("2. Build the house.", InOut.Out.lines[0]) |> Assert.Equal
        ("    i.  By listening", InOut.Out.lines[1]) |> Assert.Equal
        ("    ii. By actions", InOut.Out.lines[2]) |> Assert.Equal

    [<Fact>]
    let ``test copy by --start and --end`` () =
        let sourceFile =
            "//#!
//#!copy --start
1. Building a house?
    i.  On sand
    ii. On rock
//#!copy --end
2. Build the house.
    i.  By listening
    ii. By actions"

        setIn sourceFile
        Implementation.cmd.parse [] |> Implementation.exe

        (3, InOut.Out.length) |> Assert.Equal
        ("1. Building a house?", InOut.Out.lines[0]) |> Assert.Equal
        ("    i.  On sand", InOut.Out.lines[1]) |> Assert.Equal
        ("    ii. On rock", InOut.Out.lines[2]) |> Assert.Equal
