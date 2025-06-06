module Regl.CommandLine.Commands.RemoveEmpty

open System
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let exe (result: ParseResult option) =
    InOut.In <- ReadonlyLinesBuffer(ByConsoleIn)
    match result with
    | Some _ ->
        InOut.In.lines
        |> List.filter (fun l -> not (l |> String.IsNullOrEmpty))
        |> List.iter (fun l -> InOut.Out.appendLine l)

        InOut.Out.sendToPipe()
    | None -> raise (Exception "remove-empty can't be executed...")

let cmd =

    let builder = CommandBuilder("remove-empty", exe)
    builder.usage <- Some "regl remove-empty
    Execute the remove-empty command
    "
    builder.build ()
