module XTests.Commands.Gen.Copy.Tests

open Regl.CommandLine.Commands.GenCommand
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.IO
open Regl.CommandLine.Types.Arguments
open XTests.Shared
open XTests.Types
open Xunit
open Xunit.Abstractions

type Tests (helper : ITestOutputHelper) =
    inherit TestBase(helper)
    
    [<Fact>]
    let ``test copy by line`` () =
        // lang=md
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

        Args "gen"
        |> executeEntries [| Implementation.entry |]
        |> ignore

        (3, InOut.Out.length) |> Assert.Equal
        ("2. Build the house.", InOut.Out.lines[0]) |> Assert.Equal
        ("    i.  By listening", InOut.Out.lines[1]) |> Assert.Equal
        ("    ii. By actions", InOut.Out.lines[2]) |> Assert.Equal

    [<Fact>]
    let ``test copy by line + //#!wrong commands`` () =
        //lang=md
        let sourceFile =
            "//#!
1. Building a house?
    i.  On sand
    ii. On rock
//#!copy 3
//#!a
//#!b
//#!c
2. Build the house.
    i.  By listening
    ii. By actions"

        setIn sourceFile

        Args "gen"
        |> executeEntries [| Implementation.entry |]
        |> ignore

        (3, InOut.Out.length) |> Assert.Equal
        ("2. Build the house.", InOut.Out.lines[0]) |> Assert.Equal
        ("    i.  By listening", InOut.Out.lines[1]) |> Assert.Equal
        ("    ii. By actions", InOut.Out.lines[2]) |> Assert.Equal

    [<Fact>]
    let ``test copy by --start + --end`` () =
        // lang=md
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

        Args "gen"
        |> executeEntries [| Implementation.entry |]
        |> ignore

        (3, InOut.Out.length) |> Assert.Equal
        ("1. Building a house?", InOut.Out.lines[0]) |> Assert.Equal
        ("    i.  On sand", InOut.Out.lines[1]) |> Assert.Equal
        ("    ii. On rock", InOut.Out.lines[2]) |> Assert.Equal

    [<Fact>]
    let ``test copy by --start + --end + //#!wrong commands`` () =
        // lang=md
        let sourceFile =
            "//#!
//#!copy --start
1. Building a house?
    i.  On sand
    ii. On rock
//#!something
//#!not a command
//#!copy --end
2. Build the house.
    i.  By listening
    ii. By actions"

        setIn sourceFile

        Args "gen"
        |> executeEntries [| Implementation.entry |]
        |> ignore

        (3, InOut.Out.length) |> Assert.Equal
        ("1. Building a house?", InOut.Out.lines[0]) |> Assert.Equal
        ("    i.  On sand", InOut.Out.lines[1]) |> Assert.Equal
        ("    ii. On rock", InOut.Out.lines[2]) |> Assert.Equal
