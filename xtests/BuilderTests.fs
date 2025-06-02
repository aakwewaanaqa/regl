module xtests.BuilderTests

open System
open Xunit
open Xunit.Abstractions
open regl
open regl.Builders.Source

type TestParsing(output: ITestOutputHelper) =

    let log a =
        output.WriteLine $"{a}"
        a

    [<Fact>]
    let ``Test Build`` () = output.WriteLine "Hello"

    [<Fact>]
    let ``Test Copy`` () =
        Console.In.Close()
        Assert.True true

    [<Fact>]
    let ``Test Match Arguments`` () =
        Console.In.Close()
        output.WriteLine $"{Commands.matchCmd}"

        [| "match"; "[0-9]"; "--format"; "$1" |]
        |> Commands.matchCmd.parse
        |> Option.get
        |> _.flags
        |> Array.tryFind (fun f -> f.name = "--format")
        |> Option.map (fun f -> f :?> IInFlag<string>)
        |> Option.bind (fun f -> Some f.value)
        |> Option.get
        |> (fun str -> Assert.Equal(str, "$1"))

        ()
