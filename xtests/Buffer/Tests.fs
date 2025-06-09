module XTests.Buffer.Tests

open Regl.CommandLine.IO
open Xunit
open Xunit.Abstractions

type Tests(helper : ITestOutputHelper) =
    [<Fact>]
    let ``test print 1 line`` () =
        InOut.Out.all <- "1 line is here"
        InOut.Out.sendToPipe()