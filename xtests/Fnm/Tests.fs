namespace XTests.Fnm

open System.Reflection
open Fnm.Helper
open Fnm.Pattern.Parse
open XTests.Types
open Xunit
open Xunit.Abstractions

type Tests(helper: ITestOutputHelper) =
    inherit TestBase(helper)

    [<Theory>]
    [<InlineData("abc", "abc", true)>]                     
    [<InlineData("abc", "abc/f.txt", true)>]               
    [<InlineData("abc", "abc/.env", true)>]                
    [<InlineData("*.env", "abc/.env", true)>]              
    [<InlineData("*.env", "abc/mine.env", true)>]          
    [<InlineData("*.env", "/a/b/c/d/abc/mine.env", true)>] 
    [<InlineData("*.env", "/a/b/c/d/abc/mine.doc", false)>]
    [<InlineData("*abc*", "/a/b/c/d/abc/mine.doc", true)>]                   
    [<InlineData("*abc", "/a/b/c/d/abc/mine.doc", true)>]                   
    [<InlineData("*abc", "abc", true)>]                    
    [<InlineData("*abc*", "abc", true)>]                   
    [<InlineData("aab", "abc", false)>]                    
    [<InlineData("aabede", "abc", false)>]                 
    let ``test basic tree``(pattern: string, path: string, isMatched: bool) =
        pattern
        |> StringCargo
        |> Trees.basicTree
        |> function
            | Some matcher ->
                path
                |> StringCargo
                |> matcher.func
                |> function
                    | Some rem when isMatched -> ()
                    | None when not isMatched -> ()
                    | _ -> Assert.Fail()
            | None ->
                Assert.Fail()