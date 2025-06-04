module Regl.CommandLine.Commands.ToFile

open System
open System.IO
open Regl.CommandLine.IO
open Regl.CommandLine.Shared
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let exe (result: ParseResult option) =
    match result with
    | Some result ->
        let isAppend =
            result.flags |> Array.tryFind (fun f -> f.name = "--append") |> _.IsSome

        let path = result.parameters[0]

        if isAppend then
            File.AppendAllText(path, LinesReader.allLines ())
        else
            File.WriteAllText(path, LinesReader.allLines ())
    | None -> raise (Exception "to-file can't be executed...")

let cmd =
    let builder = CommandBuilder("to-file", exe)
    builder.usage <- Some "regl to-file [--append] <FILEPATH>"
    builder.requiredParamsCount <- 1
    builder.build ()