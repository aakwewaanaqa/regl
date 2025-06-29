module Regl.CommandLine.Commands.Copy

open System
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds
open TextCopy
open Regl.CommandLine.IO

let cmdName = "copy"

let cmdInfo = "copies piped input to clipboard"

let cmdNotice = "this commands rely on xsel on linux platform"

let entry =
    let exeCopy : ArgBehaviour =
        fun _ ->
            In <- ReadonlyLinesBuffer ByStdIn
            ClipboardService.SetText In.all

    CmdEntry(cmdName)
        .addInfo(cmdInfo)
        .addNotice(cmdNotice)
        .addEntry(ArgEntry()
            .addBehaviour(exeCopy))
