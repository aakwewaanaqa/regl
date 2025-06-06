module XTests.ShellTests

open System
open System.IO
open System.Diagnostics
open Xunit
open Xunit.Abstractions

type ShellResult = { code: int; output: string }

let randomName () =
    let random = Random()

    let consonants =
        [| 'b'
           'c'
           'd'
           'f'
           'g'
           'h'
           'j'
           'k'
           'l'
           'm'
           'n'
           'p'
           'q'
           'r'
           's'
           't'
           'v'
           'w'
           'x'
           'y'
           'z' |]

    let vowels = [| 'a'; 'e'; 'i'; 'o'; 'u' |]
    let length = random.Next(4, 8)

    Array.init length (fun i ->
        if i % 2 = 0 then
            consonants.[random.Next(consonants.Length)]
        else
            vowels.[random.Next(vowels.Length)])
    |> String

[<Obsolete("do shell by string will cause error 2...")>]
let doShell (c: string) =
    let startInfo = ProcessStartInfo()
    startInfo.FileName <- "/bin/bash"
    startInfo.Arguments <- $"-c \"{c}\""
    startInfo.RedirectStandardOutput <- true
    startInfo.UseShellExecute <- false
    let prcs = Process.Start startInfo
    prcs.WaitForExit()
    let code = prcs.ExitCode
    let output = prcs.StandardOutput.ReadToEnd()
    { code = code; output = output }

let doShellFile (file: string) =
    let startInfo = ProcessStartInfo()
    startInfo.FileName <- "/bin/bash"
    startInfo.Arguments <- file
    startInfo.RedirectStandardOutput <- true
    startInfo.UseShellExecute <- false
    let prcs = Process.Start startInfo
    prcs.WaitForExit()
    let code = prcs.ExitCode
    let output = prcs.StandardOutput.ReadToEnd()
    { code = code; output = output }

type Tests(output: ITestOutputHelper) =
    [<Fact>]
    let ``test envar equality of Environment.SetEnvironmentVariable`` () =
        let varname = randomName ()
        let varval = "test_value"
        Environment.SetEnvironmentVariable(varname, varval)
        let result = doShell $"echo ${varname}"
        Assert.Equal(0, result.code)
        Assert.Equal(varval + Environment.NewLine, result.output)
        output.WriteLine result.output

    [<Fact>]
    let ``test output equality of shell-test-1.sh`` () =
        let resultByFile = doShellFile "shell-test-1.sh"
        Assert.Equal(0, resultByFile.code)

        let fileText = File.ReadAllText "shell-test-1.sh"
        let resultByText = doShell fileText
        Assert.Equal(0, resultByText.code)
        Assert.Equal(resultByFile.output, resultByText.output)
        output.WriteLine resultByText.output


    [<Fact>]
    let ``test output equality by templating shell-test-2.sh`` () =
        let fileText =
            File.ReadAllText "shell-test-2.sh"
            |> _.Split("\n")
            |> Array.map (fun line ->
                if line.StartsWith("#>") then
                    "echo \"" + line.TrimStart('#', '>').TrimEnd() + "\""
                else
                    line)
            |> Array.reduce (fun a b -> $"{a}\n{b}")

        Environment.SetEnvironmentVariable("must_be_set", "7")
        Environment.SetEnvironmentVariable("TResult", "UserInfo")
        Environment.SetEnvironmentVariable("TDto", "UserDto")
        File.WriteAllText("tmp.sh", fileText)
        let result = doShellFile "tmp.sh"
        Assert.Equal(0, result.code)
        Assert.Equal("    public static UniTask<Response<UserInfo>> IsUserAlive(UserDto dto)\n", result.output)
        File.WriteAllText("output.txt", result.output)
        ()
