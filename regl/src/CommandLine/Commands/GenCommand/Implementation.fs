module Regl.CommandLine.Commands.GenCommand.Implementation

open Regl.CommandLine
open Regl.CommandLine.Commands.GenCommand.Shared
open Regl.CommandLine.Commands.GenCommand.Types.Lines
open Regl.CommandLine.IO
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Commands.GenCommand
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds

let cmdName = "gen"

let cmdInfo = "Generates codes from a source file..."

let subCmdEntries =
    [| AddEvcm.entry
       Copy.entry
       Echo.entry
       Import.entry
       SetEnvar.entry
       Tpl.entry
       UnsetEnvar.entry |]

let entry =
    let exeGen : ArgBehaviour =
        fun dto ->
            In.iteriRest (fun i raw ->
                In.index <- i // push index forward
                match i = 0 with
                | true -> identifier <- raw
                | false ->
                    try
                        let line = SourceLine(identifier, raw)
                        match line.isCmd with
                        | true -> line.args |> executeEntries subCmdEntries |> ignore
                        | false -> ()
                    with ex ->
                        Debug.through ex |> ignore
            )

    let fileFlag =
        StringFlag ("--file", "the file path for the source file to be generated")

    let exeByStdin : ArgBehaviour =
        fun dto ->
            In <- ReadonlyLinesBuffer (ByStdIn)
            exeGen dto

    let exeByFile : ArgBehaviour =
        fun dto ->
            let file = dto.flags.first<string> (fileFlag)
            In <- ReadonlyLinesBuffer (ByFilePath file)
            exeGen dto

    CmdEntry (cmdName, cmdInfo)
    |> _.addEntry(ArgEntry ("gen with stdin")
                  |> _.addBehaviour(exeByStdin)
    )
    |> _.addEntry(ArgEntry ("gen with file path")
                  |> _.addFlag(fileFlag)
                  |> _.addBehaviour(exeByFile)
    )
