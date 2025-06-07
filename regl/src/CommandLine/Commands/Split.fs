module Regl.CommandLine.Commands.Split

open System
open System.Text.RegularExpressions
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.IO
open TextCopy
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let usage =
    "regl split <DELIMITER>
    Splits piped input using specified delimiter
    then outputs them into lines"

let exe (result : ParseResult option) =
    // Reads piped input
    InOut.In <- ReadonlyLinesBuffer (ByConsoleIn)

    match result with
    | Some r ->
        let splits =
            r.getParam 0
            |> Regex
            |> _.Split(InOut.In.all)
            |> List.ofArray

        if r.hasFlag "--array" then
            splits
            |> List.mapi (fun i e ->
                if i = 0 then $"({e}\\"
                elif i = splits.Length then $" {e})"
                else $" {e}\\"
                )
            |> fun e -> InOut.Out.lines <- e
        else
            splits
            |> fun e -> InOut.Out.lines <- e
    | None -> raise (Exception usage)

let cmd =

    let builder = CommandBuilder ("split", exe)
    builder.requiredParamsCount <- 1
    builder.optionalFlags <- [ OnFlag ("--array") ]
    builder.usage <- Some usage
    builder.build ()
