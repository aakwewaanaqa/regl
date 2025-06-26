namespace XTests.Codebase.Parsing.ArgEntryTests

open System.Collections.Generic
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.FlagsAndParams
open Regl.Exts
open XTests.Types
open Xunit
open Xunit.Abstractions
open Regl.CommandLine.Types

type ArgEntryTests (helper : ITestOutputHelper) =
    inherit TestBase (helper)

    let printFlags : ArgBehaviour = fun dto ->
        dto.flags.map |> List.reduce (fun a b -> $"{a}{b}")

    [<Fact>]
    let ``test mechanics`` () =
        let dict = Dictionary<IFlag, bool>()
        dict.Add (BoolFlag("a"), true)
        dict.ContainsKey (BoolFlag("a")) |> Assert.True

    [<Theory>]
    [<InlineData("-ab")>]
    [<InlineData("-a -b")>]
    let ``test clustered short flags`` (argv : string) =
        let entry = ArgEntry("some entry")
        entry.flags <- [ BoolFlag("-a"); BoolFlag("-b") ]
        let result = entry |> ArgEntry.validate (Args argv) |> guardResult
        result.flags.ContainsKey (OnFlag("-a")) |> Assert.True
        result.flags.ContainsKey (OnFlag("-b")) |> Assert.True
        
    [<Theory>]
    [<InlineData("-ab -c")>] 
    [<InlineData("-a -b -c")>]
    [<InlineData("-abc")>]
    let ``test multiple clustered flags`` (argv : string) =
        let entry = ArgEntry("some entry")
        entry.flags <- [ BoolFlag("-a"); BoolFlag("-b"); BoolFlag("-c") ]
        let result = entry |> ArgEntry.validate (Args argv) |> guardResult
        result.flags.ContainsKey (OnFlag("-a")) |> Assert.True
        result.flags.ContainsKey (OnFlag("-b")) |> Assert.True
        result.flags.ContainsKey (OnFlag("-c")) |> Assert.True

    [<Theory>]
    [<InlineData("-fq -e 1337")>]
    [<InlineData("-f -q -e 1337")>]
    let ``test flag value`` (argv : string) =
        let entry = ArgEntry("some entry")
        entry.flags <- [ StringFlag("-e"); BoolFlag("-f"); BoolFlag("-q") ]
        let result = entry |> ArgEntry.validate (Args argv) |> guardResult
        result.flags.ContainsKey (OnFlag("-f")) |> Assert.True
        result.flags.ContainsKey (OnFlag("-q")) |> Assert.True
        match result.flags[InStringFlag("-e")][0] with
        | OfText t -> ("1337", t) |> Assert.Equal
        | _ -> Assert.Fail()

    [<Theory>]
    [<InlineData("-e foo -e bar -q")>]
    [<InlineData("-e foo -q -e bar")>]
    let ``test multiple flag values`` (argv : string) =
        let entry = ArgEntry("some entry")
        entry.flags <- [ StringFlag("-e"); BoolFlag("-q") ]
        let result = entry |> ArgEntry.validate (Args argv) |> guardResult
        Assert.Contains(OfText "foo", result.flags[InStringFlag("-e")])
        Assert.Contains(OfText "bar", result.flags[InStringFlag("-e")])