namespace XTests.Codebase.Parsing.ArgEntryTests

open System.Collections.Generic
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.FlagsAndParams
open Regl.Exts
open XTests.Types
open Xunit
open Xunit.Abstractions
open Regl.CommandLine.Types

type ArgEntryTests (helper : ITestOutputHelper) =
    inherit TestBase (helper)

    let printFlags : ArgBehaviour =
        fun dto -> dto.flags |> Seq.iter (fun a -> Out.appendLine $"{a}")

    [<Fact>]
    let ``test mechanics`` () =
        let dict = Dictionary<IFlag, bool> ()
        dict.Add (BoolFlag "a", true)
        dict.ContainsKey (BoolFlag "a") |> Assert.True

    [<Theory>]
    [<InlineData("-ab -c")>]
    [<InlineData("-a -bc")>]
    [<InlineData("-a -b -c")>]
    let ``test clustered short flags`` (argv : string) =
        let aFlag = BoolFlag "-a"
        let bFlag = BoolFlag "-b"
        let cFlag = BoolFlag "-c"

        let ``assert`` : ArgBehaviour = fun dto ->
            dto.flags.containsFlag(aFlag) |> Assert.True
            dto.flags.containsFlag(bFlag) |> Assert.True
            dto.flags.containsFlag(cFlag) |> Assert.True

        ArgEntry()
        |> _.addFlag(BoolFlag "-a")
        |> _.addFlag(BoolFlag "-b")
        |> _.addFlag(BoolFlag "-c")
        |> _.addBehaviour(``assert``)
        |> ArgEntry.validate (Args argv)
        |> _.IsOk
        |> Assert.True

    [<Theory>]
    [<InlineData("-fq -e 1337")>]
    [<InlineData("-f -q -e 1337")>]
    let ``test flag value`` (argv : string) =
        let fFlag = StringFlag "-f"
        let qFlag = StringFlag "-q"
        let eFlag = StringFlag "-e"

        let ``assert`` : ArgBehaviour = fun dto ->
            dto.flags.containsFlag(fFlag) |> Assert.True
            dto.flags.containsFlag(qFlag) |> Assert.True
            ("1337", dto.flags.first(eFlag)) |> Assert.Equal

        ArgEntry()
        |> _.addFlag(BoolFlag "-f")
        |> _.addFlag(BoolFlag "-q")
        |> _.addFlag(eFlag)
        |> _.addBehaviour(``assert``)
        |> ArgEntry.validate (Args argv)
        |> _.IsOk
        |> Assert.True

    [<Theory>]
    [<InlineData("boo -a --b")>]
    [<InlineData("-a boo --b")>]
    [<InlineData("-a --b boo")>]
    [<InlineData("boo --b -a")>]
    [<InlineData("--b boo -a")>]
    [<InlineData("--b -a boo")>]
    let ``test mixed`` (argv : string) =
        let aFlag = BoolFlag "-a"
        let bFlag = BoolFlag "--b"
        let cParam = Param "c"

        let ``assert`` : ArgBehaviour = fun dto ->
            dto.flags.containsFlag(aFlag) |> Assert.True
            dto.flags.containsFlag(bFlag) |> Assert.True
            ("boo", dto.parameters[cParam].value<string>()) |> Assert.Equal

        ArgEntry()
        |> _.addParameter(cParam)
        |> _.addFlag(aFlag)
        |> _.addFlag(bFlag)
        |> _.addBehaviour(``assert``)
        |> ArgEntry.validate(Args argv)
        |> _.IsOk
        |> Assert.True


    // TODO: Future feature this is too hard for the tool right now...
    // [<Theory>]
    // [<InlineData("-e foo -e bar -q")>]
    // [<InlineData("-e foo -q -e bar")>]
    let ``test multiple flag values`` (argv : string) =
        let eFlag = StringFlag "-e"

        let ``assert`` : ArgBehaviour = fun dto ->
            (2, dto.flags[eFlag].Length) |> Assert.Equal
            ("foo", dto.flags[eFlag].[0].value<string>()) |> Assert.Equal
            ("bar", dto.flags[eFlag].[1].value<string>()) |> Assert.Equal

        ArgEntry()
        |> _.addFlag(StringFlag "-e")
        |> _.addFlag(BoolFlag "-q")
        |> _.addBehaviour(``assert``)
        |> ArgEntry.validate (Args argv)
        |> _.IsOk
        |> Assert.True