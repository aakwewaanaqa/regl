module Regl.CommandLine.Commands.RemoveEmpty

open System
open Regl.CommandLine.IO
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Shared
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let exe (result: ParseResult option) =
    match result with
    | Some _ ->
        In._lines
        |> List.filter (fun l -> not (l |> String.IsNullOrEmpty))
        |> List.iter (fun l -> Out.appendLine l)

        writeToPipe()
    | None -> raise (Exception "remove-empty can't be executed...")

let cmd =

    let builder = CommandBuilder("remove-empty", exe)
    builder.usage <- Some "regl remove-empty
    Execute the remove-empty command
    "
    builder.build ()
