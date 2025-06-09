module XTests.Shared

open System
open System.IO
open Microsoft.VisualStudio.TestPlatform.PlatformAbstractions
open Xunit
open Xunit.Abstractions
open System.Diagnostics

type ShellResult = { code: int; output: string } with
    member this.lines =
        this.output.Split("\n")

let reglPathInCmd =
    "PATH=$PATH:/home/c0054/UnityProjects/regl/regl/bin/Release/net9.0/linux-x64"

let doShell (filePath : string) =
    let startInfo = ProcessStartInfo()
    startInfo.FileName <- "/bin/bash"
    startInfo.Arguments <- filePath
    startInfo.RedirectStandardOutput <- true
    startInfo.UseShellExecute <- false
    let ``process`` = Process.Start startInfo
    ``process``.WaitForExit()
    let code = ``process``.ExitCode
    let output = ``process``.StandardOutput.ReadToEnd()
    { code = code; output = output }

let doShellCmd (cmd: string) =
    File.WriteAllText("tmp.sh", $"#!/bin/bash\n{cmd}")

    let startInfo = ProcessStartInfo()
    startInfo.FileName <- "/bin/bash"
    startInfo.Arguments <- "tmp.sh"
    startInfo.RedirectStandardOutput <- true
    startInfo.UseShellExecute <- false
    let ``process`` = Process.Start startInfo
    ``process``.WaitForExit()
    let code = ``process``.ExitCode
    let output = ``process``.StandardOutput.ReadToEnd()
    { code = code; output = output }

let testLog (output : ITestOutputHelper) a =
    output.WriteLine $"{a}"
    a

let is0 (result : ShellResult) =
    Assert.Equal(0, result.code)

let setIn (text : string) =
    Console.SetIn (new StringReader (text) :> TextReader)