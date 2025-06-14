module XTests.Commands.Copy.Tests

open System
open Microsoft.VisualStudio.TestPlatform.ObjectModel
open Regl.CommandLine.Commands
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
        Copy.cmd.parse [] |> Copy.cmd.execute
        ("1 line is here", ClipboardService.GetText()) |> Assert.Equal

    [<Fact>]
    let ``test copy has LF`` () =
        setIn "1 line is here \n 2 second line is here \n"
        Copy.cmd.parse [] |> Copy.cmd.execute
        ("1 line is here \n 2 second line is here \n", ClipboardService.GetText()) |> Assert.Equal