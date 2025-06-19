namespace XTests.Codebase.Parsing.ArgEntryTests

open System.Collections.Generic
open Regl.Exts
open XTests.Types
open Xunit
open Xunit.Abstractions
open Regl.CommandLine.Types

type ArgEntryTests (helper : ITestOutputHelper) =
    inherit TestBase (helper)

    [<Fact>]
    let ``test mechanics`` () =
        let dict = Dictionary<IFlag, bool>()
        dict.Add (OnFlag("a"), true)
        dict.ContainsKey (OnFlag("a")) |> Assert.True

    [<Theory>]
    [<InlineData("-ab")>]
    [<InlineData("-a -b")>]
    let ``test clustered short flags`` (argv : string) =
        let entry = ArgEntry("some entry")
        entry.flags <- [ OnFlag("-a"); OnFlag("-b") ]
        let result = entry |> ArgEntry.validate (Args argv) |> guardResult
        result.flags.ContainsKey (OnFlag("-a")) |> Assert.True
        result.flags.ContainsKey (OnFlag("-b")) |> Assert.True
        
    [<Theory>]
    [<InlineData("-ab -c")>] 
    [<InlineData("-a -b -c")>]
    let ``test multiple clustered flags`` (argv : string) =
        let entry = ArgEntry("some entry")
        entry.flags <- [ OnFlag("-a"); OnFlag("-b"); OnFlag("-c") ]
        let result = entry |> ArgEntry.validate (Args argv) |> guardResult
        result.flags.ContainsKey (OnFlag("-a")) |> Assert.True
        result.flags.ContainsKey (OnFlag("-b")) |> Assert.True
        result.flags.ContainsKey (OnFlag("-c")) |> Assert.True

    [<Theory>]
    [<InlineData("-fq -e 1337")>]
    [<InlineData("-f -q -e 1337")>]
    let ``test flag value`` (argv : string) =
        let entry = ArgEntry("some entry")
        entry.flags <- [ InStringFlag("-e"); OnFlag("-f"); OnFlag("-q") ]
        let result = entry |> ArgEntry.validate (Args argv) |> guardResult
        result.flags.ContainsKey (OnFlag("-f")) |> Assert.True
        result.flags.ContainsKey (OnFlag("-q")) |> Assert.True
        match result.flags[InStringFlag("-e")][0] with
        | OfText t -> ("1337", t) |> Assert.Equal
        | _ -> Assert.Fail()
        
    // [<Theory>]
    // [<InlineData("-e foo -e bar")>]
    // [<InlineData("-e foo -q -e bar")>]
    // let ``test multiple flag values`` (argv : string) =
    //     let entry = ArgEntry("some entry")
    //     entry.flags <- [ InStringFlag("-e"); OnFlag("-q") ]
    //     let result = entry |> ArgEntry.validate (Args argv) |> guardResult
    //     match result.flags[InStringFlag("-e")] with
    //     | OfText t -> ("bar", t) |> Assert.Equal
    //     | _ -> Assert.Fail()