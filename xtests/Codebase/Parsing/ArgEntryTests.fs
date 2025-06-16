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
    let ``test arg entry`` () =
        let entry = ArgEntry("some entry")
        entry.flags <- [ OnFlag("-a"); OnFlag("-b") ]
        let result = entry |> ArgEntry.validate (Args ["-ab"]) |> guardResult
        let dict = result.flags :> IDictionary<IFlag, FlagVal> |> Dictionary
        Assert.Contains(OnFlag("-a") :> IFlag, dict) |> ignore
        Assert.Contains(OnFlag("-b") :> IFlag, dict) |> ignore