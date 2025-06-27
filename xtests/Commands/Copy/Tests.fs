module XTests.Commands.Copy.Tests

open Regl.CommandLine.Commands
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Types.Arguments
open TextCopy
open XTests.Shared
open XTests.Types
open Xunit
open Xunit.Abstractions

[<Trait("github CI", "false")>]
type Tests(helper : ITestOutputHelper) =
    inherit TestBase(helper)

    [<Fact>]
    let ``test copy no LF`` () =
        setIn "1 line is here"

        Args "copy"
        |> executeEntries([|Copy.entry|])
        |> _.IsSome
        |> Assert.True

        ("1 line is here", ClipboardService.GetText()) |> Assert.Equal

    [<Fact>]
    let ``test copy has LF`` () =
        setIn "1 line is here \n 2 second line is here \n"

        Args "copy"
        |> executeEntries([|Copy.entry|])
        |> _.IsSome
        |> Assert.True

        ("1 line is here \n 2 second line is here \n", ClipboardService.GetText()) |> Assert.Equal