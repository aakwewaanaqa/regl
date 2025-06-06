module Regl.CommandLine.Commands.ToFile

open System
open System.IO
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let exe (result: ParseResult option) =
    InOut.In <- ReadonlyLinesBuffer(ByConsoleIn)
    match result with
    | Some result ->
        let isAppend =
            result.flags |> Array.tryFind (fun f -> f.name = "--append") |> _.IsSome

        let path = result.parameters[0]

        if isAppend then
            File.AppendAllText(path, InOut.In.all)
        else
            File.WriteAllText(path, InOut.In.all)
    | None -> raise (Exception "to-file can't be executed...")

let cmd =
    let builder = CommandBuilder("to-file", exe)
    builder.usage <- Some "regl to-file [--append] <FILEPATH>"
    builder.requiredParamsCount <- 1
    builder.build ()