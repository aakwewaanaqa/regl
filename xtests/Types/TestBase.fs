namespace XTests.Types

open Xunit
open Xunit.Abstractions
open Regl.CommandLine.IO

[<CollectionDefinition("Seq", DisableParallelization = true)>]
type SeqCollection =
    class end

[<Collection("Seq")>]
type TestBase(helper : ITestOutputHelper) =
    do
        InOut.Out.all <- ""

    member b.helper = helper

