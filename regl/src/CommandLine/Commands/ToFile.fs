module Regl.CommandLine.Commands.ToFile

open System
open System.IO
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let usage = "regl to-file <FILE-PATH> [--append]
    Writes piped input to a file
        --append : Appends writing
"

let exe (result: ParseResult option) =
    // Reads piped input
    InOut.In <- ReadonlyLinesBuffer(ByConsoleIn)

    match result with
    | Some r ->
        let path = r.getParam 0

        if r.hasFlag "--append" then
            File.AppendAllText(path, InOut.In.all)
        else
            File.WriteAllText(path, InOut.In.all)
    | None -> raise (Exception usage)

let cmd =
    let builder = CommandBuilder("to-file", exe)
    builder.usage <- Some usage
    builder.requiredParamsCount <- 1
    builder.build ()
