module Regl.CommandLine.Commands.Copy

open System
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds
open TextCopy
open Regl.CommandLine.IO

let cmdName = "copy"

let cmdInfo = "copies piped input to clipboard"

let entry =
    let exeCopy : ArgBehaviour =
        fun dto ->
            In <- ReadonlyLinesBuffer ByStdIn
            ClipboardService.SetText In.all

    CmdEntry (cmdName, cmdInfo)
    |> _.addEntry(ArgEntry cmdName |> _.addBehaviour(exeCopy))
