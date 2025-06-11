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

    // 新增测试用例
    [<Fact>]
    let ``test empty string`` () =
        let result = parseCommandLineArgs ""
        (0, result.Length) |> Assert.Equal

    [<Fact>]
    let ``test single argument`` () =
        let result = parseCommandLineArgs "hello"
        (1, result.Length) |> Assert.Equal
        ("hello", result[0]) |> Assert.Equal

    [<Theory>]
    [<InlineData("--flag value", "--flag", "value")>]
    [<InlineData("--flag=value", "--flag", "value")>]
    [<InlineData("--config=file.txt", "--config", "file.txt")>]
    [<InlineData("--path=/home/user", "--path", "/home/user")>]
    let ``test long flags with equals`` (line : string) (flag : string) (value : string) =
        let result = parseCommandLineArgs line
        (2, result.Length) |> Assert.Equal
        (flag, result[0]) |> Assert.Equal
        (value, result[1]) |> Assert.Equal

    [<Theory>]
    [<InlineData("-f value", "-f", "value")>]
    [<InlineData("-f=value", "-f=value", "")>]
    let ``test short flags`` (line : string) (expected1 : string) (expected2 : string) =
        let result = parseCommandLineArgs line

        if expected2 <> "" then
            (2, result.Length) |> Assert.Equal
            (expected1, result[0]) |> Assert.Equal
            (expected2, result[1]) |> Assert.Equal
        else
            (1, result.Length) |> Assert.Equal
            (expected1, result[0]) |> Assert.Equal

    [<Theory>]
    [<InlineData("'hello world'", "hello world")>]
    [<InlineData("\"hello world\"", "hello world")>]
    [<InlineData("'with spaces   '", "with spaces   ")>]
    [<InlineData("\"with spaces   \"", "with spaces   ")>]
    let ``test quoted strings with spaces`` (line : string) (expected : string) =
        let result = parseCommandLineArgs line
        (1, result.Length) |> Assert.Equal
        (expected, result[0]) |> Assert.Equal

    [<Theory>]
    [<InlineData("'a\"b'", "a\"b")>]
    [<InlineData("\"a'b\"", "a'b")>]
    [<InlineData("'mix\"ed'", "mix\"ed")>]
    [<InlineData("\"mix'ed\"", "mix'ed")>]
    let ``test mixed quotes`` (line : string) (expected : string) =
        let result = parseCommandLineArgs line
        (1, result.Length) |> Assert.Equal
        (expected, result[0]) |> Assert.Equal

    [<Theory>]
    [<InlineData("a\\\\b", "a\\b")>]
    [<InlineData("a\\ b", "a b")>]
    [<InlineData("\\\"hello\\\"", "\"hello\"")>]
    [<InlineData("\\'hello\\'", "'hello'")>]
    let ``test escape sequences`` (line : string) (expected : string) =
        let result = parseCommandLineArgs line
        (1, result.Length) |> Assert.Equal
        (expected, result[0]) |> Assert.Equal

    [<Theory>]
    [<InlineData("   ")>]
    [<InlineData("\t\t")>]
    [<InlineData("     \t   ")>]
    let ``test whitespace only`` (line : string) =
        let result = parseCommandLineArgs line
        (0, result.Length) |> Assert.Equal

    [<Fact>]
    let ``test complex command lines`` () =
        // Case 1: Command with flags and arguments
        let line1 = "cmd --flag1 --flag2=value arg1 arg2"
        let expected1 = ["cmd"; "--flag1"; "--flag2"; "value"; "arg1"; "arg2"]
        let result1 = parseCommandLineArgs line1
        (expected1.Length, result1.Length) |> Assert.Equal
        expected1 |> List.iteri (fun i exp -> (exp, result1[i]) |> Assert.Equal)

        // Case 2: Command with quoted arguments
        let line2 = "program 'arg with spaces' --config=file.txt"
        let expected2 = ["program"; "arg with spaces"; "--config"; "file.txt"]
        let result2 = parseCommandLineArgs line2
        (expected2.Length, result2.Length) |> Assert.Equal
        expected2 |> List.iteri (fun i exp -> (exp, result2[i]) |> Assert.Equal)

        // Case 3: Command with double-quoted arguments
        let line3 = "app \"quoted arg\" normal_arg"
        let expected3 = ["app"; "quoted arg"; "normal_arg"]
        let result3 = parseCommandLineArgs line3
        (expected3.Length, result3.Length) |> Assert.Equal
        expected3 |> List.iteri (fun i exp -> (exp, result3[i]) |> Assert.Equal)

    [<Theory>]
    [<InlineData("'unclosed quote")>]
    [<InlineData("\"unclosed quote")>]
    [<InlineData("mixed 'quote\"")>]
    let ``test unclosed quotes`` (line : string) =
        let result = parseCommandLineArgs line
        // 函数应该处理未闭合的引号，通常将剩余内容作为一个参数
        Assert.True (result.Length > 0)

    [<Fact>]
    let ``test multiple long flags`` () =
        // Case 1: Multiple flag-value pairs with equals
        let line1 = "--flag1=value1 --flag2=value2"
        let expected1 = ["--flag1"; "value1"; "--flag2"; "value2"]
        let result1 = parseCommandLineArgs line1
        (expected1.Length, result1.Length) |> Assert.Equal
        expected1 |> List.iteri (fun i exp -> (exp, result1[i]) |> Assert.Equal)

        // Case 2: Multiple flags with different formats
        let line2 = "--debug --verbose=high --output=file.txt"
        let expected2 = ["--debug"; "--verbose"; "high"; "--output"; "file.txt"]
        let result2 = parseCommandLineArgs line2
        (expected2.Length, result2.Length) |> Assert.Equal
        expected2 |> List.iteri (fun i exp -> (exp, result2[i]) |> Assert.Equal)

    [<Theory>]
    [<InlineData("'a=b'", "a=b")>]
    [<InlineData("\"x=y\"", "x=y")>]
    [<InlineData("'--flag=value'", "--flag=value")>]
    let ``test equals in quoted strings`` (line : string) (expected : string) =
        let result = parseCommandLineArgs line
        (1, result.Length) |> Assert.Equal
        (expected, result[0]) |> Assert.Equal
