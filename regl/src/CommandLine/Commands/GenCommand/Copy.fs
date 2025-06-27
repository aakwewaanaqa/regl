module Regl.CommandLine.Commands.GenCommand.Copy

open System
open Regl.CommandLine.Commands.GenCommand.Shared
open Regl.CommandLine.Commands.GenCommand.Types.Lines
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds
open Regl.CommandLine.Types.FlagsAndParams

let cmdName = "copy"

let cmdInfo = "Copies context to stdout"

let entry =
    /// flag to start copying to stdout
    let startFlag = BoolFlag ("--start", "start copying to stdout")
    /// flag to stop copying to stdout
    let endFlag = BoolFlag ("--end", "stop copying to stdout")

    /// param to tell how many copying lines to stdout
    let lineCountParam =
        IntParam ("line-count", "tell how many copying lines to stdout")

    /// `copy --stop` entry
    let endCopyEntry = ArgEntry ("stop copying") |> _.addFlag(endFlag)

    /// copy lines behaviour
    let exeByLines : ArgBehaviour =
        fun dto ->
            let mutable lineCount = dto.parameters[lineCountParam].value<int> ()

            In.iterRest (fun raw ->
                let line = SourceLine (identifier, raw)

                match lineCount > 0 with
                | true when not line.isCmd -> // when still copying
                    Out.appendLine raw
                    lineCount <- lineCount - 1
                    ()
                | _ -> ())

    /// copy from `copy --start` to `copy --end` behaviour
    let exeByStartToEnd : ArgBehaviour =
        fun dto ->
            let mutable copying = true
            // iterates the rest lines when encountered the copy --start
            In.iterRest (fun raw ->
                let line = SourceLine (identifier, raw)

                match copying with
                | true when not line.isCmd -> // encountered a normal line
                    Out.appendLine raw
                    ()
                | true when line.isCmd && endCopyEntry |> ArgEntry.validate line.args |> _.IsOk -> // encountered `copy --end`
                    copying <- false
                    ()
                | _ -> ())

    CmdEntry (cmdName, cmdInfo)
    |> _.addEntry(
        ArgEntry ("copies by line")
        |> _.addParameter(lineCountParam)
        |> _.addBehaviour(exeByLines)
    )
    |> _.addEntry(
        ArgEntry ("starts copying")
        |> _.addFlag(startFlag)
        |> _.addBehaviour(exeByStartToEnd)
    )
    |> _.addEntry(endCopyEntry)
