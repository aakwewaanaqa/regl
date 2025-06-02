module xtests.BuilderTests

open Regl.Program
open Xunit
open Xunit.Abstractions

type TestParsing(output: ITestOutputHelper) =
    let log a = output.WriteLine $"{a}"

    [<Fact>]
    let ``Test Positional Parameters`` () =
        let builder = CommandBuilder("split")
        builder.requiredParamsCount <- 1
        let parser = builder.build ()
        let results = [| "split"; ":" |] |> parser

        Assert.True(results.IsSome)
        Assert.True(results.Value.prmtrs[0] = ":")

    [<Fact>]
    let ``Test Required Arguments`` () =
        let builder = CommandBuilder("match")
        builder.requiredParamsCount <- 1
        builder.requiredFlags <- [ "--outformat" ]
        let parser = builder.build ()

        do
            let results = [| "match"; "[0-9]+"; "--outformat"; "$0" |] |> parser
            Assert.True(results.IsSome)
            let value = results.Value
            Assert.Equal(value.prmtrs[0], "[0-9]+")
            let f, i = value.args[0]
            Assert.Equal(f, "--outformat")
            Assert.True(i.IsSome)
            Assert.Equal(i.Value, "$0")
