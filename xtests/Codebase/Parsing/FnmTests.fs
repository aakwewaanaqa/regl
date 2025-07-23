namespace XTests.Codebase.Parsing

open Fnm.Types
open XTests.Types
open Xunit
open Xunit.Abstractions
open Fnm.Types.Builders

type FnmTests(helper : ITestOutputHelper) =
    inherit TestBase(helper)

    [<Fact>]
    let ``test fact`` () =
        let ptrn = PatternBuilder.compile "abc"
        
        PatternCargo(true, "abc")
        |> ptrn.visit
        |> fun pc ->
            Assert.True(pc.IsSome)
            Assert.False(pc.Value.isIn)