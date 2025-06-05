module Regl.CommandLine.Commands.GenCommand.Tpl

open System
open System.Diagnostics
open System.IO
open Regl.CommandLine.Builders
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Commands.GenCommand.Shared

let echoIdentifier = "#>"


let exe (result: ParseResult option) =
    let echoMapper (line: string) =
        if line.TrimStart().StartsWith(echoIdentifier) then
            "echo \"" + line.TrimStart('#', '>').TrimEnd() + "\""
        else
            line

    let doShell (buffer: LinesBuffer) =
        let tmpShName = "tmp.sh"
        File.WriteAllText("tmp.sh", buffer.all)

        let startInfo = ProcessStartInfo()
        startInfo.FileName <- "/bin/bash"
        startInfo.Arguments <- tmpShName
        startInfo.RedirectStandardOutput <- true
        startInfo.UseShellExecute <- false
        let prcs = Process.Start startInfo
        prcs.WaitForExit()

        if prcs.ExitCode > 0 then
            raise (Exception("Oops! The shell script went on vacation unexpectedly... 🏖️"))
        else
            prcs.StandardOutput.ReadToEnd()

    do
        InOut.In.filterRest isNotCmd (getParam result 0 |> int)
        |> fun sequence -> ReadonlyLinesBuffer(BySeq sequence)
        |> _.all
        |> fun ctx -> evcms |> List.iter (fun m -> m.doMatch ctx)

    do
        getParam result 1
        |> ByFilePath
        |> LinesBuffer
        |> _.mapRest(echoMapper)
        |> doShell
        |> fun output -> InOut.Out.all <- output


let cmd =
    let builder = CommandBuilder("tpl", exe)
    builder.requiredParamsCount <- 2
    builder.build ()
