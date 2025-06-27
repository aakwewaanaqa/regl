module Commands.Ls.Tests

open Regl.CommandLine.Commands
open Regl.CommandLine.Commands.Shared
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
        (16, Ls.entry.entries.Length) |> Assert.Equal
        ("ls", Ls.entry.name) |> Assert.Equal

        Args args |> executeEntries [| Ls.entry |] |> ignore
