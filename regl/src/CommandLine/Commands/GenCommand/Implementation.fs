module Regl.CommandLine.Commands.GenCommand.Implementation

open System
open Regl.CommandLine.Commands.GenCommand.Types.Lines
open Regl.CommandLine.IO
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types
open Regl.CommandLine.Builders
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Commands.GenCommand
open Regl.CommandLine.Commands.GenCommand.Shared
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds

let subCmdEntries =
    [ AddEvcm.cmd
      Copy.cmd
      Echo.cmd
      Import.cmd
      SetEnvar.cmd
      Tpl.cmd
      UnsetEnvar.cmd ]

///TODO : remove
[<Obsolete>]
let exe (r : CommandParseResult) =
    let iteri i (line : string) =
        In.index <- i

        if i = 0 then
            identifier <- line.TrimEnd ()
        elif line.Trim().StartsWith (identifier) then
            line
            |> _.Trim()
            |> _.Substring(identifier.Length)
            |> parseCommandLineArgs
            |> tryCommands subCmdEntries
            |> function
                | Ok () -> ()
                | Error ex -> debugLog $"regl gen -> {ex}"

    r.tryGetFlagValue "--file"
    |> function
        | Some path -> In <- ReadonlyLinesBuffer (ByFilePath (path.ToString ()))
        | None -> In <- ReadonlyLinesBuffer (ByStdIn)

    In.iteriRest iteri

///TODO : remove
[<Obsolete>]
let cmd =
    let builder = CommandBuilder ("gen", exe)
    builder.optionalFlags <- [ StringFlag ("--file") ]
    builder.build ()

//TODO : write entry
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
            let mutable identifier = "//#!"

            In.iteriRest (fun i l ->
                In.index <- i // push index forward

                if i = 0 then
                    identifier <- l
                elif l.Trim().StartsWith (identifier) then
                    let line = l.Trim().Substring (identifier.Length) |> Line

                    if line.isCmd then
                        line.raw |> Args |> executeEntries subCmdEntries |> ignore)

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
    |> _.addEntry(ArgEntry ("gen with stdin") |> _.addBehaviour(exeByStdin))
    |> _.addEntry(
        ArgEntry ("gen with file path")
        |> _.addFlag(fileFlag)
        |> _.addBehaviour(exeByFile)
    )
