module Regl.CommandLine.Commands.Copy

open System
open TextCopy
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

///TODO : remove
[<Obsolete>]
let usage = "regl copy
    Copies piped input to clipboard"

///TODO : remove
[<Obsolete>]
let exe (_: CommandParseResult) =
    InOut.In <- ReadonlyLinesBuffer(ByConsoleIn)
    ClipboardService.SetText InOut.In.all

///TODO : remove
[<Obsolete>]
let cmd =
    let builder = CommandBuilder("copy", exe)
    builder.usage <- "regl copy"
    builder.build ()

//TODO : write entry