module Regl.CommandLine.Commands.Split

open System.Text.RegularExpressions
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let usage =
    "regl split <DELIMITER>
    Splits piped input using specified delimiter
    then outputs them into lines"

let exe (r : CommandParseResult) =
    // Reads piped input
    InOut.In <- ReadonlyLinesBuffer (ByConsoleIn)

    r.getParam 0
    |> Regex
    |> _.Split(InOut.In.all)
    |> List.ofArray
    |> List.map (fun l -> ternary (r.hasFlag "--quote") $"\"{l}\"" l)
    |> fun e -> InOut.Out.lines <- e

let cmd =
    let builder = CommandBuilder ("split", exe)
    builder.parameters <- [ Param("<DELIMITER>") ]
    builder.optionalFlags <- [ OnFlag("--quote") ]
    builder.usage <- usage
    builder.build ()
