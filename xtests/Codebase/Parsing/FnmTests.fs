namespace XTests.Codebase.Parsing

open Fnm.Types
open XTests.Types
open Xunit
open Xunit.Abstractions
open Fnm.Types.Builders

type FnmTests(helper: ITestOutputHelper) =
    inherit TestBase(helper)

    [<Fact>]
    let ``test fact`` () =
        let pattern = PatternBuilder.compile "abc"

        PatternCargo(true, "abc")
        |> pattern.visit
        |> fun pc -> Assert.False(pc.isIn)

    [<Theory>]
    [<InlineData("abc", "abc", false)>]
    [<InlineData("abc", "abc/f.txt", false)>]
    [<InlineData("abc", "abc/.env", false)>]
    [<InlineData("*.env", "abc/.env", false)>]
    [<InlineData("*.env", "abc/mine.env", false)>]
    [<InlineData("*abc", "abc", false)>]
    [<InlineData("*abc*", "abc", false)>]
    [<InlineData("aab", "abc", true)>]
    let ``test pattern`` (pattern: string) (path: string) (isIn: bool) =
        let pattern = PatternBuilder.compile pattern

        PatternCargo(true, path)
        |> pattern.visit
        |> fun pc ->
            if isIn then
                Assert.True pc.isIn
            else
                Assert.False pc.isIn