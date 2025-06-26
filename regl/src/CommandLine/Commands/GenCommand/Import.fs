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
    let fileParam = Param("file", "the file path to another source file")

    let exeImport : ArgBehaviour = fun dto ->
        let mutable identifier : string = ""
        let mutable buffer = dto.parameters[fileParam].value<string>() |> ByFilePath |> LinesBuffer
        buffer.iteriRest(fun i l ->
            In.iteriRest (fun i l ->
            In.index <- i // push index forward

            if i = 0 then
                identifier <- l
            elif l.Trim().StartsWith (identifier) then
                let line = l.Trim().Substring (identifier.Length) |> Line

                if line.isCmd then
                    line.raw |> Args |> executeEntries subCmdEntries |> ignore)
            )

    CmdEntry(cmdName, cmdInfo)
    |> _.addEntry(ArgEntry(cmdName)
                  |> _.addParameter(fileParam)
                  |> _.addBehaviour(exeImport)
    )