module XTests.Shared

open System.IO
open Xunit
open Xunit.Abstractions
open System.Diagnostics

type ShellResult = { code: int; output: string }

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