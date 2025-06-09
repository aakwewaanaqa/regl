module XTests.Builders.Tests

open Regl.CommandLine.Commands
open Xunit
open Xunit.Abstractions

type Tests(helper : ITestOutputHelper) =
    [<Fact>]
    let ``test parsing split`` () =
        let cmd = Split.cmd
        ("split", cmd.name) |> Assert.Equal

        let parsed = cmd.parse [ ":"; "--quote" ]
        (":", parsed.getParam 0) |> Assert.Equal
        parsed.hasFlag "--quote" |> Assert.True