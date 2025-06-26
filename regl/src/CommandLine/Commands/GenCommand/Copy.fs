module Regl.CommandLine.Commands.GenCommand.Copy

open System
open Regl.CommandLine.Commands.GenCommand.Types.Lines
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds
open Regl.CommandLine.Types.FlagsAndParams

let cmdName = "copy"

let cmdInfo = "Copies context to stdout"

let entry =
    let startFlag = BoolFlag ("--start", "starts copying to stdout")
    let endFlag = BoolFlag ("--end", "stop copying to stdout")

    let lineCountParam =
        IntParam ("line-count", "copying lines with a specific count of line to stdout")

    let endCopyEntry = ArgEntry ("stop copying") |> _.addFlag(endFlag)

    let exeCopyLines : ArgBehaviour =
        fun dto ->
            let mutable lineCount = dto.parameters[lineCountParam].value<int> ()

            In.iterRest (fun iteratedLine ->
                let line = iteratedLine |> Line

                if line.isCmd |> not && lineCount > 0 then
                    lineCount <- lineCount - 1
                    Out.appendLine iteratedLine)

    let exeCopyStartToEnd : ArgBehaviour =
        fun dto ->
            let mutable copying = true

            In.iterRest (fun iteratedLine ->
                let line = iteratedLine |> Line

                if copying && line.isCmd |> not then
                    Out.appendLine iteratedLine
                elif line.isCmd && line.cmdName.Value = "copy" then
                    let validateDto = endCopyEntry |> ArgEntry.validate line.args.Value

                    match validateDto with
                    | Ok _ -> copying <- false
                    | Error ex -> debugLog ex)

    CmdEntry (cmdName, cmdInfo)
    |> _.addEntry(ArgEntry ("copies by line")
                  |> _.addParameter(lineCountParam)
                  |> _.addBehaviour(exeCopyLines)
    )
    |> _.addEntry(ArgEntry ("starts copying")
                  |> _.addFlag(startFlag)
                  |> _.addBehaviour(exeCopyStartToEnd)
    )
    |> _.addEntry(endCopyEntry)
