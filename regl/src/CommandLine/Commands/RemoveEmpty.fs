module Regl.CommandLine.Commands.RemoveEmpty

open System
open Regl.CommandLine.IO
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds

let cmdName = "remove-empty"

let cmdInfo = "removes empty lines from stdin and writes to stdout"

let entry =
    let exeRemoveEmpty : ArgBehaviour =
        fun dto ->
            InOut.In <- ReadonlyLinesBuffer ByStdIn

            InOut.In.lines
            |> List.filter (fun l -> not (l |> String.IsNullOrEmpty))
            |> List.iter (fun l -> InOut.Out.appendLine l)

    CmdEntry(cmdName)
        .addInfo(cmdInfo)
        .addEntry(ArgEntry()
            .addBehaviour(exeRemoveEmpty))
