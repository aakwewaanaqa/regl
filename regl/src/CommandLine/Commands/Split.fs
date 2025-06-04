module Regl.CommandLine.Commands.Split

open System
open System.Text.RegularExpressions
open TextCopy
open Regl.CommandLine.Shared
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let exe (result: ParseResult option) =
    match result with
    | Some result ->
        let out =
            Regex(result.parameters[0])
            |> _.Split(readIn ())
            |> Array.reduce (fun a b -> $"{a}\n{b}")

        writeOut out
    | None -> raise (Exception "split can't be executed...")

let cmd =

    let builder = CommandBuilder("split", exe)
    builder.requiredParamsCount <- 1
    builder.usage <- Some "regl split <DELIMITER>"
    builder.build ()
