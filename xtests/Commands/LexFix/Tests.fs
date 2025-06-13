module XTests.Commands.LexFix

open Regl.CommandLine.IO.InOut
open XTests.Shared
open XTests.Types
open Xunit
open Xunit.Abstractions
open Regl.CommandLine.Commands

type LexFix (helper : ITestOutputHelper) =
    inherit TestBase(helper)

    [<Theory>]
    [<InlineData("<<>>>", "<>", "<<>>")>]
    [<InlineData("((()))", "()", "((()))")>]
    [<InlineData("{{}}}", "{}", "{{}}")>]   
    [<InlineData("<<>><>>", "<>", "<<>><>")>]
    [<InlineData("(()())", "()", "(()())")>]
    [<InlineData("<a>b>>", "<>", "<a>b")>]
    [<InlineData("<<abc>>>def", "<>", "<<abc>>def")>]
    let ``test lex-fix`` (raw : string, pattern : string, expected : string) =
        setIn raw 
        LexFix.cmd.parse ["--scope"; pattern ]
        |> LexFix.exe

        (expected, Out.lines[0]) |> Assert.Equal