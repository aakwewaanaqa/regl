module Regl.CommandLine.Commands.Copy

open System
open TextCopy
open Regl.CommandLine.IO
open Regl.CommandLine.Shared
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let exe (result: ParseResult option) =
    match result with
    | Some _ -> ClipboardService.SetText (InOut.In.all())
    | None -> raise (Exception "copy can't be executed...")

let cmd =
    let builder = CommandBuilder("copy", exe)
    builder.usage <- Some "regl copy"
    builder.build ()