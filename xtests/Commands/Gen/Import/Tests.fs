module XTests.Commands.Gen.Import

open System
open System.IO
open Regl.CommandLine.IO.InOut
open XTests.Shared
open XTests.Types
open Xunit
open Xunit.Abstractions
open Regl.CommandLine.Commands.GenCommand

type Tests (helper : ITestOutputHelper) =
    inherit TestBase (helper)

    [<Fact>]
    let ``test import`` () =
        let sourceFile1 =
            "//#!
//#!echo 0
//#!import sourceFile2.tmp
        "

        let sourceFile2 =
            "//#!
//#!echo 1
        "

        File.WriteAllText ("sourceFile2.tmp", sourceFile2)
        setIn sourceFile1

        Implementation.cmd.parse [] |> Implementation.exe

        ("0", Out.lines[0]) |> Assert.Equal
        ("1", Out.lines[1]) |> Assert.Equal

    [<Fact>]
    let ``test chain import`` () =
        let sourceFile =
            "//#!
//#!import 1.tpl"

        let templateFile1 =
            "//#!
//#!import 2.tpl"

        let templateFile2 =
            "//#!
//#!echo 2"

        File.WriteAllText ("1.tpl", templateFile1)
        File.WriteAllText ("2.tpl", templateFile2)
        setIn sourceFile

        Implementation.cmd.parse [] |> Implementation.exe
        ("2", Out.lines[0]) |> Assert.Equal