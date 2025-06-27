module XTests.Commands.Split.Tests

open Regl.CommandLine.Commands
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.IO
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types.Arguments
open XTests.Shared
open XTests.Types
open Xunit
open Xunit.Abstractions

type Tests (helper : ITestOutputHelper) =
    inherit TestBase (helper)

    [<Fact>]
    let ``test split`` () =
        setIn "192.168.0.255"

        Args "split [.]"
        |> executeEntries [| Split.entry |]
        |> fun entry ->
            helper.WriteLine entry.Value.name
            entry
        |> _.IsSome
        |> Assert.True

        ("192", Out.lines[0]) |> Assert.Equal
        ("168", Out.lines[1]) |> Assert.Equal
        ("0", Out.lines[2]) |> Assert.Equal
        ("255", Out.lines[3]) |> Assert.Equal

    [<Fact>]
    let ``test split --quote`` () =
        setIn "192.168.0.255"

        Args "split [.] --quote"
        |> executeEntries [| Split.entry |]
        |> _.IsSome
        |> Assert.True

        ("\"192\"", Out.lines[0]) |> Assert.Equal
        ("\"168\"", Out.lines[1]) |> Assert.Equal
        ("\"0\"", Out.lines[2]) |> Assert.Equal
        ("\"255\"", Out.lines[3]) |> Assert.Equal

    [<Theory>]
    [<InlineData("192..168..0..255.")>]
    [<InlineData(" 192 .. 168 .. 0..255.  ")>]
    [<InlineData("192.. 168 . . 0  ..255  .  ")>]
    [<InlineData("    192..168 . . 0  . .255  .  ")>]
    [<InlineData("    192..168 . . 0   . .255....")>]
    let ``test split --quote --trim`` (input : string) =
        setIn input

        Args "split [.] --quote --trim"
        |> executeEntries [| Split.entry |]
        |> _.IsSome
        |> Assert.True

        ("\"192\"", Out.lines[0]) |> Assert.Equal
        ("\"168\"", Out.lines[1]) |> Assert.Equal
        ("\"0\"", Out.lines[2]) |> Assert.Equal
        ("\"255\"", Out.lines[3]) |> Assert.Equal
