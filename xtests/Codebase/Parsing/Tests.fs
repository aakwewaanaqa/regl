module XTests.Codebase.Parsing.Tests

open Xunit
open Xunit.Abstractions
open Regl.CommandLine.Commands.Shared

type Tests (output : ITestOutputHelper) =
    [<Theory>]
    [<InlineData("a b c")>]
    [<InlineData("a 'b' \"c\"")>]
    [<InlineData("a 'b' \"c\"   ")>]
    [<InlineData("  a   'b'   \"c\"   ")>]
    let ``test parse line`` (line : string) =
        let result = parseCommandLineArgs line
        (3, result.Length) |> Assert.Equal
        ("a", result[0]) |> Assert.Equal
        ("b", result[1]) |> Assert.Equal
        ("c", result[2]) |> Assert.Equal

    [<Theory>]
    [<InlineData(""" "a\"" b """, "a\"", "b")>]
    [<InlineData(""" "\"a\\"    'b\''    """, "\"a\\", "b'")>]
    [<InlineData(""" "a'"    'b"'  """, "a'", "b\"")>]
    let ``test parse line with escaping`` (line : string) (a : string) (b : string) =
        let result = parseCommandLineArgs line
        (2, result.Length) |> Assert.Equal
        (a, result[0]) |> Assert.Equal
        (b, result[1]) |> Assert.Equal
