module XTests.Shared

open System.Diagnostics

type ShellResult = { code: int; output: string }

let doShellCmd (cmd: string) =
    let startInfo = ProcessStartInfo()
    startInfo.FileName <- "/bin/bash"
    startInfo.Arguments <- $"-c \"{cmd}\""
    startInfo.RedirectStandardOutput <- true
    startInfo.UseShellExecute <- false
    let ``process`` = Process.Start startInfo
    ``process``.WaitForExit()
    let code = ``process``.ExitCode
    let output = ``process``.StandardOutput.ReadToEnd()
    { code = code; output = output }
