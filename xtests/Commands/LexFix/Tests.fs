module XTests.Commands.LexFix

open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types.Arguments
open XTests.Shared
open XTests.Types
open Xunit
open Xunit.Abstractions
open Regl.CommandLine.Commands

type LexFix (helper : ITestOutputHelper) =
    inherit TestBase(helper)

    [<Theory>]
    [<InlineData("<<>>>", "lex-fix --scope <>", "<<>>")>]
    [<InlineData("((()))", "lex-fix --scope ()", "((()))")>]
    [<InlineData("{{}}}", "lex-fix --scope {}", "{{}}")>]
    [<InlineData("<<>><>>", "lex-fix --scope <>", "<<>><>")>]
    [<InlineData("(()())", "lex-fix --scope ()", "(()())")>]
    [<InlineData("<a>b>>", "lex-fix --scope <>", "<a>b")>]
    [<InlineData("<<abc>>>def", "lex-fix --scope <>", "<<abc>>def")>]
    let ``test lex-fix`` (stdin : string, args : string, expected : string) =
        setIn stdin

        Args args
        |> executeEntries [|LexFix.entry|]
        |> ignore

        (expected, Out.lines[0]) |> Assert.Equal