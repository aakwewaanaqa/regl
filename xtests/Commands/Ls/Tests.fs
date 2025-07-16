module Commands.Ls.Tests

open System.IO
open System.Text.RegularExpressions
open Regl.CommandLine.Commands
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.IO
open Regl.CommandLine.Types.Arguments
open XTests.Types
open Xunit
open Xunit.Abstractions

type Tests (helper : ITestOutputHelper) =
    inherit TestBase (helper)

    [<Theory>]
    [<InlineData("ls")>]
    [<InlineData("ls -R")>]
    [<InlineData("ls -d")>]
    [<InlineData("ls -f")>]
    [<InlineData("ls -Rd")>]
    [<InlineData("ls -df")>]
    [<InlineData("ls -Rf")>]
    [<InlineData("ls -Rdf")>]
    [<InlineData("ls -R --pattern '*'")>]
    [<InlineData("ls -d --pattern '*'")>]
    [<InlineData("ls -f --pattern '*'")>]
    [<InlineData("ls -Rd --pattern '*'")>]
    [<InlineData("ls -df --pattern '*'")>]
    [<InlineData("ls -Rf --pattern '*'")>]
    [<InlineData("ls -Rdf --pattern '*'")>]
    let ``test ls`` (args : string) =
        (32, Ls.entry.entries.Length) |> Assert.Equal
        ("ls", Ls.entry.name) |> Assert.Equal

        Args args |> executeEntries [| Ls.entry |] |> ignore

    [<Fact>]
    let ``test fact`` () =
        
        [|
            "/home/c0054/UnityProjects/regl/xtests/bin/Release/net9.0/mine.env"
            "/home/c0054/UnityProjects/regl/xtests/bin/Release/net9.0/main.pyc"
        |]
        |> Array.filter(fun path ->
            [|
                Regex(".*\.env")
                Regex(".*\.pyc")
            |]
            |> Array.exists(_.IsMatch(path))
            |> not
        )
        |> fun array -> Assert.True(array.Length = 0)
    
    [<Theory>]
    [<InlineData("ls --ignore-file='.gitignore'")>]
    let ``test ls -f --ignore-file`` (args : string) =
        // Arrange
        // Prepare gitignore and files
        let gitignore = "
*.env
*.pyc
*.doc*
        "
        File.WriteAllText(".gitignore", gitignore)
        
        let someTxt = "123"
        File.WriteAllText("something.txt", someTxt)
        File.WriteAllText("nothing.txt", someTxt)
        File.WriteAllText("anything.txt", someTxt)
        File.WriteAllText("mine.env", someTxt)
        File.WriteAllText("main.pyc", someTxt)

        Directory.CreateDirectory(".doc") |> ignore        
        File.WriteAllText(".doc/main.pyc", someTxt)
        
        // Act
        let entry = Args args
                    |> executeEntries [| Ls.entry |]
             
        // Assert
        helper.WriteLine(InOut.Out.all)
        Assert.False(InOut.Out.all.Contains(".pyc"))
        Assert.False(InOut.Out.all.Contains(".env"))
        Assert.False(InOut.Out.all.Contains(".doc"))