module Regl.CommandLine.Commands.Copy

open System
open TextCopy
open Regl.CommandLine.Shared
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let exe (result: ParseResult option) =
    match result with
    | Some result -> ClipboardService.SetText(readIn ())
    | None -> raise (Exception "copy can't be executed...")

let cmd =
    let builder = CommandBuilder("copy", exe)
    builder.usage <- Some "regl copy"
    builder.build ()