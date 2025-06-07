module Regl.CommandLine.Commands.Copy

open System
open TextCopy
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let usage = "regl copy
    Copies piped input to clipboard"

let exe (result: ParseResult option) =
    InOut.In <- ReadonlyLinesBuffer(ByConsoleIn)

    match result with
    | Some r -> ClipboardService.SetText (InOut.In.all)
    | None -> raise (Exception "copy can't be executed...")

let cmd =
    let builder = CommandBuilder("copy", exe)
    builder.usage <- Some "regl copy"
    builder.build ()