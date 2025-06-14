module XTests.Commands.Gen.Tpl.Tests

open System.IO
open Regl.CommandLine.IO.InOut
open XTests.Types
open Xunit
open Xunit.Abstractions
open Regl.CommandLine.Commands.GenCommand
open XTests.Shared

type Tests (helper : ITestOutputHelper) =
    inherit TestBase(helper)

    [<Fact>]
    let ``test tpl`` () =
        cd "Commands/Gen/Tpl"
        setIn (File.ReadAllText("controller.cs"))
        Implementation.cmd.parse [ "gen" ] |> Implementation.exe

        ("[FromBody]", Out.lines[0]) |> Assert.Equal<string>
        ("[FromQuery]", Out.lines[0]) |> Assert.NotEqual<string>
        ("[FromBody] FirestoreDocDto dto", Out.lines[1]) |> Assert.Equal<string>

    [<Fact>]
    let ``test tpl if envar is reverted`` () =
        let srcFile = "//#!
//#!add-evcm God $0 God
//#!tpl 1 sh.sh
God : Let there be light.
//#!tpl 1 sh.sh
And there is the light.
        "

        let shFile = """
if [[ -n "$God" ]]; then
    echo "True"
else
    echo "False"
fi
"""

        File.WriteAllText("sh.sh", shFile)
        setIn srcFile

        Implementation.cmd.parse [] |> Implementation.exe

        (4, Out.length) |> Assert.Equal
        ("True", Out.lines[0]) |> Assert.Equal
        ("False", Out.lines[2]) |> Assert.Equal
