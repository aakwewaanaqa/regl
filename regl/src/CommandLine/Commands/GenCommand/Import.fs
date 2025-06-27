module Regl.CommandLine.Commands.GenCommand.Import

open Regl.CommandLine.Commands.GenCommand.Types.Lines
open Regl.CommandLine.IO
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds
open Regl.CommandLine.Types.FlagsAndParams

let cmdName = "import"

let cmdInfo = "Imports or executes another source file"

let rec subCmdEntries =
    [| AddEvcm.entry
       Copy.entry
       Echo.entry
       entry
       SetEnvar.entry
       Tpl.entry
       UnsetEnvar.entry |]

and entry =
    let fileParam = Param ("file", "the file path to another source file")

    let exeImport : ArgBehaviour =
        fun dto ->
            let mutable cmdBeginning = "//#!"
            let buffer = dto.parameters[fileParam].value<string> () |> ByFilePath |> LinesBuffer
            // iterates buffer with line index and raw string as each line
            buffer.iteriRest (fun i raw ->
                // push iterator forward
                buffer.index <- i
                // the first line will be the cmdBeginning indicator
                match buffer.index with
                | 0 -> cmdBeginning <- raw
                | _ ->
                    // the rest lines will be maybe commands to be executed if it starts with cmdBeginning
                    // <see cmdBeginning/>
                    match SourceLine (cmdBeginning, raw) with
                    | line when line.isCmd -> line.args |> executeEntries subCmdEntries |> ignore
                    | line -> line |> ignore)

    CmdEntry (cmdName, cmdInfo)
    |> _.addEntry(ArgEntry (cmdName)
                  |> _.addParameter(fileParam)
                  |> _.addBehaviour(exeImport)
    )
