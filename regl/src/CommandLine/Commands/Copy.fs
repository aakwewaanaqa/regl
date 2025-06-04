module Regl.CommandLine.Commands.Copy

open System
open TextCopy
open Regl.CommandLine.Shared
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

/// Copy piped input to clipboard
let copyCmd =
    let copyExe (result: ParseResult option) =
        match result with
        | Some result -> ClipboardService.SetText(readIn ())
        | None -> raise (Exception "copy can't be executed...")

    let builder = CommandBuilder("copy", copyExe)
    builder.usage <- Some "regl copy"
    builder.build ()