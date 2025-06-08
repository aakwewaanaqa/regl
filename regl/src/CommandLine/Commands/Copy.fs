module Regl.CommandLine.Commands.Copy

open System
open TextCopy
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let usage = "regl copy
    Copies piped input to clipboard"

let exe (_: CommandParseResult) =
    InOut.In <- ReadonlyLinesBuffer(ByConsoleIn)
    ClipboardService.SetText InOut.In.all

let cmd =
    let builder = CommandBuilder("copy", exe)
    builder.usage <- "regl copy"
    builder.build ()