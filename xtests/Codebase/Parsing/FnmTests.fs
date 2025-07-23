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
    [<InlineData("abc", "abc", false)>]                     //
    [<InlineData("abc", "abc/f.txt", false)>]               //
    [<InlineData("abc", "abc/.env", false)>]                //
    [<InlineData("*.env", "abc/.env", false)>]              //
    [<InlineData("*.env", "abc/mine.env", false)>]          //
    [<InlineData("*.env", "/a/b/c/d/abc/mine.env", false)>] //
    [<InlineData("*.env", "/a/b/c/d/abc/mine.doc", true)>]  // deal with IndexOutOfRangeException
    [<InlineData("*abc", "abc", false)>]                    //
    [<InlineData("*abc*", "abc", false)>]                   //
    [<InlineData("aab", "abc", true)>]                      //
    [<InlineData("aabede", "abc", true)>]                   // deal with IndexOutOfRangeException
    let ``test pattern`` (pattern: string) (path: string) (isIn: bool) =
        let pattern = PatternBuilder.compile pattern

        PatternCargo(true, path)
        |> pattern.visit
        |> fun pc ->
            if isIn then
                Assert.True pc.isIn
            else
                Assert.False pc.isIn
                
    [<Fact>]
    let ``test matcher fact`` () =
        let pathFilter (path : string) =
            "*.env\n*.doc"
            |> MatchBuilder.ofRaw
            |> _.visit(path)
            |> _.isIn
            
        [|
            "/src/shells/bin/.env"
            "/src/.env"
            ".env"
            "/src/Program.cs"
        |]
        |> Array.filter pathFilter
        |> Array.map (fun p ->
            helper.WriteLine p
            p
        )
        |> Array.reduce (fun a b -> $"{a}\n{b}")
        |> fun all -> Assert.False(all.Contains(".env"))