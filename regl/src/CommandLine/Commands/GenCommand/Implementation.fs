module Regl.CommandLine.Commands.GenCommand.Implementation

open Regl
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
open Regl.CommandLine.Types.FlagsAndParams

let cmdName = "gen"

let cmdInfo = "generates codes from a source file
        uses like `< source.cs regl gen`
        or `regl gen --file source.cs` 😎"

let subCmdEntries =
    [| AddEvcm.entry
       Copy.entry
       Echo.entry
       Import.entry
       SetEnvar.entry
       Tpl.entry
       UnsetEnvar.entry |]

let entry =
    /// the file path for the source file to be generated
    let fileFlag =
        StringFlag("--file", "the file path for the source file to be generated")

    /// labels the gen command
    let labelFlag = StringFlag("--label", "labels the gen command")

    let combos = Exts.powerset [ fileFlag :> IFlag; labelFlag :> IFlag ]
    
    let exeGen: ArgBehaviour =
        fun dto ->
            match dto.flags.tryFirst<string> fileFlag with
            | Some file -> In <- ReadonlyLinesBuffer(ByFilePath file)
            | None -> In <- ReadonlyLinesBuffer ByStdIn

            match dto.flags.tryFirst<string> labelFlag with
            | Some label -> Debug.through label |> ignore
            | None -> ()

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
                        Debug.through ex |> ignore)

    CmdEntry(cmdName, cmdInfo)
    |> CmdEntry.acceptCombos combos exeGen
