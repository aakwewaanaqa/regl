module XTests.Commands.Match.Tests

open Regl.CommandLine.IO
open XTests.Types
open Xunit
open Xunit.Abstractions
open XTests.Shared
open Regl.CommandLine.Commands

type Tests (helper : ITestOutputHelper) =
    inherit TestBase (helper)

    [<Fact>]
    let ``test match`` () =
        setIn "public async Task<Response<object>> GetDoc([FromBody] Firestore Dto)"
        Match.cmd.parse [ "Task<Response<([a-zA-Z0-9]+?)>>" ] |> Match.cmd.execute

        ("Task<Response<object>>", InOut.Out.lines[0]) |> Assert.Equal

    [<Fact>]
    let ``test match --format`` () =
        setIn "public async Task<Response<object>> GetDoc([FromBody] Firestore Dto)"

        Match.cmd.parse [ "Task<Response<([a-zA-Z0-9]+?)>>" ; "--format" ; "$1" ]
        |> Match.cmd.execute

        ("object", InOut.Out.lines[0]) |> Assert.Equal