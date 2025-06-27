module XTests.Commands.Match.Tests

open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.IO
open Regl.CommandLine.Types.Arguments
open XTests.Types
open Xunit
open Xunit.Abstractions
open XTests.Shared
open Regl.CommandLine.Commands

type Tests (helper : ITestOutputHelper) =
    inherit TestBase (helper)

    [<Theory>]
    [<InlineData("public async Task<Response<object>> GetDoc([FromBody] Firestore Dto)",
                 "match 'Task<Response<([a-zA-Z0-9]+?)>>'",
                 "Task<Response<object>>")>]
    [<InlineData("public async Task<Response<object>> GetDoc([FromBody] Firestore Dto)",
                 "match 'Task<Response<([a-zA-Z0-9]+?)>>' --format '$1'",
                 "object")>]
    let ``test match`` (stdin : string, args : string, expected : string) =
        setIn stdin

        Args args
        |> executeEntries [| Match.entry |]
        |> _.IsSome
        |> Assert.True

        (expected, InOut.Out.lines[0]) |> Assert.Equal