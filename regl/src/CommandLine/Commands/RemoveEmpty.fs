module Regl.CommandLine.Commands.RemoveEmpty

open System
open Regl.CommandLine.IO
open Regl.CommandLine.Shared
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let exe (result: ParseResult option) =
    match result with
    | Some _ ->
        let out =
            LinesReader.allLines()
            |> _.Split("\n", StringSplitOptions.RemoveEmptyEntries)
            |> Array.reduce (fun a b -> $"{a}\n{b}")

        writeOut out
    | None -> raise (Exception "remove-empty can't be executed...")

let cmd =

    let builder = CommandBuilder("remove-empty", exe)
    builder.usage <- Some "regl remove-empty
    Execute the remove-empty command
    "
    builder.build ()
