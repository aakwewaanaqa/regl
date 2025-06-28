module XTests.Codebase.Manual.Tests

open XTests.Types
open Xunit
open Xunit.Abstractions

type Tests (helper : ITestOutputHelper) =
    inherit TestBase (helper)
    
    [<Fact>]
    let ``test manual`` () = ()